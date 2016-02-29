using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WMATC.ViewModels
{
    public class GameReport
    {
        [Key ]
        public int MatchupId { get; set; }
        public List<SimplePlayer> Players { get; set; }
        public int? WinnerId { get; set; }

        public SimplePlayer Player1 { get; set; }
        public List<SimpleList> Player1Lists { get; set; }
        public int? Player1ListId { get; set; }
        public int Player1CP { get; set; }
        public int Player1APD { get; set; }
        
        public SimplePlayer Player2 { get; set; }
        public List<SimpleList> Player2Lists { get; set; }
        public int? Player2ListId { get; set; }
        public int Player2CP { get; set; }
        public int Player2APD { get; set; }

        public class SimplePlayer
        {
            [Key]
            public int PlayerId { get; set; }
            public string PlayerName { get; set; }
        }

        public class SimpleList
        {
            [Key]
            public int? ListId { get; set; }
            public string Warnoun { get; set; }
        }
    }


}