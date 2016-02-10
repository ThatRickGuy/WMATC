using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WMATC.Models
{
    public class Faction
    {
        [Required]
        [Key]
        public int FactionId { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
    }
}
