using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WMATC.Models
{
    public class Matchup
    {
        [Required]
        [Key]
        public int MatchupId { get; set; }
        public int RoundTeamMatchupId { get; set; }
        [ForeignKey("RoundTeamMatchupId")]
        public RoundTeamMatchup RoundTeamMatchup { get; set; }

        public int Player1Id { get; set; }
        [ForeignKey("Player1Id")]
        public Player Player1 { get; set;  }

        public int Player2Id { get; set; }
        [ForeignKey("Player2Id")]
        public Player Player2 { get; set; }

        public int WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public Player Winner { get; set; }
        
        public int Player1List { get; set; }
        public int Player2List { get; set; }
    }
}