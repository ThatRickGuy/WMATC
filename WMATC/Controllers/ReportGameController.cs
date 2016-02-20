using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMATC.Models;

namespace WMATC.Controllers
{
    public class ReportGameController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReportGame
        public ActionResult Index()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");
            int SelectedEventId;
            int.TryParse(Session["SelectedEventId"].ToString(), out SelectedEventId);
            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);


            var matchups = (from p in db.Matchups where p.RoundTeamMatchup.RoundId== RoundID && p.RoundTeamMatchup.Round.EventId == SelectedEventId select p).Include(m => m.Player1).Include(m => m.Player2).Include(m => m.RoundTeamMatchup).Include(m => m.Winner);
            return View(matchups.ToList());
        }
        
        // POST: ReportGame/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchupId,RoundTeamMatchupId,Player1Id,Player2Id,WinnerId,Player1List,Player2List")] Matchup matchup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Player1Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player1Id);
            ViewBag.Player2Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player2Id);
            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);
            ViewBag.WinnerId = new SelectList(db.Players, "PlayerId", "Name", matchup.WinnerId);
            return View(matchup);
        }

        // GET: ReportGame/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matchup matchup = db.Matchups.Find(id);
            if (matchup == null)
            {
                return HttpNotFound();
            }
            return View(matchup);
        }

        // POST: ReportGame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matchup matchup = db.Matchups.Find(id);
            db.Matchups.Remove(matchup);
            db.SaveChanges();
            return RedirectToAction("Index");
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
