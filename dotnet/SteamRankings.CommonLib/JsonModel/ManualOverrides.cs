using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.JsonModel
{
    public class ManualOverrides
    {
        public int[] BlacklistIds { get; set; }

        public ManualOverridesExclusion[] Exclusions { get; set; }

        public int[][] Dependencies { get; set; }
    }

    public class ManualOverridesExclusion
    {
        public string Reason { get; set; }

        public int[] Ids { get; set; }
    }
}
