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
    public class PlayersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Players
        [Authorize(Roles = "canEdit")]
        public ActionResult Index(int? id)
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedTeamId"] == null && id == null) return Redirect("Teams");

            if (id != null)
            {
                Session["SelectedTeamId"] = id;
                Session["SelectedTeam"] = (from p in db.Teams where p.TeamId == id select p.Name).FirstOrDefault();
            }

            var players = db.Players.Include(p => p.Team);
            if (Session["SelectedEvent"] != null)
            {
                int EventID;
                int.TryParse(Session["SelectedEventID"].ToString(), out EventID);
                players = from p in players where p.Team.EventId == EventID select p;

                if (id == null)
                {
                    int tempTeamID;
                    int.TryParse(Session["SelectedTeamID"].ToString(), out tempTeamID);
                    id = tempTeamID;
                }
                players = from p in players where p.TeamId == id select p;

            }
            return View(players.ToList());
        }

        // GET: Players/Details/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            var teams = from p in db.Teams select p;
            if (Session["SelectedEvent"] != null)
            {
                int EventID;
                int.TryParse(Session["SelectedEventID"].ToString(), out EventID);
                teams = from p in teams where p.EventId == EventID select p;

                if (Session["SelectedTeam"] != null)
                {
                    int TeamID;
                    int.TryParse(Session["SelectedTeamID"].ToString(), out TeamID);
                    teams = from p in teams where p.TeamId == TeamID select p;
                }

            }

            ViewBag.TeamId = new SelectList(teams.ToList(), "TeamId", "Name");
            ViewBag.FactionId = new SelectList(db.Faction, "FactionId", "Title");
            
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Create([Bind(Include = "PlayerId,Name,FactionId,Caster1,List1,Theme1,Caster2,List2,Theme2,TeamId")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var teams = from p in db.Teams select p;
            if (Session["SelectedEvent"] != null)
            {
                int EventID;
                int.TryParse(Session["SelectedEventID"].ToString(), out EventID);
                teams = from p in teams where p.EventId == EventID select p;

                if (Session["SelectedTeam"] != null)
                {
                    int TeamID;
                    int.TryParse(Session["SelectedTeamID"].ToString(), out TeamID);
                    teams = from p in teams where p.TeamId == TeamID select p;
                }

            }

            ViewBag.FactionId = new SelectList(db.Faction, "FactionId", "Title", player.FactionId);
            return View(player);
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            var teams = from p in db.Teams select p;
            if (Session["SelectedEvent"] != null)
            {
                int EventID;
                int.TryParse(Session["SelectedEventID"].ToString(), out EventID);
                teams = from p in teams where p.EventId == EventID select p;

                if (Session["SelectedTeam"] != null)
                {
                    int TeamID;
                    int.TryParse(Session["SelectedTeamID"].ToString(), out TeamID);
                    teams = from p in teams where p.TeamId == TeamID select p;
                }
            }

            ViewBag.TeamId = new SelectList(teams.ToList(), "TeamId", "Name", player.TeamId);
            ViewBag.FactionId = new SelectList(db.Faction, "FactionId", "Title", player.FactionId);
            
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Edit([Bind(Include = "PlayerId,Name,FactionId,Caster1,List1,Theme1,Caster2,List2,Theme2,TeamId")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var teams = from p in db.Teams select p;
            if (Session["SelectedEvent"] != null)
            {
                int EventID;
                int.TryParse(Session["SelectedEventID"].ToString(), out EventID);
                teams = from p in teams where p.EventId == EventID select p;

                if (Session["SelectedTeam"] != null)
                {
                    int TeamID;
                    int.TryParse(Session["SelectedTeamID"].ToString(), out TeamID);
                    teams = from p in teams where p.TeamId == TeamID select p;
                }
            }

            ViewBag.TeamId = new SelectList(teams.ToList(), "TeamId", "Name", player.TeamId);
            ViewBag.FactionId = new SelectList(db.Faction, "FactionId", "Title", player.FactionId);
            
            return View(player);
        }

        // GET: Players/Delete/5
        [Authorize(Roles = "canEdit")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
