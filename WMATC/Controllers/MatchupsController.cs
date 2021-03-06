﻿using Microsoft.AspNet.Identity;
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
        //[Authorize(Roles = "canEdit")]
        public ActionResult Index(int? id)
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");
            if (Session["SelectedRoundTeamMatchupId"] == null && id == null) return Redirect("RoundTeamMatchups");

            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");


            if (id != null)
            {
                Session["SelectedRoundTeamMatchupId"] = id;
                Session["SelectedRoundTeamMatchup"] = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == id select p.Team1.Name + " vs. " + p.Team2.Name).FirstOrDefault();
            }

            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);

            var matchups = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p).Include(m => m.Player1).Include(m => m.Player2).Include(m => m.RoundTeamMatchup).Include(m => m.Winner);
            return View(matchups.ToList());
        }

        // GET: Matchups/Details/5
        //[Authorize(Roles = "canEdit")]
        public ActionResult Details(int? id)
        {
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");
            }
            Matchup matchup = db.Matchups.Find(id);
            if (matchup == null)
            {
                return HttpNotFound();
            }
            return View(matchup);
        }

        // GET: Matchups/Create
        //[Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            if (Session["SelectedEventId"] == null) return Redirect("Events");
            if (Session["SelectedRoundId"] == null) return Redirect("Rounds");
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

            int SelectedRoundTeamMatchupId = -1;
            int EventID = -1;
            int.TryParse(Session["SelectedEventId"].ToString(), out EventID);
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);
            var Team1Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team1.TeamId).FirstOrDefault();
            var Team2Id = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Team2.TeamId).FirstOrDefault();
            var RoundID = (from p in db.RoundTeamMatchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.RoundId).FirstOrDefault();

            var bye = (from p in db.Players where p.Name == "Bye" && p.Team.EventId == EventID select p).FirstOrDefault();
            var AvailablePlayers = (from p in db.Players where p.TeamId == Team1Id select p).ToList();
            var UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player1Id).ToList();
            AvailablePlayers = (from p in AvailablePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p).ToList();
            AvailablePlayers.Add(bye);
            ViewBag.Player1Id = new SelectList(AvailablePlayers, "PlayerId", "Name");

            List<PlayerList> Lists = new List<PlayerList>();
            //Lists.Add(new PlayerList() { Key = null, PlayerName = "", Value = "" });
            foreach (var Player in AvailablePlayers)
            {
                Lists.Add(new PlayerList() { Key = 1, PlayerName = HttpUtility.HtmlEncode(Player.Name), Value = "1. " + HttpUtility.HtmlEncode(Player.Caster1) });
                Lists.Add(new PlayerList() { Key = 2, PlayerName = HttpUtility.HtmlEncode(Player.Name), Value = "2. " + HttpUtility.HtmlEncode(Player.Caster2) });
            }
            ViewBag.Player1List = new SelectList(Lists, "Key", "Value", "PlayerName");




            AvailablePlayers = (from p in db.Players where p.TeamId == Team2Id select p).ToList();
            UnavailablePlayers = (from p in db.Matchups where p.RoundTeamMatchupId == SelectedRoundTeamMatchupId select p.Player2Id).ToList();
            AvailablePlayers = (from p in AvailablePlayers where !UnavailablePlayers.Contains(p.PlayerId) select p).ToList();
            AvailablePlayers.Add(bye);
            ViewBag.Player2Id = new SelectList(AvailablePlayers.ToList(), "PlayerId", "Name");

            ViewBag.WinnerId = new SelectList(new List<Player>(), "PlayerId", "Name");

            
            foreach (var Player in AvailablePlayers)
            {
                Lists.Add(new PlayerList() { Key = 1, PlayerName = HttpUtility.HtmlEncode(Player.Name), Value = "1. " + HttpUtility.HtmlEncode(Player.Caster1) });
                Lists.Add(new PlayerList() { Key = 2, PlayerName = HttpUtility.HtmlEncode(Player.Name), Value = "2. " + HttpUtility.HtmlEncode(Player.Caster2) });
            }
            ViewBag.Player2List = new SelectList(Lists, "Key", "Value", "PlayerName");


            Matchup matchup = new Matchup();
            matchup.MatchupViewModel = new Matchup.ViewModel(matchup );
            matchup.MatchupViewModel.Lists = (from p in Lists select new Matchup.List { PlayerName = HttpUtility.HtmlEncode(p.PlayerName), ListNumber = p.Key, ListName = HttpUtility.HtmlEncode(p.Value) }).ToList ();
            return View(matchup);
        }

        public class PlayerList
        {
            public string PlayerName { get; set; }
            public int? Key { get; set; }
            public string Value { get; set; }
        }

        // POST: Matchups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MatchupId,RoundTeamMatchupId,Player1Id,Player2Id,WinnerId,Player1List,Player2List,Player1CP,Player2CP,Player1APD,Player2APD,VictoryCondition,ByeRound,PairDownRound")] Matchup matchup, string Command)
        {
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

            int SelectedRoundTeamMatchupId = -1;
            int.TryParse(Session["SelectedRoundTeamMatchupId"].ToString(), out SelectedRoundTeamMatchupId);

            if (ModelState.IsValid)
            {
                matchup.RoundTeamMatchupId = SelectedRoundTeamMatchupId;
                if (matchup.WinnerId == 0) matchup.WinnerId = null;
                db.Matchups.Add(matchup);
                db.SaveChanges();
                if (Command == "CreateAnother")
                    return RedirectToAction("Create");
                else
                    return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Matchups/Edit/5
        //[Authorize(Roles = "canEdit")]
        public ActionResult Edit(int? id)
        {
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

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
        //[Authorize(Roles = "canEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchupId,RoundTeamMatchupId,Player1Id,Player2Id,WinnerId,Player1List,Player2List,Player1CP,Player2CP,Player1APD,Player2APD")] Matchup matchup)
        {
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

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

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Matchups/Delete/5
        //[Authorize(Roles = "canEdit")]
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

            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

            return View(matchup);
        }

        // POST: Matchups/Delete/5
        //[Authorize(Roles = "canEdit")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(Session["SelectedEventId"]);
            if (@event.Owner != new Guid(User.Identity.GetUserId()))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Users may only modify their own events");

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
