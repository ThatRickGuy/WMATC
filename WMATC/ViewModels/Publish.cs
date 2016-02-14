﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMATC.Models;

namespace WMATC.ViewModels
{
    public class PublishViewModel
    {
        private ApplicationDbContext db;

        public PublishViewModel(int EventID)
        {
            db = new ApplicationDbContext();

            var MyEvent = (from p in db.Events where p.EventId == EventID select p).First();
            this.EventDate = MyEvent.EventDate;
            this.EventTitle = MyEvent.Title;
            this.EventImageURL = MyEvent.ImageURL;

            this.Rounds = (from p in db.Rounds where p.EventId == MyEvent.EventId orderby p.Sequence select new RoundViewModel() { Sequence = p.Sequence, Scenario = p.Scenario, RoundID = p.RoundId }).ToList<RoundViewModel>();
            foreach (RoundViewModel Round in Rounds)
            {
                Round.TeamMatchups = (from p in db.RoundTeamMatchups
                                      where p.RoundId == Round.RoundID
                                      orderby p.Team1.Name
                                      select new TeamMatchupViewModel()
                                      {
                                          RoundTeamMatchupID = p.RoundTeamMatchupId,
                                          Team1Name = p.Team1.Name,
                                          Team1ImageURL = p.Team1.ImgURL,
                                          Team1IsWinner = "BlankBackground",
                                          Team2Name = p.Team2.Name,
                                          Team2ImageURL = p.Team2.ImgURL,
                                          Team2IsWinner = "BlankBackground"
                                      }).ToList<TeamMatchupViewModel>();

                foreach (TeamMatchupViewModel TeamMatchup in Round.TeamMatchups)
                {
                    TeamMatchup.Matchups = (from p in db.Matchups
                                            where p.RoundTeamMatchupId == TeamMatchup.RoundTeamMatchupID
                                            select new MatchupViewModel()
                                            {
                                                Player1FactionURL = p.Player1.Faction.ImageURL,
                                                Player1Faction = p.Player1.Faction.Title,
                                                Player1Caster = (p.Player1List == 1) ? p.Player1.Caster1 : p.Player1.Caster2,
                                                Player1ListNumber = p.Player1List,
                                                Player1Name = p.Player1.Name,
                                                Player1IsWinner = (p.WinnerId == p.Player1Id) ? "WinnerBackground" : (p.WinnerId == null) ? "BlankBackground" : "LoserBackground",
                                                Player2FactionURL = p.Player2.Faction.ImageURL,
                                                Player2Faction = p.Player2.Faction.Title,
                                                Player2Caster = (p.Player2List == 1) ? p.Player2.Caster1 : p.Player2.Caster2,
                                                Player2ListNumber = p.Player2List,
                                                Player2Name = p.Player2.Name,
                                                Player2IsWinner = (p.WinnerId == p.Player2Id) ? "WinnerBackground" : (p.WinnerId == null) ? "BlankBackground" : "LoserBackground"
                                            }).ToList<MatchupViewModel>();
                }
                foreach (TeamMatchupViewModel TeamMatchup in Round.TeamMatchups)
                {
                    TeamMatchup.Team1Wins = (from p in TeamMatchup.Matchups where p.Player1IsWinner == "WinnerBackground" select p).Count();
                    TeamMatchup.Team2Wins = (from p in TeamMatchup.Matchups where p.Player2IsWinner == "WinnerBackground" select p).Count();
                    if (TeamMatchup.Team1Wins > TeamMatchup.Matchups.Count / 2 && TeamMatchup.Team1Wins + TeamMatchup.Team2Wins == TeamMatchup.Matchups.Count())
                    {
                        TeamMatchup.Team1IsWinner = "WinnerBackground";
                        TeamMatchup.Team2IsWinner = "LoserBackground";
                    }
                    else if (TeamMatchup.Team2Wins > TeamMatchup.Matchups.Count / 2 && TeamMatchup.Team1Wins + TeamMatchup.Team2Wins == TeamMatchup.Matchups.Count())
                    {
                        TeamMatchup.Team1IsWinner = "LoserBackground";
                        TeamMatchup.Team2IsWinner = "WinnerBackground";
                    }
                    else
                    {
                        TeamMatchup.Team1IsWinner = "BlankBackground";
                        TeamMatchup.Team2IsWinner = "BlankBackground";
                    }
                }
            }
        }

        public string EventImageURL;
        public string EventTitle;
        public DateTime EventDate;
        public List<RoundViewModel> Rounds;

        public class RoundViewModel
        {
            public int RoundID;
            public int Sequence;
            public string Scenario;
            public List<TeamMatchupViewModel> TeamMatchups = new List<TeamMatchupViewModel>();
        }

        public class TeamMatchupViewModel
        {
            public int RoundTeamMatchupID;
            public string Team1Name;
            public string Team1ImageURL;
            public string Team2Name;
            public string Team2ImageURL;
            public string Team1IsWinner;
            public string Team2IsWinner;
            public int Team1Wins;
            public int Team2Wins;
            public List<MatchupViewModel> Matchups = new List<MatchupViewModel>();
        }

        public class MatchupViewModel
        {
            public string Player1Name;
            public string Player1Faction;
            public string Player1FactionURL;
            public int Player1ListNumber;
            public string Player1Caster;
            public string Player1IsWinner;

            public string Player2Name;
            public string Player2Faction;
            public string Player2FactionURL;
            public int Player2ListNumber;
            public string Player2Caster;
            public string Player2IsWinner;
        }

    }
}