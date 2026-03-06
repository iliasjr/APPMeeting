using System.Collections.Generic;
using API.Models;

namespace API.Services
{
    public interface IMatchmakingService
    {
        List<MatchRecommendation> GetBestMatches(MatchmakingRequest request);
    }
}
