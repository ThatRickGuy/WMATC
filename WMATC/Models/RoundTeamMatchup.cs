using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class RoundTeamMatchup
    {
        [Required]
        [Key]
        public int RoundTeamMatchupId { get; set; }

        public int RoundId { get; set; }
        [ForeignKey("RoundId")]
        public Round Round { get; set; }

        public int Team1Id { get; set; }
        [ForeignKey("Team1Id")]
        public Team Team1 { get; set; }

        public int Team2Id { get; set; }
        [ForeignKey("Team2Id")]
        public Team Team2 { get; set; }
    }
}