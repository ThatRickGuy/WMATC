using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WMATC.Migrations;
using WMATC.Models;
using WMATC.ViewModels;

namespace WMATC.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Events/1
        //[Authorize(Roles = "canEdit")]
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                Session["SelectedEventId"] = id;
                Session["SelectedEvent"] = (from p in db.Events where p.EventId == id select p.Title).FirstOrDefault() ;

                //Clear the SelectedTeam if we've selected a new Event
                if (Session["SelectedTeam"] != null)
                {
                    int TeamId;
                    int.TryParse(Session["SelectedTeamId"].ToString(), out TeamId);
                    if ((from p in db.Teams where p.EventId == id && p.TeamId == TeamId select p).FirstOrDefault() == null)
                    {
                        Session["SelectedTeam"] = null;
                        Session["SelectedTeamId"] = null;
                    }
                }
                Session["SelectedRound"] = null;
                Session["SelectedRoundId"] = null;
                Session["SelectedRoundTeamMatchupId"] = null;
                Session["SelectedRoundTeamMatchupId"] = null;

            }

            var MyGUID = new Guid(User.Identity.GetUserId());
            List<Event> MyEvents;
            if (User.IsInRole("canEdit"))
                MyEvents = db.Events.ToList();
            else
                MyEvents = (from p in db.Events where p.Owner == MyGUID select p).ToList();

            return View(MyEvents);
        }

        // GET: Events/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            Event NewEvent = new Event();

            return View(NewEvent);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,Title,EventDate,ImageURL,ListLockDate")] Event @event)
        {
            if (ModelState.IsValid)
            {
                var MyGUID = new Guid(User.Identity.GetUserId());
                @event.Owner = MyGUID;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,Title,EventDate,ImageURL,ListLockDate,JSONDump")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                if (db.Entry(@event).Entity.JSONDump != "")
                {
                    //parse json
                    string JSON = db.Entry(@event).Entity.JSONDump;
                    JSONDump AllLists = new JavaScriptSerializer().Deserialize<JSONDump>(JSON);
                    
                    foreach (var List in AllLists.data)
                    {
                        var q = (from p in db.Players where p.Name == List.Name orderby p.TeamId descending select p).FirstOrDefault() ;
                        if (q == null)
                        {
                            var x = 1;
                        }
                        else
                        {
                            var f = List.Faction;
                            if (f == "Legion of Everblight") f = "Legion";
                            if (f == "Retribution of Scyrah") f = "Retribution";
                            if (f == "Circle Orboros") f = "Circle";
                            if (f == "Convergence of Cyriss") f = "Convergence";
                            if (f == "Protectorate of Menoth") f = "Protectorate";

                            q.Faction = (from p in db.Faction where p.Title == f select p ).FirstOrDefault ();
                            if (q.Faction == null)
                            {
                                var x2 = 1;
                            }
                            q.List1 = string.Empty;
                            if (List.List1 != null) foreach (string s in List.List1.List)
                                q.List1 += s + @"<br/>";
                            q.List2 = string.Empty;
                            if (List.List2 != null) foreach (string s in List.List2.List)
                                q.List2 += s + @"<br/>";
                            q.Caster1 = List.List1.List.FirstOrDefault();
                            q.Caster2 = List.List2.List.FirstOrDefault();
                            db.SaveChanges();
                        }

                    }
                }
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
