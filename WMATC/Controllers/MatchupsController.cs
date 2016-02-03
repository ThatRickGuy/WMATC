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
    public class MatchupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Matchups
        [Authorize(Roles = "canEdit")]
        public ActionResult Index()
        {
            var matchups = db.Matchups.Include(m => m.Player1).Include(m => m.Player2).Include(m => m.RoundTeamMatchup).Include(m => m.Winner);
            return View(matchups.ToList());
        }

        // GET: Matchups/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
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

        // GET: Matchups/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            ViewBag.Player1Id = new SelectList(db.Players, "PlayerId", "Name");
            ViewBag.Player2Id = new SelectList(db.Players, "PlayerId", "Name");
            ViewBag.RoundTeamMatchupId = new SelectList((from p in db.RoundTeamMatchups select new ViewModels.KeyValuePair() { Key = p.RoundTeamMatchupId, Value =  "<span id=" + p.Team1.TeamId + ">"+ p.Team1.Name + "</span> vs <span id=" + p.Team2.TeamId + ">" + p.Team2.Name + "</span>"}).ToList(), "Key", "Value");
            ViewBag.WinnerId = new SelectList(db.Players, "PlayerId", "Name");
            return View();
        }

        // POST: Matchups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MatchupId,RoundTeamMatchupId,Player1Id,Player2Id,WinnerId,Player1List,Player2List")] Matchup matchup)
        {
            if (ModelState.IsValid)
            {
                db.Matchups.Add(matchup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Player1Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player1Id);
            ViewBag.Player2Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player2Id);
            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);
            ViewBag.WinnerId = new SelectList(db.Players, "PlayerId", "Name", matchup.WinnerId);
            return View(matchup);
        }

        // GET: Matchups/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
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
            ViewBag.Player1Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player1Id);
            ViewBag.Player2Id = new SelectList(db.Players, "PlayerId", "Name", matchup.Player2Id);
            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);
            ViewBag.WinnerId = new SelectList(db.Players, "PlayerId", "Name", matchup.WinnerId);
            return View(matchup);
        }

        // POST: Matchups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
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

        // GET: Matchups/Delete/5
        [Authorize(Roles = "canEdit")]
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

        // POST: Matchups/Delete/5
        [Authorize(Roles = "canEdit")]
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
