using System;
using System.Collections.Generic;
using System.Linq;
using API.Models;

namespace API.Services
{
    public class MatchmakingService : IMatchmakingService
    {
        public List<MatchRecommendation> GetBestMatches(MatchmakingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.TargetProfile == null)
            {
                throw new ArgumentException("TargetProfile is required.", nameof(request));
            }

            var target = request.TargetProfile;
            var top = request.Top <= 0 ? 5 : request.Top;

            var results = request.CandidateProfiles
                .Where(c => c != null)
                .Where(c => !string.IsNullOrWhiteSpace(c.Username))
                .Where(c => !string.Equals(c.Username, target.Username, StringComparison.OrdinalIgnoreCase))
                .GroupBy(c => c.Username.Trim().ToLowerInvariant())
                .Select(g => g.First())
                .Select(candidate => ScoreProfile(target, candidate))
                .OrderByDescending(match => match.Score)
                .Take(top)
                .ToList();

            return results;
        }

        private static MatchRecommendation ScoreProfile(UserProfileInput target, UserProfileInput candidate)
        {
            var sharedInterests = SharedValues(target.Interests, candidate.Interests);
            var sharedSkills = SharedValues(target.Skills, candidate.Skills);

            var interestScore = Math.Min(40, sharedInterests.Count * 10);
            var skillScore = Math.Min(35, sharedSkills.Count * 7);
            var meetingTypeScore = SameMeetingType(target, candidate) ? 15 : 0;
            var bioSimilarityScore = ComputeBioKeywordScore(target.Bio, candidate.Bio);

            var total = Math.Round(Math.Min(100, interestScore + skillScore + meetingTypeScore + bioSimilarityScore), 2);

            return new MatchRecommendation
            {
                Username = candidate.Username,
                Score = total,
                Explanation = BuildExplanation(sharedInterests, sharedSkills, meetingTypeScore, bioSimilarityScore)
            };
        }

        private static HashSet<string> SharedValues(IEnumerable<string> left, IEnumerable<string> right)
        {
            var leftSet = NormalizeSet(left);
            leftSet.IntersectWith(NormalizeSet(right));
            return leftSet;
        }

        private static HashSet<string> NormalizeSet(IEnumerable<string> values)
        {
            return (values ?? Enumerable.Empty<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToLowerInvariant())
                .ToHashSet();
        }

        private static bool SameMeetingType(UserProfileInput target, UserProfileInput candidate)
        {
            return !string.IsNullOrWhiteSpace(target.PreferredMeetingType)
                   && string.Equals(target.PreferredMeetingType?.Trim(), candidate.PreferredMeetingType?.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static int ComputeBioKeywordScore(string leftBio, string rightBio)
        {
            var leftWords = Tokenize(leftBio);
            leftWords.IntersectWith(Tokenize(rightBio));
            return Math.Min(10, leftWords.Count * 2);
        }

        private static HashSet<string> Tokenize(string text)
        {
            return (text ?? string.Empty)
                .Split(new[] { ' ', ',', '.', ';', ':', '\n', '\r', '\t', '!', '?', '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLowerInvariant())
                .Where(x => x.Length > 2)
                .ToHashSet();
        }

        private static string BuildExplanation(ICollection<string> sharedInterests, ICollection<string> sharedSkills, int meetingTypeScore, int bioSimilarityScore)
        {
            var interestsText = sharedInterests.Count == 0 ? "none" : string.Join(", ", sharedInterests.Take(3));
            var skillsText = sharedSkills.Count == 0 ? "none" : string.Join(", ", sharedSkills.Take(3));

            return $"Shared interests ({sharedInterests.Count}): {interestsText}. Shared skills ({sharedSkills.Count}): {skillsText}. Meeting preference match: {(meetingTypeScore > 0 ? "yes" : "no")}. Bio similarity points: {bioSimilarityScore}.";
        }
    }
}
