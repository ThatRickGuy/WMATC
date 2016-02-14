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
        public ActionResult Index(int? id)
        {

            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null && id == null ) return Redirect("Rounds");

            if (id != null)
            {
                Session["SelectedRoundId"] = id;
                Session["SelectedRound"] = (from p in db.Rounds where p.RoundId == id select p.Sequence + ". " + p.Scenario ).FirstOrDefault();
            }

            ActionResult rView = null;

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            var roundTeamMatchups = (from p in db.RoundTeamMatchups where p.RoundId == RoundID select p).Include(r => r.Round).Include(r => r.Team1).Include(r => r.Team2);
            rView = View(roundTeamMatchups.ToList());
            
            return rView;          
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
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            ViewBag.RoundId = new SelectList((from p in db.Rounds where p.RoundId == RoundID select p).ToList(), "RoundId", "Scenario");

            var AvailableTeams = from p in db.Teams where p.EventId == EventID select p;
            var Team1s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team1Id;
            var Team2s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team2Id;
            AvailableTeams = from p in AvailableTeams where !Team1s.Contains(p.TeamId) && !Team2s.Contains(p.TeamId) select p;

            ViewBag.Team1Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
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
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            ViewBag.RoundId = new SelectList((from p in db.Rounds where p.RoundId == RoundID select p).ToList(), "RoundId", "Scenario");

            var AvailableTeams = from p in db.Teams where p.EventId == EventID select p;
            var Team1s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team1Id;
            var Team2s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team2Id;
            AvailableTeams = from p in AvailableTeams where !Team1s.Contains(p.TeamId) && !Team2s.Contains(p.TeamId) select p;

            ViewBag.Team1Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
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

            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            ViewBag.RoundId = new SelectList((from p in db.Rounds where p.RoundId == RoundID select p).ToList(), "RoundId", "Scenario");

            var AvailableTeams = from p in db.Teams where p.EventId == EventID select p;
            var Team1s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team1Id;
            var Team2s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team2Id;
            AvailableTeams = from p in AvailableTeams where !Team1s.Contains(p.TeamId) && !Team2s.Contains(p.TeamId) select p;

            ViewBag.Team1Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
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

             if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");

            int RoundID = 0;
            int.TryParse(Session["SelectedRoundId"].ToString(), out RoundID);
            int EventID = 0;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);

            ViewBag.RoundId = new SelectList((from p in db.Rounds where p.RoundId == RoundID select p).ToList(), "RoundId", "Scenario");

            var AvailableTeams = from p in db.Teams where p.EventId == EventID select p;
            var Team1s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team1Id;
            var Team2s = from p in db.RoundTeamMatchups where p.RoundId == RoundID select p.Team2Id;
            AvailableTeams = from p in AvailableTeams where !Team1s.Contains(p.TeamId) && !Team2s.Contains(p.TeamId) select p;

            ViewBag.Team1Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
            ViewBag.Team2Id = new SelectList(AvailableTeams.ToList(), "TeamId", "Name");
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
