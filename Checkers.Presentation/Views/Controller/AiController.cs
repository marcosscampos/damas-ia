using Checkers.Common;
using Checkers.Domain;
using Checkers.Presentation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers.Presentation.Views.Controller
{
    public static class AiController
    {
        private static readonly Logger Logger = Logger.GetSimpleLogger();
        private static readonly Random Rng = new Random();

        private static int counter = 0;
        private static bool thinking;

        public static bool IsThinking() => thinking;

        public static CheckersMove MinimaxStart(CheckerBoard board)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            thinking = true;

            List<CheckersMove> possibleMoves = board.GetMovesForPlayer();
            List<int> values = new List<int>();

            Logger.Info($"Max is {board.CurrentPlayerTurn}");

            if (possibleMoves.IsNullOrEmpty())
            {
                return null;
            }

            foreach (CheckersMove move in possibleMoves)
            {
                CheckersMove moveToMake = move;
                CheckerBoard boardToMakeMoveOn = board;
                do
                {
                    Logger.Debug("Board Before");
                    Logger.Debug(boardToMakeMoveOn.ToString());

                    boardToMakeMoveOn = (CheckerBoard)boardToMakeMoveOn.GetMinimaxClone();
                    boardToMakeMoveOn.MakeMoveOnBoard((CheckersMove)moveToMake.GetMinimaxClone());
                    moveToMake = moveToMake.NextMove;

                    Logger.Debug("Board After");
                    Logger.Debug(boardToMakeMoveOn.ToString());
                }
                while (moveToMake != null);

                values.Add(Minimax(boardToMakeMoveOn, ConstantsSettings.DefaultDepth - 1, alpha, beta, false, board.CurrentPlayerTurn));
            }

            int maxHeuristics = int.MinValue;
            foreach (int value in values)
            {
                if (value >= maxHeuristics)
                {
                    maxHeuristics = value;
                }
            }

            List<CheckersMove> bestMoves = new List<CheckersMove>();
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == maxHeuristics)
                {
                    bestMoves.Add(possibleMoves[i]);
                }
            }

            counter = 0;
            thinking = false;
            Logger.Info("Node Values: " + string.Join(",", values.Select(x => x.ToString()).ToArray()));
            return bestMoves[Rng.Next(bestMoves.Count)];
        }

        private static int Minimax(CheckerBoard board, int depth, int alpha, int beta, bool isMax, PlayerColor rootPlayer)
        {
            List<CheckersMove> possibleMoves = board.GetMovesForPlayer();

            if (depth == 0 || possibleMoves.Count == 0)
            {
                return Score(board, rootPlayer);
            }

            int value = 0;
            if (isMax)
            {
                value = int.MinValue;
                foreach (CheckersMove move in possibleMoves)
                {
                    CheckersMove moveToMake = move;
                    CheckerBoard boardToMakeMoveOn = board;
                    do
                    {
                        boardToMakeMoveOn = (CheckerBoard)boardToMakeMoveOn.GetMinimaxClone();
                        boardToMakeMoveOn.MakeMoveOnBoard((CheckersMove)moveToMake.GetMinimaxClone());
                        moveToMake = moveToMake.NextMove;
                    }
                    while (moveToMake != null);
                    int result = Minimax(boardToMakeMoveOn, depth - 1, alpha, beta, false, rootPlayer);

                    value = Math.Max(result, value);
                    alpha = Math.Max(alpha, value);

                    if (alpha >= beta)
                    {
                        Logger.Debug("Branch was pruned");
                        break;
                    }
                }
            }
            else
            {
                value = int.MaxValue;
                foreach (CheckersMove move in possibleMoves)
                {
                    CheckersMove moveToMake = move;
                    CheckerBoard boardToMakeMoveOn = board;
                    do
                    {
                        boardToMakeMoveOn = (CheckerBoard)boardToMakeMoveOn.GetMinimaxClone();
                        boardToMakeMoveOn.MakeMoveOnBoard((CheckersMove)moveToMake.GetMinimaxClone());
                        moveToMake = moveToMake.NextMove;
                    }
                    while (moveToMake != null);

                    int result = Minimax(boardToMakeMoveOn, depth - 1, alpha, beta, true, rootPlayer);

                    value = Math.Min(result, value);
                    beta = Math.Min(alpha, value);

                    if (alpha >= beta)
                    {
                        Logger.Debug("Branch was pruned");
                        break;
                    }
                }
            }

            return value;
        }

        private static int Score(CheckerBoard board, PlayerColor rootPlayer)
        {
            int score = board.ScoreWin(rootPlayer);

            if (score == 0)
            {
                switch (ConstantsSettings.Difficulty.ToLower())
                {
                    case "hard":
                        score += board.ScoreC(rootPlayer);
                        goto case "medium";
                    case "medium":
                        score += board.ScoreB(rootPlayer);
                        goto case "easy";
                    case "easy":
                        score += board.ScoreA(rootPlayer);
                        score += board.ScoreWin(rootPlayer);
                        break;
                    default:
                        goto case "hard";
                }
            }

            if (counter++ % 10000 == 0)
            {
                Logger.Info(string.Format("Evaluating Node number: {0} - Score: {1}", counter, score));
            }

            Console.WriteLine(board.ToString());

            return score;
        }
    }
}
