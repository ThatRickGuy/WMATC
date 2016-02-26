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
    public class RoundTeamMatchupPrint
    {

        [Key]
        public int RoundTeamMatchupId { get; set; }
        public List<PrintPairing> LeftList { get; set; }
        public List<PrintPairing> RightList { get; set; }

        public string EventName { get; set; }
        public string EventImageURL { get; set; }
        public DateTime EventDate { get; set; }

        public int RoundNumber { get; set; }
        public string RoundScenario { get; set; }
    }

    public class PrintPairing
    {
        [Key]
        public int Team1Id { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public int TableZone { get; set; }
    }
}