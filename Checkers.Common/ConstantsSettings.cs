using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Common
{
    public static class ConstantsSettings
    {
        public const int KingWorth = 32;
        public const int PawnWorth = 13;
        public const int PawnDangerValue = 1;
        public const int KingDangerValue = 16;
        public const bool IsAiGame = true;
        public const bool IsAiDuel = false;
        public const string WhosFirst = "red";
        public static TimeSpan TimeToSleepBetweenMoves = TimeSpan.FromSeconds(1000.0);
        public const bool RunningGeneticAlgo = false;
        public const int NumberOfSimulations = 30;
        public const int NumberOfRounds = 50;
        public const int MinimumLogLevel = 2;
    }
}
