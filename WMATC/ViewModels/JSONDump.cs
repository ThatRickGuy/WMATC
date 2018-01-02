using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMATC.ViewModels
{
    public class JSONDump
    {

        public string Name { get; set; }
        public List<PlayerData> Players { get; set; }

        public class PlayerData
        {
            public string Name { get; set; }
            public string Faction { get; set; }
            public string CCCode { get; set; }
            public ArmyList List1 { get; set; }
            public ArmyList List2 { get; set; }
        }

        public class ArmyList
        {
            public string Theme { get; set; }
            public List<string> List { get; set; }
        }

    }
}





