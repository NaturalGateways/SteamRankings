using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.LambdaDtos
{
    public class LambdaRequestDto
    {
        public string RequestType { get; set; }

        public string PayloadJson { get; set; }
    }
}
