using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class Round
    {
        [Required]
        [Key]
        public int RoundId { get; set; }
        public int Sequence { get; set; }
        public string Scenario { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}