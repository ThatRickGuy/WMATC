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
    public class RoundTeamMatchupPrintURL
    {
        [Key]
        public string PrintURL { get; set; }
    }
}