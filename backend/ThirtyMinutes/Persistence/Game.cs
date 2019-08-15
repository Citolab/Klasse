using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ThirtyMinutes.Persistence
{
    public class Game
    {
        public int Id { get; set; }
        public int SolutionLength { get; set; }
        public int MaxTimeInMinutes { get; set; }
        public int PenaltySeconds { get; set; }
        public int MaxTimeInSeconds { get; set; }
        public string Solution { get; set; }

        public Game(int id, string solution, int maxTimeInMinutes, int penaltySeconds)
        {
            Id = id;
            Solution = solution;
            MaxTimeInMinutes = maxTimeInMinutes;
            MaxTimeInSeconds = maxTimeInMinutes * 60;
            PenaltySeconds = penaltySeconds;
            SolutionLength = Solution.Length;
        }

        public bool CheckResponse(string response)
        {
            return response.ToUpperInvariant() == Solution.ToUpperInvariant();
        }

        public int GetRemainingSeconds(DateTime startTime, DateTime now, int penaltySeconds)
        {
            var remainingTime = startTime.AddSeconds(MaxTimeInSeconds - penaltySeconds) - now;
            return (int) remainingTime.TotalSeconds;
        }
    }
}