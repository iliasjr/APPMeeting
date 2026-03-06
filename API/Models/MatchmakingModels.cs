using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserProfileInput
    {
        [Required]
        [MinLength(2)]
        public string Username { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        public List<string> Interests { get; set; } = new List<string>();

        public List<string> Skills { get; set; } = new List<string>();

        [MaxLength(50)]
        public string PreferredMeetingType { get; set; }
    }

    public class MatchmakingRequest
    {
        [Required]
        public UserProfileInput TargetProfile { get; set; }

        [Required]
        [MinLength(1)]
        public List<UserProfileInput> CandidateProfiles { get; set; } = new List<UserProfileInput>();

        [Range(1, 50)]
        public int Top { get; set; } = 5;
    }

    public class MatchRecommendation
    {
        public string Username { get; set; }

        public double Score { get; set; }

        public string Explanation { get; set; }
    }
}
