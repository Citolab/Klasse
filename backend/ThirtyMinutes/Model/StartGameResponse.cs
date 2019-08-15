using System;
using ThirtyMinutes.Persistence;

namespace ThirtyMinutes.Model
{
    public class StartGameResponse
    {
        public DateTime StartTime;
        public int SolutionLength;
        public int MaxTimeInMinutes;
        public int PenaltySeconds;
        public int RemainingSeconds;
        public GameState State;
    }
}