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
    public class RoundsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rounds
        [Authorize(Roles = "canEdit")]
        public ActionResult Index(int? id)
        {
            ActionResult rView = null;

            if (id != null)
            {
                Session["SelectedRoundId"] = id;
                Session["SelectedRound"] = (from p in db.Rounds where p.RoundId == id select p.Sequence + ". " + p.Scenario).FirstOrDefault();

                Session["SelectedTeam"] = null;
                Session["SelectedTeamId"] = null;
                Session["SelectedRoundTeamMatchupId"] = null;
                Session["SelectedRoundTeamMatchup"] = null;
            }
            if (Session["SelectedEventId"] == null)
            {
                rView = Redirect("Events");
            }
            else
            {
                int EventID;
                int.TryParse(Session["SelectedEventId"].ToString(), out EventID);
                var rounds = (from p in db.Rounds where p.EventId== EventID select p ).Include(r => r.Event);
                rView = View(rounds.ToList());
            }

            return rView;
        }

        // GET: Rounds/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // GET: Rounds/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Title");
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoundId,Sequence,Scenario,EventId")] Round round)
        {
            if (ModelState.IsValid)
            {
                db.Rounds.Add(round);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Events, "EventId", "Title", round.EventId);
            return View(round);
        }

        // GET: Rounds/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Title", round.EventId);
            return View(round);
        }

        // POST: Rounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoundId,Sequence,Scenario,EventId")] Round round)
        {
            if (ModelState.IsValid)
            {
                db.Entry(round).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Events, "EventId", "Title", round.EventId);
            return View(round);
        }

        // GET: Rounds/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Round round = db.Rounds.Find(id);
            if (round == null)
            {
                return HttpNotFound();
            }
            return View(round);
        }

        // POST: Rounds/Delete/5
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Round round = db.Rounds.Find(id);
            db.Rounds.Remove(round);
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
