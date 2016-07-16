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
    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Teams
        [Authorize(Roles = "canEdit")]
        public ActionResult Index(int? id)
        {

            if (Session["SelectedEventId"] == null) return Redirect("Events");
            var teams = db.Teams.Include(t => t.Event);

            if (id != null) 
            {
                Session["SelectedTeamId"] = id;

                Session["SelectedRoundId"] = null;
                Session["SelectedRound"] = null;
                Session["SelectedRoundTeamMatchupId"] = null;
                Session["SelectedRoundTeamMatchup"] = null;
            }
            if (Session["SelectedEventId"] != null)
            {
                int EventId;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventId);
                teams = (from p in teams where p.EventId == EventId select p);
            }
            return View(teams.ToList());
        }

        // GET: Teams/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            var events = (from p in db.Events select p);
            if (Session["SelectedEventId"] != null)
            {
                int EventId;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventId);
                events = (from p in events where p.EventId == EventId select p);
            }
            ViewBag.EventId = new SelectList(events.ToList(), "EventId", "Title", Session["SelectedEventId"]);
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamId,Name,ImgURL,EventId,PairedDownRound,ByeRound")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var events = (from p in db.Events select p);
            if (Session["SelectedEventId"] != null)
            {
                int EventId;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventId);
                events = (from p in events where p.EventId == EventId select p);
            }
            ViewBag.EventId = new SelectList(events.ToList(), "EventId", "Title", team.EventId);
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            var events = (from p in db.Events select p);
            if (Session["SelectedEventId"] != null)
            {
                int EventId;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventId);
                events = (from p in events where p.EventId == EventId select p);
            }
            ViewBag.EventId = new SelectList(events.ToList(), "EventId", "Title", team.EventId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamId,Name,ImgURL,EventId,PairedDownRound,ByeRound,DropRound")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var events = (from p in db.Events select p);
            if (Session["SelectedEventId"] != null)
            {
                int EventId;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventId);
                events = (from p in events where p.EventId == EventId select p);
            }
            ViewBag.EventId = new SelectList(events.ToList(), "EventId", "Title", team.EventId);
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            var q = from p in db.Players where p.TeamId == team.TeamId select 1;
            if (q.Count() > 0)
                ViewBag.Message = "Cannot delete a team with players.";
            q = from p in db.RoundTeamMatchups where p.Team1Id == team.TeamId || p.Team2Id==team.TeamId select 1;
            if (q.Count() > 0)
                ViewBag.Message = "Cannot delete a team that has pairings.";
            if (ViewBag.Message != string.Empty)
            {
                return View(team);
            }
            else
            {
                var deleteme = 1;
                deleteme += 1;
                db.Teams.Remove(team);
                db.SaveChanges();
            }
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
