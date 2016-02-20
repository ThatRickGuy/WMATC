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
        public RoundTeamMatchup()
        {
            RoundTeamMatchupViewModel = new ViewModel(this);
        }
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

        public int? TableZone { get; set; }

        [NotMapped]
        public ViewModel RoundTeamMatchupViewModel { get; set; }

        public class ViewModel
        {
            public ViewModel(RoundTeamMatchup Parent)
            {
                _Parent = Parent;
            }
            private RoundTeamMatchup _Parent;
            public string Round { get { return _Parent.Round.Sequence + ". " + _Parent.Round.Scenario; } }
            public string Matchup { get { return _Parent.Team1.Name + " vs. " + _Parent.Team2.Name;  } }
        }
    }
}