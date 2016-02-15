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

        public int? WinnerId { get; set; }
        [ForeignKey("WinnerId")]
        public virtual Player Winner { get; set; }
        
        public int? Player1List { get; set; }
        public int? Player2List { get; set; }

        [NotMapped]
        public ViewModel MatchupViewModel { get; set; }


        public class ViewModel
        {
            public ViewModel(Matchup Parent)
            {
                _Parent = Parent;
            }
            private Matchup _Parent;

            public string RoundTeamMatchup { get { return _Parent.RoundTeamMatchup.RoundTeamMatchupViewModel.Round + _Parent.RoundTeamMatchup.RoundTeamMatchupViewModel.Matchup; } }
            public List<List> Lists { get; set; }
            //public string Round { get { return _Parent.Round.Sequence + ". " + _Parent.Round.Scenario; } }
            //public string Matchup { get { return _Parent.Team1.Name + " vs. " + _Parent.Team2.Name; } }
        }

        public class List
        {
            public string PlayerName { get; set; }
            public int? ListNumber { get; set; }
            public string ListName { get; set; }
        }
    }
}