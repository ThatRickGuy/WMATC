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
    public class RoundTeamMatchupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RoundTeamMatchups
        [Authorize(Roles = "canEdit")]
        public ActionResult Index()
        {
            var roundTeamMatchups = db.RoundTeamMatchups.Include(r => r.Round).Include(r => r.Team1).Include(r => r.Team2);
            return View(roundTeamMatchups.ToList());
        }

        // GET: RoundTeamMatchups/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundTeamMatchup roundTeamMatchup = db.RoundTeamMatchups.Find(id);
            if (roundTeamMatchup == null)
            {
                return HttpNotFound();
            }
            return View(roundTeamMatchup);
        }

        // GET: RoundTeamMatchups/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            ViewBag.RoundId = new SelectList(db.Rounds, "RoundId", "Scenario");
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name");
            return View();
        }

        // POST: RoundTeamMatchups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoundTeamMatchupId,RoundId,Team1Id,Team2Id")] RoundTeamMatchup roundTeamMatchup)
        {
            if (ModelState.IsValid)
            {
                db.RoundTeamMatchups.Add(roundTeamMatchup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoundId = new SelectList(db.Rounds, "RoundId", "Scenario", roundTeamMatchup.RoundId);
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team2Id);
            return View(roundTeamMatchup);
        }

        // GET: RoundTeamMatchups/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundTeamMatchup roundTeamMatchup = db.RoundTeamMatchups.Find(id);
            if (roundTeamMatchup == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoundId = new SelectList(db.Rounds, "RoundId", "Scenario", roundTeamMatchup.RoundId);
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team2Id);
            return View(roundTeamMatchup);
        }

        // POST: RoundTeamMatchups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoundTeamMatchupId,RoundId,Team1Id,Team2Id")] RoundTeamMatchup roundTeamMatchup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roundTeamMatchup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoundId = new SelectList(db.Rounds, "RoundId", "Scenario", roundTeamMatchup.RoundId);
            ViewBag.Team1Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team1Id);
            ViewBag.Team2Id = new SelectList(db.Teams, "TeamId", "Name", roundTeamMatchup.Team2Id);
            return View(roundTeamMatchup);
        }

        // GET: RoundTeamMatchups/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoundTeamMatchup roundTeamMatchup = db.RoundTeamMatchups.Find(id);
            if (roundTeamMatchup == null)
            {
                return HttpNotFound();
            }
            return View(roundTeamMatchup);
        }

        // POST: RoundTeamMatchups/Delete/5
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoundTeamMatchup roundTeamMatchup = db.RoundTeamMatchups.Find(id);
            db.RoundTeamMatchups.Remove(roundTeamMatchup);
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
