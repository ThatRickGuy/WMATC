using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMATC.Models;
using WMATC.ViewModels;

namespace WMATC.Controllers
{
    public class GameReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GameReport
        //[Authorize(Roles = "canEdit")]
        public ActionResult Index()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");
            
            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);


            //Event @event = db.Events.Find(EventID);
            //if (@event.Owner != new Guid(User.Identity.GetUserId()))
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

            var GameReports = (from p in db.Matchups
                               where p.RoundTeamMatchup.RoundId == RoundID && p.RoundTeamMatchup.Round.EventId == EventID
                               select new ViewModels.GameReport()
                               {
                                   MatchupId = p.MatchupId,
                                   Players = new List<ViewModels.GameReport.SimplePlayer>() { new ViewModels.GameReport.SimplePlayer() { PlayerId = 0, PlayerName = "" }, new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player1Id, PlayerName = p.Player1.Name }, new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player2Id, PlayerName = p.Player2.Name } },
                                   WinnerId = p.WinnerId,

                                   Player1 = new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player1Id, PlayerName = p.Player1.Name },
                                   Player1APD = (p.Player1APD.HasValue) ? p.Player1APD.Value : 0,
                                   Player1CP = (p.Player1CP.HasValue) ? p.Player1CP.Value : 0,
                                   Player1ListId = (p.Player1List.HasValue) ? p.Player1List.Value : 0,
                                   Player1Lists = new List<ViewModels.GameReport.SimpleList>() { new ViewModels.GameReport.SimpleList() { ListId = 0, Warnoun = "" }, new ViewModels.GameReport.SimpleList() { ListId = 1, Warnoun = p.Player1.Caster1 }, new ViewModels.GameReport.SimpleList() { ListId = 2, Warnoun = p.Player1.Caster2 } },

                                   Player2 = new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player2Id, PlayerName = p.Player2.Name },
                                   Player2APD = (p.Player2APD.HasValue) ? p.Player2APD.Value : 0,
                                   Player2CP = (p.Player2CP.HasValue) ? p.Player2CP.Value : 0,
                                   Player2ListId = (p.Player2List.HasValue) ? p.Player1List.Value : 0,
                                   Player2Lists = new List<ViewModels.GameReport.SimpleList>() { new ViewModels.GameReport.SimpleList() { ListId = 0, Warnoun = "" }, new ViewModels.GameReport.SimpleList() { ListId = 1, Warnoun = p.Player2.Caster1 }, new ViewModels.GameReport.SimpleList() { ListId = 2, Warnoun = p.Player2.Caster2 } },

                                   FirstId = p.FirstPlayerID,
                                   GameLength = p.GameLength
                               }).ToList();
            return View(GameReports);
        }


        // GET: DeleteMe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matchup matchup = db.Matchups.Find(id);

            //Event @event = db.Events.Find(id);
            //if (@event.Owner != new Guid(User.Identity.GetUserId()))
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");


            var gr = (from p in db.Matchups
                      where p.MatchupId == id.Value
                      select new ViewModels.GameReport()
                      {
                          MatchupId = p.MatchupId,
                          Players = new List<ViewModels.GameReport.SimplePlayer>() { new ViewModels.GameReport.SimplePlayer() { PlayerId = 0, PlayerName = "" }, new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player1Id, PlayerName = p.Player1.Name }, new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player2Id, PlayerName = p.Player2.Name } },
                          WinnerId = p.WinnerId,

                          Player1 = new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player1Id, PlayerName = p.Player1.Name },
                          Player1APD = (p.Player1APD.HasValue) ? p.Player1APD.Value : 0,
                          Player1CP = (p.Player1CP.HasValue) ? p.Player1CP.Value : 0,
                          Player1ListId = (p.Player1List.HasValue) ? p.Player1List.Value : 0,
                          Player1Lists = new List<ViewModels.GameReport.SimpleList>() { new ViewModels.GameReport.SimpleList() { ListId = 0, Warnoun = "" }, new ViewModels.GameReport.SimpleList() { ListId = 1, Warnoun = p.Player1.Caster1 }, new ViewModels.GameReport.SimpleList() { ListId = 2, Warnoun = p.Player1.Caster2 } },

                          Player2 = new ViewModels.GameReport.SimplePlayer() { PlayerId = p.Player2Id, PlayerName = p.Player2.Name },
                          Player2APD = (p.Player2APD.HasValue) ? p.Player2APD.Value : 0,
                          Player2CP = (p.Player2CP.HasValue) ? p.Player2CP.Value : 0,
                          Player2ListId = (p.Player2List.HasValue) ? p.Player1List.Value : 0,
                          Player2Lists = new List<ViewModels.GameReport.SimpleList>() { new ViewModels.GameReport.SimpleList() { ListId = 0, Warnoun = "" }, new ViewModels.GameReport.SimpleList() { ListId = 1, Warnoun = p.Player2.Caster1 }, new ViewModels.GameReport.SimpleList() { ListId = 2, Warnoun = p.Player2.Caster2 } },

                          FirstId = (p.FirstPlayerID.HasValue) ? p.FirstPlayerID : null,
                          GameLength = (p.GameLength.HasValue) ? p.GameLength : null
            
                               }).ToList();

            if (gr.FirstOrDefault() == null)
            {
                return HttpNotFound();
            }
            return View(gr.FirstOrDefault());
        }

        // POST: DeleteMe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchupId,WinnerId,Player1CP,Player1APD,Player1ListId,Player2CP,Player2APD,Player2ListId,FirstId,GameLength")] GameReport gameReport)
        {
            if (ModelState.IsValid)
            {
                var m = db.Matchups.Find(gameReport.MatchupId);
                if (gameReport.WinnerId.HasValue && gameReport.WinnerId > 0 ) m.WinnerId = gameReport.WinnerId;
                m.Player1APD = gameReport.Player1APD;
                m.Player1CP = gameReport.Player1CP;
                m.Player1List = gameReport.Player1ListId;
                m.Player2APD = gameReport.Player2APD;
                m.Player2CP = gameReport.Player2CP;
                m.Player2List = gameReport.Player2ListId;
                m.FirstPlayerID = gameReport.FirstId;
                m.GameLength = gameReport.GameLength;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return null;
                //return RedirectToAction("Index");
            }
            return View(gameReport);
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
