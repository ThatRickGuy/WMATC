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

            var PlayerViewModel = (from p in PublishView.Players select p).ToList();

            var js = RenderRazorViewToString(this.ControllerContext, "PublishListJS", PlayerViewModel);
            if (!System.IO.Directory.Exists(Server.MapPath(".") + "\\StaticEvents\\")) System.IO.Directory.CreateDirectory(Server.MapPath(".") + "\\StaticEvents\\");
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + ".js", js);

            var rounds = RenderRazorViewToString(this.ControllerContext, "Index", PublishView);
            System.IO.File.WriteAllText(Server.MapPath(".") + "\\StaticEvents\\" + PublishView.EventTitle + "rounds.html", rounds);

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
                                          Team2IsWinner = "BlankBackground"
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
    }
}
