using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int PublishViewModelId { get; set; }
        public string EventImageURL { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public List<RoundViewModel> Rounds { get; set; }
        public List<PlayerViewModel> Players { get; set; }
    }

    public class PlayerViewModel
    {
        [Key]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public string JSVar1 { get { return "var l" + Player.PlayerId + "_1="; } }
        public string JSList1 { get { return Player.List1; } }
        public string JSVar2 { get { return "var l" + Player.PlayerId + "_2="; } }
        public string JSList2 { get { return Player.List2; } }
        
    }
    public class RoundViewModel
    {
        [Key]
        public int RoundID { get; set; }
        public int Sequence { get; set; }
        public string Scenario { get; set; }
        public List<TeamMatchupViewModel> TeamMatchups { get; set; }
    }

    public class TeamMatchupViewModel
    {
        [Key]
        public int RoundTeamMatchupID { get; set; }
        public string Team1Name { get; set; }
        public string Team1ImageURL { get; set; }
        public string Team2Name { get; set; }
        public string Team2ImageURL { get; set; }
        public string Team1IsWinner { get; set; }
        public string Team2IsWinner { get; set; }
        public int Team1Wins { get; set; }
        public int Team2Wins { get; set; }
        public int TableZone { get; set; }
        public List<MatchupViewModel> Matchups { get; set; }
    }

    public class MatchupViewModel
    {
        [Key]
        public int MatchupId { get; set; }
        public int Player1Id { get; set; }
        public string Player1Name { get; set; }
        public string Player1Faction { get; set; }
        public string Player1FactionURL { get; set; }
        public int Player1ListNumber { get; set; }
        public string Player1Caster { get; set; }
        public string Player1IsWinner { get; set; }

        public int Player2Id { get; set; }
        public string Player2Name { get; set; }
        public string Player2Faction { get; set; }
        public string Player2FactionURL { get; set; }
        public int Player2ListNumber { get; set; }
        public string Player2Caster { get; set; }
        public string Player2IsWinner { get; set; }
    }
}