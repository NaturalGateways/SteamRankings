using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.UploadLib
{
    public class UploadParameters
    {
        public string Dirpath { get; set; }

        public string ExtractFilename { get; set; }

        public string OverridesFilepath { get; set; }

        public Natural.Aws.AwsAccessCredentials AwsCredentials { get; set; }
    }
}
