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
using WMATC.ViewModels;

namespace WMATC.Controllers
{
    public class PublishController : Controller
    {

        private ApplicationDbContext db;

        // GET: Publish
        public ActionResult Index()
        {
            db = new ApplicationDbContext();

            if (Session["SelectedEventId"] == null) return Redirect("Events");
            int SelectedEventId;
            int.TryParse(Session["SelectedEventId"].ToString(), out SelectedEventId);


            var model = new PublishViewModel();
            var PublishView = PopulatePublishViewModel_Matches(model, SelectedEventId);
            PublishView = PopulatePublishViewModel_Players(model, SelectedEventId);
            var TeamsView = PopulatePublishViewModel_Teams(SelectedEventId);
            var StandingsView = PopulatePublishViewModel_Standings(SelectedEventId);
            var PlayerViewModel = (from p in PublishView.Players select p).ToList();


            var js = RenderRazorViewToString(this.ControllerContext, "PublishListJS", PlayerViewModel);
            if (!System.IO.Directory.Exists(Server.MapPath(".") + "\\StaticEvents\\")) System.IO.Directory.CreateDirectory(Server.MapPath(".") + "\\StaticEvents\\");
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + ".js", js);

            var rounds = RenderRazorViewToString(this.ControllerContext, "Index", PublishView);
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + "rounds.html", rounds);

            var teams = RenderRazorViewToString(this.ControllerContext, "TeamBrowser", TeamsView);
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + "teams.html", teams);

            var standings = RenderRazorViewToString(this.ControllerContext, "Standings", StandingsView);
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + "standings.html", standings);

            return View(PublishView);
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

        private TeamBrowser PopulatePublishViewModel_Teams(int EventID)
        {
            var Event = (from p in db.Events where p.EventId == EventID select p).First();
            var Model = new TeamBrowser();
            Model.EventDate = Event.EventDate;
            Model.EventTitle = Event.Title;
            Model.EventImageURL = Event.ImageURL;
            Model.Teams = (from p in db.Teams where p.EventId == EventID select new TeamBrowser.Team() { TeamImageURL = p.ImgURL, TeamName = p.Name }).ToList();
            foreach (var team in Model.Teams)
            {
                team.Players = (from p in db.Players where p.Team.Name == team.TeamName orderby p.Name select p).ToList();
            }
            return Model;
        }

        private PublishViewModel PopulatePublishViewModel_Players(PublishViewModel Model, int EventID)
        {
            var Players = from p in db.Players where p.Team.EventId == EventID select new PlayerViewModel { Player = p, PlayerId = p.PlayerId };
            Model.Players = Players.ToList();
            return Model;
        }

        private PublishViewModel PopulatePublishViewModel_Matches(PublishViewModel Model, int EventID)
        {
            var MyEvent = (from p in db.Events where p.EventId == EventID select p).First();

            Model.EventDate = MyEvent.EventDate;
            Model.EventTitle = MyEvent.Title;
            Model.EventImageURL = MyEvent.ImageURL;

            Model.Rounds = (from p in db.Rounds where p.EventId == MyEvent.EventId orderby p.Sequence select new RoundViewModel() { Sequence = p.Sequence, Scenario = p.Scenario, RoundID = p.RoundId }).ToList<RoundViewModel>();
            foreach (RoundViewModel Round in Model.Rounds)
            {
                Round.TeamMatchups = (from p in db.RoundTeamMatchups
                                      where p.RoundId == Round.RoundID
                                      orderby p.Team1.Name
                                      select new TeamMatchupViewModel()
                                      {
                                          RoundTeamMatchupID = p.RoundTeamMatchupId,
                                          Team1Name = p.Team1.Name,
                                          Team1ImageURL = p.Team1.ImgURL,
                                          Team1IsWinner = "BlankBackground",
                                          Team2Name = p.Team2.Name,
                                          Team2ImageURL = p.Team2.ImgURL,
                                          Team2IsWinner = "BlankBackground",
                                          TableZone = (p.TableZone.HasValue) ? p.TableZone.Value : 0
                                      }).ToList<TeamMatchupViewModel>();

                foreach (TeamMatchupViewModel TeamMatchup in Round.TeamMatchups)
                {
                    TeamMatchup.Matchups = (from p in db.Matchups
                                            where p.RoundTeamMatchupId == TeamMatchup.RoundTeamMatchupID
                                            select new MatchupViewModel()
                                            {
                                                Player1FactionURL = p.Player1.Faction.ImageURL,
                                                Player1Faction = p.Player1.Faction.Title,
                                                Player1Caster = (p.Player1List == null) ? "" : (p.Player1List == 1) ? p.Player1.Caster1 : (p.Player1List == 2) ? p.Player1.Caster2 : "",
                                                Player1ListNumber = (p.Player1List == null) ? 0 : p.Player1List.Value,
                                                Player1Name = p.Player1.Name,
                                                Player1Id = p.Player1Id,
                                                Player1IsWinner = (p.WinnerId == p.Player1Id) ? "WinnerBackground" : (p.WinnerId == null) ? "BlankBackground" : "LoserBackground",
                                                Player2FactionURL = p.Player2.Faction.ImageURL,
                                                Player2Faction = p.Player2.Faction.Title,
                                                Player2Caster = (p.Player2List == null) ? "" : (p.Player2List == 1) ? p.Player2.Caster1 : (p.Player2List == 2) ? p.Player2.Caster2 : "",
                                                Player2ListNumber = (p.Player1List == null) ? 0 : p.Player2List.Value,
                                                Player2Name = p.Player2.Name,
                                                Player2Id = p.Player2Id,
                                                Player2IsWinner = (p.WinnerId == p.Player2Id) ? "WinnerBackground" : (p.WinnerId == null) ? "BlankBackground" : "LoserBackground"
                                            }).ToList<MatchupViewModel>();
                }
                foreach (TeamMatchupViewModel TeamMatchup in Round.TeamMatchups)
                {
                    TeamMatchup.Team1Wins = (from p in TeamMatchup.Matchups where p.Player1IsWinner == "WinnerBackground" select p).Count();
                    TeamMatchup.Team2Wins = (from p in TeamMatchup.Matchups where p.Player2IsWinner == "WinnerBackground" select p).Count();
                    if (TeamMatchup.Team1Wins > TeamMatchup.Matchups.Count / 2 && TeamMatchup.Team1Wins + TeamMatchup.Team2Wins == TeamMatchup.Matchups.Count())
                    {
                        TeamMatchup.Team1IsWinner = "WinnerBackground";
                        TeamMatchup.Team2IsWinner = "LoserBackground";
                    }
                    else if (TeamMatchup.Team2Wins > TeamMatchup.Matchups.Count / 2 && TeamMatchup.Team1Wins + TeamMatchup.Team2Wins == TeamMatchup.Matchups.Count())
                    {
                        TeamMatchup.Team1IsWinner = "LoserBackground";
                        TeamMatchup.Team2IsWinner = "WinnerBackground";
                    }
                    else
                    {
                        TeamMatchup.Team1IsWinner = "BlankBackground";
                        TeamMatchup.Team2IsWinner = "BlankBackground";
                    }
                }
            }
            return Model;
        }


        private Standings PopulatePublishViewModel_Standings(int EventID)
        {
            var Event = (from p in db.Events where p.EventId == EventID select p).First();
            var Model = new Standings();
            Model.EventDate = Event.EventDate;
            Model.EventTitle = Event.Title;
            Model.EventImageURL = Event.ImageURL;
            Model.Teams = (from p in db.Teams where p.EventId == EventID select new Standings.Team() { TeamImageURL = p.ImgURL, TeamName = p.Name, TeamId = p.TeamId }).ToList();

            //Round trickery. All rounds will likely be created prior to the event, so we need to figure out which round we're actually on
            var Rounds = (from p in db.Rounds where p.EventId == Event.EventId orderby p.Sequence select p ).ToList();
            var CurrentRound = 0;
            foreach (var round in Rounds )
            {
                CurrentRound += 1;
                var UnreportedGames = (from p in db.Matchups where p.RoundTeamMatchup.RoundId == round.RoundId && p.WinnerId == null select p).Count();
                var ReportedGames = (from p in db.Matchups where p.RoundTeamMatchup.RoundId == round.RoundId && p.WinnerId != null select p).Count();
                if (UnreportedGames > 0 || (UnreportedGames ==0 && ReportedGames ==0))
                {
                    // there are unreported games, so we're going to break, but we need to know if this round has /any/ reported games. 
                    // If not, it's a new round, and so standings should still reflect last round.
                    if (ReportedGames == 0) CurrentRound -= 1;
                    break;
                }
            }
            Model.CurrentRound = CurrentRound;
            

            foreach (var team in Model.Teams)
            {
                team.Players = (from p in db.Players where p.TeamId == team.TeamId orderby p.Name select new Standings.Player() { Name = p.Name, Faction = p.Faction.Title, FactionImageURL = p.Faction.ImageURL }).ToList();


                // player stats
                foreach (var player in team.Players)
                {
                    player.TP = GetPlayerWins(player.PlayerId);
                    var Matches = (from p in db.Matchups where p.Player1Id == player.PlayerId || p.Player2Id == player.PlayerId select p).ToList();
                    foreach (var match in Matches)
                    {
                        if (match.Player1Id == player.PlayerId)
                        {
                            player.StrengthOfSchedule += GetPlayerWins(match.Player1Id);
                            player.APD += match.Player1APD.Value;
                            player.CP += match.Player1CP.Value;
                        }
                        if (match.Player2Id == player.PlayerId)
                        {
                            player.StrengthOfSchedule += GetPlayerWins(match.Player2Id);
                            player.APD += match.Player2APD.Value;
                            player.CP += match.Player2CP.Value;
                        }
                    }
                }

                // Team stats
                team.TeamWins = GetTeamWins(team.TeamId);
                var TeamMatches = (from p in db.RoundTeamMatchups where p.Team1Id == team.TeamId || p.Team2Id == team.TeamId select p).ToList();
                foreach (var teamMatch in TeamMatches)
                {
                    if (teamMatch.Team1Id == team.TeamId) team.StrengthOfSchedule += GetTeamWins(teamMatch.Team2Id);
                    if (teamMatch.Team2Id == team.TeamId) team.StrengthOfSchedule += GetTeamWins(teamMatch.Team1Id);
                }
                team.TP = (from p in team.Players select p.TP).Sum();
                team.CP = (from p in team.Players select p.CP).Sum();
                team.APD = (from p in team.Players select p.APD).Sum();

            }
            //Model.Teams = (from p in Model.Teams orderby p.TeamWins descending, p.StrengthOfSchedule descending, p.TP descending, p.CP descending, p.APD descending select p).ToList();
            return Model;
        }


        private int GetPlayerWins(int PlayerID)
        {
            int iReturn = 0;

            iReturn = (from p in db.Matchups where p.WinnerId == PlayerID select p).Count();

            return iReturn;
        }
        private int GetTeamWins(int TeamID)
        {
            int iReturn = 0;

            var TeamMatches = (from p in db.RoundTeamMatchups where p.Team1Id == TeamID || p.Team2Id == TeamID select p).ToList();
            foreach (var teamMatch in TeamMatches)
            {
                var MatchupCount = (from p in db.Matchups where p.RoundTeamMatchupId == teamMatch.RoundTeamMatchupId select p).Count();
                int Wins = 0;
                if (teamMatch.Team1Id == TeamID) Wins = (from p in db.Matchups where p.RoundTeamMatchupId == teamMatch.RoundTeamMatchupId && p.Player1Id == p.WinnerId select p).Count();
                if (teamMatch.Team2Id == TeamID) Wins = (from p in db.Matchups where p.RoundTeamMatchupId == teamMatch.RoundTeamMatchupId && p.Player2Id == p.WinnerId select p).Count();

                if (Wins > MatchupCount / 2) iReturn += 1;
            }

            return iReturn;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
