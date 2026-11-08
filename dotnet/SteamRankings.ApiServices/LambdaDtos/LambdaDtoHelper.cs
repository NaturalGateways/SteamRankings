using System;
using System.Collections.Generic;
using System.Text;

namespace SteamRankings.LambdaDtos
{
    public static class LambdaDtoHelper
    {
        public static ApiDtos.ApiRequestDto ConvertRequest(LambdaRequestDto lambdaDto)
        {
            // Convert enum
            if (string.IsNullOrEmpty(lambdaDto.RequestType))
            {
                throw new ArgumentException("Request type must be valid.");
            }
            ApiDtos.ApiRequestType requestType = ApiDtos.ApiRequestType.PickLineupItems;
            if (Enum.TryParse<ApiDtos.ApiRequestType>(lambdaDto.RequestType, out requestType) == false)
            {
                throw new ArgumentException("Unrecognised request type: " + lambdaDto.RequestType);
            }

            // Return
            return new ApiDtos.ApiRequestDto
            {
                RequestType = requestType,
                PayloadJson = lambdaDto.PayloadJson
            };
        }
    }
}
