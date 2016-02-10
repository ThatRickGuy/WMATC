using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class Player
    {
        [Required]
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int? FactionId { get; set; }
        [ForeignKey("FactionId")]
        public virtual Faction Faction { get; set; }
        public string Caster1 { get; set; }
        public string List1 { get; set; }
        public string Caster2 { get; set; }
        public string List2 { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}