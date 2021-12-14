using Checkers.Common;
using Checkers.Domain;
using Checkers.Domain.Enum;
using Checkers.Presentation.Abstractions;
using Checkers.Presentation.Domain.Factory;
using Checkers.Presentation.Domain.Interface;
using Checkers.Presentation.Domain.Model.Genome;
using Checkers.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Checkers.Presentation.Domain.Model
{
    [Serializable]
    public class CheckerBoard : IMinimaxClonable
    {
        public List<List<CheckersSquareUserControl>> BoardArray { get; set; } = new List<List<CheckersSquareUserControl>>();
        public PlayerColor CurrentPlayerTurn { get; set; }
        public CheckerBoard() => CurrentPlayerTurn = DetermineWhosFirst();

        public void MakeBoard(RoutedEventHandler routedEventHandler)
        {
            int count = 0;
            for (int row = 0; row < 8; row++)
            {
                BoardArray.Add(new List<CheckersSquareUserControl>());
                for (int column = 0; column < 8; column++)
                {
                    CheckersSquareUserControl checkerSquareUC;
                    if (row % 2 == 0)
                    {
                        if (column % 2 == 0)
                        {
                            checkerSquareUC = new CheckersSquareUserControl(
                                Brushes.White,
                                new CheckersPoint(row, column, CheckerPieceType.nullPiece),
                                routedEventHandler);
                        }
                        else
                        {
                            if (row < 3)
                            {
                                checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.BlackPawn),
                                    routedEventHandler);
                            }
                            else if (row > 4)
                            {
                                checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.RedPawn),
                                    routedEventHandler);
                            }
                            else
                            {
                                checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.nullPiece),
                                    routedEventHandler);
                            }
                        }
                    }
                    else
                    {
                        if (column % 2 == 0)
                        {
                            if (row < 3)
                            {
                                checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.BlackPawn),
                                    routedEventHandler);
                            }
                            else if (row > 4)
                            {
                                checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.RedPawn),
                                    routedEventHandler);
                            }
                            else
                            {
                                    checkerSquareUC = new CheckersSquareUserControl(
                                    Brushes.Black,
                                    new CheckersPoint(row, column, CheckerPieceType.nullPiece),
                                    routedEventHandler);
                            }
                        }
                        else
                        {
                            checkerSquareUC = new CheckersSquareUserControl(
                                Brushes.White,
                                new CheckersPoint(row, column, CheckerPieceType.nullPiece),
                                routedEventHandler);
                        }
                    }

                    count++;
                    BoardArray[row].Add(checkerSquareUC);
                }
            }
        }

        public object GetWinner()
        {
            List<CheckersPoint> redCheckerPoints = GetPointsForColor<IRedPiece>();
            List<CheckersPoint> blackCheckerPoints = GetPointsForColor<IBlackPiece>();

            if (blackCheckerPoints.Count == 0)
            {
                return PlayerColor.Red;
            }
            else if (redCheckerPoints.Count == 0)
            {
                return PlayerColor.Black;
            }
            else
            {
                return null;
            }
        }

        public List<CheckersPoint> GetPointsForColor<ColorInterface>()
        {
            List<CheckersPoint> listOfColor = new List<CheckersPoint>();

            foreach (List<CheckersSquareUserControl> list in BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    if (squareUC.CheckersPoint.Checker != null && squareUC.CheckersPoint.Checker is ColorInterface)
                    {
                        listOfColor.Add(squareUC.CheckersPoint);
                    }
                }
            }

            return listOfColor;
        }

        public int ScoreWin(PlayerColor rootPlayer)
        {
            int score = 0;
            object winningColor = this.GetWinner();

            if (winningColor != null && winningColor is PlayerColor winnerColor)
            {
                if (rootPlayer == PlayerColor.Red)
                {
                    if (winnerColor == PlayerColor.Red)
                    {
                        score = int.MaxValue;
                    }
                    else
                    {
                        score = int.MinValue;
                    }
                }
                else
                {
                    if (winnerColor == PlayerColor.Black)
                    {
                        score = int.MaxValue;
                    }
                    else
                    {
                        score = int.MinValue;
                    }
                }
            }

            return score;
        }

        public int ScoreA(PlayerColor rootPlayer)
        {
            int score = 0;
            int kingWorth = ConstantsSettings.KingWorth;
            int pawnWorth = ConstantsSettings.PawnWorth;

            if (rootPlayer == PlayerColor.Red)
            {
                if (ConstantsSettings.RunningGeneticAlgo)
                {
                    kingWorth = RandomGenome.GetRandomGenomeInstance().KingWorthGene;
                    pawnWorth = RandomGenome.GetRandomGenomeInstance().PawnWorthGene;
                }

                foreach (CheckersPoint point in GetPointsForColor<IBlackPiece>())
                {
                    score -= point.Checker is KingCheckerPiece ? kingWorth : pawnWorth;
                }

                foreach (CheckersPoint point in GetPointsForColor<IRedPiece>())
                {
                    score += point.Checker is KingCheckerPiece ? kingWorth : pawnWorth;
                }
            }
            else
            {
                if (ConstantsSettings.RunningGeneticAlgo)
                {
                    kingWorth = WinningGenome.GetWinningGenomeInstance().KingWorthGene;
                    pawnWorth = WinningGenome.GetWinningGenomeInstance().PawnWorthGene;
                }

                foreach (CheckersPoint point in GetPointsForColor<IBlackPiece>())
                {
                    score += point.Checker is KingCheckerPiece ? kingWorth : pawnWorth;
                }

                foreach (CheckersPoint point in GetPointsForColor<IRedPiece>())
                {
                    score -= point.Checker is KingCheckerPiece ? kingWorth : pawnWorth;
                }
            }

            return score;
        }

        public int ScoreB(PlayerColor rootPlayer)
        {
            int score = 0;

            foreach (List<CheckersSquareUserControl> list in BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    if (squareUC.CheckersPoint.Checker != null && !(squareUC.CheckersPoint.Checker is KingCheckerPiece))
                    {
                        if (rootPlayer == PlayerColor.Black)
                        {
                            if (squareUC.CheckersPoint.Checker is IRedPiece)
                            {
                                score -= (squareUC.CheckersPoint.Row - 7) * -1;
                            }

                            if (squareUC.CheckersPoint.Checker is IBlackPiece)
                            {
                                score += squareUC.CheckersPoint.Row;
                            }
                        }
                        else
                        {
                            if (squareUC.CheckersPoint.Checker is IRedPiece)
                            {
                                score += (squareUC.CheckersPoint.Row - 7) * -1;
                            }

                            if (squareUC.CheckersPoint.Checker is IBlackPiece)
                            {
                                score -= squareUC.CheckersPoint.Row;
                            }
                        }
                    }
                }
            }

            return score;
        }

        public int ScoreC(PlayerColor rootPlayer)
        {
            int score = 0;
            int kingDangerValue = ConstantsSettings.KingDangerValue;
            int pawnDangerValue = ConstantsSettings.PawnDangerValue;

            if (ConstantsSettings.RunningGeneticAlgo)
            {
                if (rootPlayer == PlayerColor.Red)
                {
                    kingDangerValue = RandomGenome.GetRandomGenomeInstance().KingDangerValueGene;
                    pawnDangerValue = RandomGenome.GetRandomGenomeInstance().PawnDangerValueGene;
                }
                else
                {
                    kingDangerValue = WinningGenome.GetWinningGenomeInstance().KingDangerValueGene;
                    pawnDangerValue = WinningGenome.GetWinningGenomeInstance().PawnDangerValueGene;
                }
            }

            List<CheckersMove> movesForOtherPlayer = GetMovesForPlayer();

            foreach (CheckersMove move in movesForOtherPlayer)
            {
                CheckersMove moveToCheck = move;
                do
                {
                    if (moveToCheck.JumpedPoint != null)
                    {
                        if (moveToCheck.JumpedPoint.Checker is KingCheckerPiece)
                        {
                            score -= kingDangerValue;
                        }
                        else
                        {
                            score -= pawnDangerValue;
                        }
                    }

                    moveToCheck = moveToCheck.NextMove;
                }
                while (moveToCheck != null);
            }

            return score;
        }
        public void SwapTurns() => CurrentPlayerTurn = CurrentPlayerTurn == PlayerColor.Red ? PlayerColor.Black : PlayerColor.Red;
        
        public List<CheckersMove> GetMovesForPlayer()
        {
            List<CheckersPoint> playersPoints = null;

            if (CurrentPlayerTurn == PlayerColor.Red)
            {
                playersPoints = GetPointsForColor<IRedPiece>();
            }
            else if (CurrentPlayerTurn == PlayerColor.Black)
            {
                playersPoints = GetPointsForColor<IBlackPiece>();
            }
            else
            {
                throw new ArgumentException("Peça desconhecida.");
            }

            List<CheckersMove> allAvailableMoves = new List<CheckersMove>();
            foreach (CheckersPoint checkerPoint in playersPoints)
            {
                allAvailableMoves.AddRange(checkerPoint.GetPossibleMoves(this));
            }

            List<CheckersMove> jumpMoves = new List<CheckersMove>();
            foreach (CheckersMove move in allAvailableMoves)
            {
                if (move.JumpedPoint != null)
                {
                    jumpMoves.Add(move);
                }
            }

            if (jumpMoves.Count > 0)
            {
                return jumpMoves;
            }
            else
            {
                return allAvailableMoves;
            }
        }
        public bool MakeMoveOnBoard(CheckersMove moveToMake) => MakeMoveOnBoard(moveToMake, true);
        public bool MakeMoveOnBoard(CheckersMove moveToMake, bool swapTurn)
        {
            CheckersPoint moveSource = moveToMake.SourcePoint;
            CheckersPoint moveDestination = moveToMake.DestinationPoint;

            if (moveSource != moveDestination)
            {
                CheckersPoint realDestination = this.BoardArray[moveDestination.Row][moveDestination.Column].CheckersPoint;
                CheckersPoint realSource = this.BoardArray[moveSource.Row][moveSource.Column].CheckersPoint;

                realDestination.Checker = (CheckerPiece)realSource.Checker.GetMinimaxClone();
                realSource.Checker = CheckerPieceFactory.GetCheckerPiece(CheckerPieceType.nullPiece);

                CheckersPoint jumpedPoint = moveToMake.JumpedPoint;
                if (jumpedPoint != null)
                {
                    CheckersSquareUserControl jumpedSquareUserControl = this.BoardArray[jumpedPoint.Row][jumpedPoint.Column];
                    jumpedSquareUserControl.CheckersPoint.Checker = CheckerPieceFactory.GetCheckerPiece(CheckerPieceType.nullPiece);
                    jumpedSquareUserControl.UpdateSquare();
                }

                if (!(realDestination.Checker is KingCheckerPiece)
                    && (realDestination.Row == 7 || realDestination.Row == 0))
                {
                    if (realDestination.Checker is IRedPiece)
                    {
                        realDestination.Checker = new RedKingCheckerPiece();
                    }
                    else
                    {
                        realDestination.Checker = new BlackKingCheckerPiece();
                    }
                }

                if (moveToMake.NextMove == null && swapTurn)
                {
                    SwapTurns();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        public override string ToString()
        {
            StringBuilder boardString = new StringBuilder();
            foreach (List<CheckersSquareUserControl> list in BoardArray)
            {
                StringBuilder rowBuilder = new StringBuilder();
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    if (squareUC.CheckersPoint.Checker != null)
                    {
                        rowBuilder.Append(squareUC.CheckersPoint.Checker.GetStringRep());
                    }
                }

                boardString.AppendLine(rowBuilder.ToString());
            }

            return boardString.ToString();
        }

        public object GetMinimaxClone()
        {
            List<List<CheckersSquareUserControl>> rows = new List<List<CheckersSquareUserControl>>();

            for (int row = 0; row < this.BoardArray.Count; row++)
            {
                List<CheckersSquareUserControl> columns = new List<CheckersSquareUserControl>();
                for (int col = 0; col < this.BoardArray[row].Count; col++)
                {
                    columns.Add((CheckersSquareUserControl)this.BoardArray[row][col].GetMinimaxClone());
                }

                rows.Add(columns);
            }

            return new CheckerBoard
            {
                CurrentPlayerTurn = this.CurrentPlayerTurn,
                BoardArray = rows
            };
        }

        private PlayerColor DetermineWhosFirst()
        {
            if (ConstantsSettings.WhosFirst.Equals("black", StringComparison.CurrentCultureIgnoreCase))
            {
                return PlayerColor.Black;
            }
            else
            {
                return PlayerColor.Red;
            }
        }
    }
}