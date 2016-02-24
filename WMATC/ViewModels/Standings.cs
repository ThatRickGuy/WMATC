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
    public class Standings
    {
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string EventImageURL { get; set; }
        public int CurrentRound { get; set; }
        public List<Team> Teams;
        
        public class Team
        {
            public string TeamName { get; set; }
            public int TeamId { get; set; }
            public string TeamImageURL { get; set; }
            public int TeamWins { get; set; }
            public int StrengthOfSchedule { get; set; }
            public int TP { get; set; }
            public int CP { get; set; }
            public int APD { get; set; }

            public List<Player> Players { get; set; }
        }

        public class Player
        {
            public string Name { get; set; }
            public int PlayerId { get; set; }
            public string FactionImageURL { get; set; }
            public string Faction { get; set; }
            public int StrengthOfSchedule { get; set; }
            public int TP { get; set; }
            public int CP { get; set; }
            public int APD { get; set; }
        }
    }
}