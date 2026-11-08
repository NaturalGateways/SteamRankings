using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.LambdaDtos
{
    public class LambdaResponseDto
    {
        public bool Success { get; private set; }

        public object Payload { get; private set; }

        public string Error { get; private set; }

        public static LambdaResponseDto CreateSuccess()
        {
            return new LambdaResponseDto { Success = true };
        }

        public static LambdaResponseDto CreateSuccess(object payload)
        {
            return new LambdaResponseDto { Success = true, Payload = payload };
        }

        public static LambdaResponseDto CreateError(string errorMessage)
        {
            return new LambdaResponseDto { Success = false, Error = errorMessage };
        }

        private LambdaResponseDto()
        {
            //
        }
    }
}
