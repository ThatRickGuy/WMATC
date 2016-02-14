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
        public ActionResult Index(int? id)
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");
            if (Session["SelectedRoundTeamMatchupId"] == null && id == null) return Redirect("RoundTeamMatchups");

            if (id != null)
            {
                Session["SelectedRoundTeamMatchupId"] = id;
                Session["SelectedRoundTeamMatchup"] = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId  == id select p.Team1.Name + " vs. " + p.Team2.Name).FirstOrDefault();
            }

            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);

            var matchups = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p).Include(m => m.Player1).Include(m => m.Player2).Include(m => m.RoundTeamMatchup).Include(m => m.Winner);
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
            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);
            var Team1Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team1.TeamId).FirstOrDefault();
            var Team2Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team2.TeamId).FirstOrDefault();
            var RoundID = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.RoundId).FirstOrDefault();

            var AvailavlePlayers = from p in db.Players where p.TeamId == Team1Id select p;
            var UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player1Id).ToList();
            AvailavlePlayers = from p in AvailavlePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p;
            ViewBag.Player1Id = new SelectList(AvailavlePlayers.ToList(), "PlayerId", "Name");

            AvailavlePlayers = from p in db.Players where p.TeamId == Team2Id select p;
            UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player2Id).ToList();
            AvailavlePlayers = from p in AvailavlePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p;
            ViewBag.Player2Id = new SelectList(AvailavlePlayers.ToList(), "PlayerId", "Name");

            ViewBag.WinnerId = new SelectList(new List<Player>(), "PlayerId", "Name");

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
            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);

            if (ModelState.IsValid)
            {
                matchup.RoundTeamMatchupId = SelectedRoundTeamMatchupId;
                if (matchup.WinnerId == 0) matchup.WinnerId = null;
                db.Matchups.Add(matchup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var Team1Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team1.TeamId).FirstOrDefault();
            var Team2Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team2.TeamId).FirstOrDefault();
            var RoundID = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.RoundId).FirstOrDefault();

            var AvailavlePlayers = from p in db.Players where p.TeamId == Team1Id select p;
            var UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player1Id).ToList();
            AvailavlePlayers = from p in AvailavlePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p;
            ViewBag.Player1Id = new SelectList(AvailavlePlayers, "PlayerId", "Name");

            AvailavlePlayers = from p in db.Players where p.TeamId == Team2Id select p;
            UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player2Id).ToList();
            AvailavlePlayers = from p in AvailavlePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p;
            ViewBag.Player2Id = new SelectList(AvailavlePlayers, "PlayerId", "Name");

            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);

            List<Models.Player> players = new List<Player>();

            if (matchup.Player1Id > 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id > 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.TeamId == matchup.RoundTeamMatchup.Team2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.TeamId == matchup.RoundTeamMatchup.Team1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.TeamId == Team1Id || p.TeamId == Team2Id select p).ToList();
            }
            players.Insert(0, new Player());
            ViewBag.WinnerId = new SelectList(players, "PlayerId", "Name", matchup.WinnerId);


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
            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);
            var Team1Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team1.TeamId).FirstOrDefault();
            var Team2Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team2.TeamId).FirstOrDefault();

            ViewBag.Player1Id = new SelectList(from p in db.Players where p.TeamId == Team1Id select p, "PlayerId", "Name", matchup.Player1Id);
            ViewBag.Player2Id = new SelectList(from p in db.Players where p.TeamId == Team2Id select p, "PlayerId", "Name", matchup.Player2Id);
            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);

            List<Models.Player> players = new List<Player>();
            if (matchup.Player1Id > 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id > 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.TeamId == matchup.RoundTeamMatchup.Team2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.TeamId == matchup.RoundTeamMatchup.Team1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.TeamId == Team1Id || p.TeamId == Team2Id select p).ToList();
            }
            players.Insert(0, new Player());
            ViewBag.WinnerId = new SelectList(players, "PlayerId", "Name", matchup.WinnerId);

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
            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);
            if (ModelState.IsValid)
            {
                matchup.RoundTeamMatchupId = SelectedRoundTeamMatchupId;
                if (matchup.WinnerId == 0) matchup.WinnerId = null;
                db.Entry(matchup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var Team1Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team1.TeamId).FirstOrDefault();
            var Team2Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team2.TeamId).FirstOrDefault();

            ViewBag.Player1Id = new SelectList(from p in db.Players where p.TeamId == Team1Id select p, "PlayerId", "Name", matchup.Player1Id);
            ViewBag.Player2Id = new SelectList(from p in db.Players where p.TeamId == Team2Id select p, "PlayerId", "Name", matchup.Player2Id);
            ViewBag.RoundTeamMatchupId = new SelectList(db.RoundTeamMatchups, "RoundTeamMatchupId", "RoundTeamMatchupId", matchup.RoundTeamMatchupId);

            List<Models.Player> players = new List<Player>();
            if (matchup.Player1Id > 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id > 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.PlayerId == matchup.Player1Id || p.TeamId == matchup.RoundTeamMatchup.Team2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id > 0)
            {
                players = (from p in db.Players where p.TeamId == matchup.RoundTeamMatchup.Team1Id || p.PlayerId == matchup.Player2Id select p).ToList();
            }
            if (matchup.Player1Id == 0 && matchup.Player2Id == 0)
            {
                players = (from p in db.Players where p.TeamId == Team1Id || p.TeamId == Team2Id select p).ToList();
            }
            players.Insert(0, new Player());
            ViewBag.WinnerId = new SelectList(players, "PlayerId", "Name", matchup.WinnerId);

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
