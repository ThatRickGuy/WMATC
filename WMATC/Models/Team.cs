using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class Team
    {
        [Required]
        [Key]
        public int TeamId { get; set; }
        public string Name { get; set; }
        [Url]
        public string ImgURL { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }

        public int? PairedDownRound { get; set; }
        public int? ByeRound { get; set; }
        public int? DropRound { get; set; }

        //protected string Password { get; set; }

    }
}


