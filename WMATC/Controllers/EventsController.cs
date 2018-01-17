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
                Session["SelectedEvent"] = (from p in db.Events where p.EventId == id select p.Title).FirstOrDefault();

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
                var dbEvent = (from p in db.Events where p.EventId == @event.EventId select p).FirstOrDefault();
                db.SaveChanges();


                var q = from p in db.Teams where p.EventId == @event.EventId select p;
                if (q.Count() > 0) db.Teams.RemoveRange(q);
                db.SaveChanges();
                if (@event.JSONDump.Trim() != "")
                {
                    //parse json
                    string JSON = db.Entry(@event).Entity.JSONDump;
                    List<JSONDump> AllTeams = new JavaScriptSerializer().Deserialize<List<JSONDump>>(JSON);

                    foreach (var Team in AllTeams)
                    {
                        var dbTeam = (from p in db.Teams where p.Name == Team.Name && p.EventId == @event.EventId select p).FirstOrDefault();
                        if (dbTeam == null)
                        {
                            dbTeam = new Team();
                            dbTeam.Name = Team.Name;
                            dbTeam.EventId = @event.EventId;
                            db.Teams.Add(dbTeam);
                            db.SaveChanges();
                        }

                        foreach (var Player in Team.Players)
                        {
                            var dbPlayer = (from p in db.Players where p.TeamId == dbTeam.TeamId && p.Name == Player.Name select p).FirstOrDefault();
                            if (dbPlayer == null)
                            {
                                dbPlayer = new Player();
                                db.Players.Add(dbPlayer);
                            }
                            dbPlayer.TeamId = dbTeam.TeamId;
                            dbPlayer.Team = dbTeam;
                            dbPlayer.Name = Player.Name;

                            //Faction translation
                            var f = Player.Faction;
                            if (f == "Legion of Everblight") f = "Legion";
                            if (f == "Retribution of Scyrah") f = "Retribution";
                            if (f == "Circle Orboros") f = "Circle";
                            if (f == "Convergence of Cyriss") f = "Convergence";
                            if (f == "Protectorate of Menoth") f = "Protectorate";
                            dbPlayer.Faction = (from p in db.Faction where p.Title == f select p).FirstOrDefault();


                            //Lists & Casters
                            dbPlayer.List1 = string.Empty;
                            if (Player.List1 != null)
                            {
                                dbPlayer.Caster1 = Player.List1.List.FirstOrDefault();
                                dbPlayer.Theme1 = Player.List1.Theme;
                                foreach (string s in Player.List1.List)
                                    dbPlayer.List1 += s + @"<br/>";
                            }
                            dbPlayer.List2 = string.Empty;
                            if (Player.List2 != null)
                            {
                                dbPlayer.Caster2 = Player.List2.List.FirstOrDefault();
                                dbPlayer.Theme2 = Player.List2.Theme;
                                foreach (string s in Player.List2.List)
                                    dbPlayer.List2 += s + @"<br/>";
                            }
                        }

                        db.SaveChanges();
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
