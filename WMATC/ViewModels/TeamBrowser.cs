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
    public class TeamBrowser
    {
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string EventImageURL { get; set; }
        public List<Team> Teams { get; set; }

        public class Team
        {
            public int TeamID { get; set; }
            public string TeamName { get; set; }
            public string TeamImageURL { get; set; }
            public List<Player> Players { get; set; }
        }
    }

   
}