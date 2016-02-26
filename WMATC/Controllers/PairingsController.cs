using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMATC.Models;

namespace WMATC.Controllers
{
    public class PairingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Pairings
        [Authorize(Roles = "canEdit")]
        public ActionResult PrintLanding()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            
            var Round = (from p in db.Rounds where p.RoundId == RoundID select p).Include(m=>m.Event).FirstOrDefault();
            var RoundTeamMatchups = (from p in db.RoundTeamMatchups where p.RoundId == RoundID select p).Include(m => m.Team1).Include(m => m.Team2).ToList();

            var rtmp = new ViewModels.RoundTeamMatchupPrint();
            rtmp.EventDate = Round.Event.EventDate;
            rtmp.EventImageURL = Round.Event.ImageURL;
            rtmp.EventName = Round.Event.Title;
            rtmp.RoundNumber = Round.Sequence;
            rtmp.RoundScenario = Round.Scenario;
            rtmp.RoundTeamMatchupId = RoundTeamMatchups.First().RoundTeamMatchupId;

            var AllTeams = new List<ViewModels.PrintPairing >();
            foreach (var rtm in RoundTeamMatchups)
            {
                AllTeams.Add(new ViewModels.PrintPairing() { Team1Name = rtm.Team1.Name, Team2Name = rtm.Team2.Name, Team1Id = rtm.Team1Id, TableZone = rtm.TableZone.Value  });
                AllTeams.Add(new ViewModels.PrintPairing() { Team1Name = rtm.Team2.Name, Team2Name = rtm.Team1.Name, Team1Id = rtm.Team2Id, TableZone = rtm.TableZone.Value });
            }

            AllTeams = (from p in AllTeams orderby p.Team1Name select p).ToList();
            int MedianIndex = AllTeams.Count / 2;

            rtmp.LeftList = new List<ViewModels.PrintPairing>();
            rtmp.RightList = new List<ViewModels.PrintPairing>();

            rtmp.LeftList.AddRange(AllTeams.GetRange(0, MedianIndex));
            rtmp.RightList.AddRange(AllTeams.GetRange( MedianIndex, AllTeams.Count - MedianIndex));
            
            var output = RenderRazorViewToString(this.ControllerContext, "Print", rtmp);
            if (!System.IO.Directory.Exists(Server.MapPath(".\\..") + "\\StaticEvents\\")) System.IO.Directory.CreateDirectory(Server.MapPath(".") + "\\StaticEvents\\");
            System.IO.File.WriteAllText(Server.MapPath(".\\..") + "\\StaticEvents\\" + rtmp.EventName + "_Round" + rtmp.RoundNumber + ".html", output);

            var rtmpu = new ViewModels.RoundTeamMatchupPrintURL();
            rtmpu.PrintURL = "/StaticEvents/" + rtmp.EventName + "_Round" + rtmp.RoundNumber + ".html";

            return View(rtmpu);
        }


        // GET: Pairings
        [Authorize(Roles = "canEdit")]
        public ActionResult Generate()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            var Round = (from p in db.Rounds where p.RoundId == RoundID select p).FirstOrDefault();
            return View(Round);
        }

        // GET: Pairings
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Generate")]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateConfirmed()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            db.RoundTeamMatchups.RemoveRange(from p in db.RoundTeamMatchups where p.RoundId == RoundID && p.Round.EventId == EventID select p);
            db.SaveChanges();

            var Rounds = (from p in db.Rounds where p.EventId == EventID orderby p.Sequence select p).ToList();

            //Hold a list of Teams and the number of Wins they have
            var TeamWins = new Dictionary<Team, int>();

            foreach (var round in Rounds)
            {
                var roundTeamMatchups = (from p in db.RoundTeamMatchups
                                         where p.RoundId == round.RoundId
                                         select p).Include(m => m.Team1).Include(m => m.Team2).ToList();


                foreach (var roundTeamMatchup in roundTeamMatchups)
                {

                    if (!TeamWins.ContainsKey(roundTeamMatchup.Team1)) TeamWins.Add(roundTeamMatchup.Team1, 0);
                    if (!TeamWins.ContainsKey(roundTeamMatchup.Team2)) TeamWins.Add(roundTeamMatchup.Team2, 0);

                    var Team1Wins = (from p in db.Matchups where p.RoundTeamMatchupId == roundTeamMatchup.RoundTeamMatchupId && p.WinnerId == p.Player1Id select p).Count();
                    var Team2Wins = (from p in db.Matchups where p.RoundTeamMatchupId == roundTeamMatchup.RoundTeamMatchupId && p.WinnerId == p.Player2Id select p).Count();
                    var MatchCounts = (from p in db.Matchups where p.RoundTeamMatchupId == roundTeamMatchup.RoundTeamMatchupId select p).Count();

                    if (Team1Wins > Team2Wins && Team1Wins > MatchCounts / 2) TeamWins[roundTeamMatchup.Team1] += 1;
                    if (Team2Wins > Team1Wins && Team2Wins > MatchCounts / 2) TeamWins[roundTeamMatchup.Team1] += 1;
                }
            }

            var UnPairedTeams = (from p in TeamWins select p.Key).ToList();
            

            var rand = new Random();
            var RetryCount = 0;


            // this loop will continue until all teams have been successfully paired. 
            // If something prevents a full pairing (crap random # generator, or invalid data, or a bug) it will try to run the pairings engine 5 times before failing.
            while (UnPairedTeams.Count > 0)
            {
                RetryCount += 1;
                if (RetryCount > 5)
                {
                    //Something is preventing all pairings
                    throw new Exception("Unable to complete pairings! Please use manual Round Team Matchups.");
                }

                var Wins = (from p in TeamWins orderby p.Value descending select p.Value).FirstOrDefault();

                while (Wins > -1)
                {
                    // Pre pairing cleanup, identify pairdown and byes
                    var AvailableTeams = (from p in TeamWins where p.Value == Wins select p).ToList();
                    if (AvailableTeams.Count() % 2 != 0)
                    {
                        if (Wins > 0)
                        {
                            //Pairdown Logic
                            var PairdownCandidates = from p in AvailableTeams where p.Key.HasBeenPairedDown == false select p.Key;
                            var Pairdown = PairdownCandidates.ElementAt(rand.Next(PairdownCandidates.Count()));
                            Pairdown.HasBeenPairedDown = true;
                            TeamWins[Pairdown]-=1; //force the team into the next lower wins bucket
                            AvailableTeams = (from p in TeamWins where p.Value >= Wins select p).ToList();
                        }
                        else
                        {
                            //Bye logic
                            var PairdownCandidates = from p in AvailableTeams where p.Key.HasBeenPairedDown == false select p.Key;
                            var Bye = PairdownCandidates.ElementAt(rand.Next(PairdownCandidates.Count()));
                            Bye.HasBeenBye = true;
                            TeamWins.Remove(Bye);
                            AvailableTeams = (from p in TeamWins where p.Value >= Wins select p).ToList();
                        }
                    }

                    // Find pairings for all available teams (in this win bucket)
                    while (AvailableTeams.Count > 0)
                    {

                        // get a random team from this wins bucket
                        var Team1 = AvailableTeams.ElementAt(rand.Next(AvailableTeams.Count()));

                        // build a list of previous opponents
                        var PreviousOpponents = (from p in db.RoundTeamMatchups where p.Team1.TeamId == Team1.Key.TeamId select p.Team2).ToList();
                        PreviousOpponents.AddRange((from p in db.RoundTeamMatchups where p.Team2.TeamId == Team1.Key.TeamId select p.Team1).ToList());

                        // pull a list of possible opponents (teams in this win bucket excluding previous opponents)
                        var AvailableOpponents = (from p in AvailableTeams where !PreviousOpponents.Contains(p.Key) && p.Key != Team1.Key select p).ToList();
                        if (AvailableOpponents.Count() == 0)
                        {
                            // something really bad just happened.
                            throw new Exception("No valid opponents.");
                        }

                        // get a random team from the available opponents bucket
                        var Team2 = AvailableOpponents.ElementAt(rand.Next(AvailableOpponents.Count()));

                        // get a list of all tablezones played on by either team
                        var PreviousTables = (from p in db.RoundTeamMatchups where p.Team1Id == Team1.Key.TeamId || p.Team2Id == Team1.Key.TeamId || p.Team1Id == Team2.Key.TeamId || p.Team2Id == Team2.Key.TeamId select p.TableZone).ToList();

                        // get a list of all tablezones in use thie round
                        var UsedTables = (from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.TableZone).ToList();

                        // get the max tablezone #
                        var MaxTableZoneNumber = ((from p in db.Teams where p.EventId == EventID select p).Count() / 2);

                        // get a list of ALL tables
                        var AvailableTables = Enumerable.Range(1, MaxTableZoneNumber).ToList();

                        // Get all tables not currently in use
                        AvailableTables = (from p in AvailableTables where !UsedTables.Contains(p) select p).ToList();

                        var FreshTables = (from p in AvailableTables where !PreviousTables.Contains(p) select p).ToList();
                        if (FreshTables.Count() == 0)
                        {
                            // No fresh tables available, someone is getting a repeat
                            FreshTables = AvailableTables;
                        }
                        if (FreshTables.Count() == 0)
                        {
                            // Something really bad just happened. 
                            throw new Exception("No valid tables.");
                        }

                        var Table = FreshTables.ElementAt(rand.Next(FreshTables.Count()));

                        // save the pairing
                        var RoundTeamMatchup = new RoundTeamMatchup();
                        RoundTeamMatchup.RoundId = RoundID;
                        RoundTeamMatchup.Team1Id = Team1.Key.TeamId;
                        RoundTeamMatchup.Team2Id = Team2.Key.TeamId;
                        RoundTeamMatchup.TableZone = Table;
                        db.RoundTeamMatchups.Add(RoundTeamMatchup);
                        db.SaveChanges();

                        //Cleanup for next pass
                        AvailableTeams.Remove(Team1);
                        AvailableTeams.Remove(Team2);
                        UnPairedTeams.Remove(Team1.Key);
                        UnPairedTeams.Remove(Team2.Key);
                    }
                    Wins -= 1;
                }
            }

            return RedirectToAction("Index", "RoundTeamMatchups");
        }



        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
