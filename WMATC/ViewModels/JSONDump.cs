using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMATC.ViewModels
{
    public class JSONDump
    {
        public class ArmyList
        {
            public List<string> List { get; set; }
            public string Theme { get; set; }
        }

        public List<PlayerData> data { get; set; }

        public class PlayerData
        {
            public string Name { get; set; }
            public string Faction { get; set; }
            public string CCCode { get; set; }
            public ArmyList List1 { get; set; }
            public ArmyList List2 { get; set; }
        }
    }
}


