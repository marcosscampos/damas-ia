using Checkers.Presentation.Domain.Interface;
using Checkers.Presentation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Checkers.Presentation.Abstractions
{
    public abstract class CheckerPiece : IMinimaxClonable
    {
        protected string imageSource;

        private static readonly int MoveLeft = -1;
        private static readonly int MoveRight = 1;
        private static readonly int MoveUp = -1;
        private static readonly int MoveDown = 1;

        public abstract List<CheckersMove> GetPossibleMoves(CheckersPoint currentLocation, CheckerBoard board);
        public abstract object GetMinimaxClone();
        public abstract string GetStringRep();

        protected CheckerPiece() {}

        public virtual ImageSource BuildCheckerImageSource() => new BitmapImage(new Uri(imageSource, UriKind.Absolute));

        protected List<CheckersMove> ProcessUpMoves(CheckersPoint currentLocation, CheckerBoard checkerBoard)
        {
            List<CheckersMove> list = new List<CheckersMove>();

            int rowAbove = currentLocation.Row - 1;
            if (rowAbove >= 0)
            {
                list.AddRange(ProcessBoardHorizontal(currentLocation, checkerBoard, rowAbove, MoveUp, MoveRight));

                list.AddRange(ProcessBoardHorizontal(currentLocation, checkerBoard, rowAbove, MoveUp, MoveLeft));
            }

            return ProcessJumpMoves(list);
        }

        protected List<CheckersMove> ProcessDownMoves(CheckersPoint currentLocation, CheckerBoard checkerBoard)
        {
            List<CheckersMove> list = new List<CheckersMove>();

            int rowBelow = currentLocation.Row + 1;
            if (rowBelow < 8)
            {
                list.AddRange(ProcessBoardHorizontal(currentLocation, checkerBoard, rowBelow, MoveDown, MoveRight));

                list.AddRange(ProcessBoardHorizontal(currentLocation, checkerBoard, rowBelow, MoveDown, MoveLeft));
            }

            return ProcessJumpMoves(list);
        }

        private List<CheckersMove> ProcessBoardHorizontal(CheckersPoint currentLocation, CheckerBoard checkerBoard, int oneAdjacentRow, int verticalModifier, int horizontalModifier)
        {
            List<CheckersMove> list = new List<CheckersMove>();
            int adjacentCol = currentLocation.Column + (1 * horizontalModifier);

            if (adjacentCol >= 0 && adjacentCol < 8)
            {
                CheckerPiece possibleCheckerOnPossiblePoint = checkerBoard.BoardArray[oneAdjacentRow][adjacentCol].CheckersPoint.Checker;
                if (possibleCheckerOnPossiblePoint == null || possibleCheckerOnPossiblePoint is NullCheckerPiece)
                {
                    list.Add(new CheckersMove(currentLocation, new CheckersPoint(oneAdjacentRow, adjacentCol)));
                }
                else
                {
                    if ((possibleCheckerOnPossiblePoint is IRedPiece && this is IBlackPiece) ||
                            (possibleCheckerOnPossiblePoint is IBlackPiece && this is IRedPiece))
                    {
                        int twoAdjacentRow = oneAdjacentRow + (1 * verticalModifier);
                        int twoColAdjacent = adjacentCol + (1 * horizontalModifier);

                        if (twoColAdjacent >= 0 && twoColAdjacent < 8 && twoAdjacentRow >= 0 && twoAdjacentRow < 8)
                        {
                            CheckerPiece possibleCheckerOnPossibleJumpPoint = checkerBoard.BoardArray[twoAdjacentRow][twoColAdjacent].CheckersPoint.Checker;
                            if (possibleCheckerOnPossibleJumpPoint == null || possibleCheckerOnPossibleJumpPoint is NullCheckerPiece)
                            {
                                CheckersMove jumpMove = new CheckersMove(currentLocation, new CheckersPoint(twoAdjacentRow, twoColAdjacent), new CheckersPoint(oneAdjacentRow, adjacentCol));

                                CheckerBoard clonedBoard = (CheckerBoard)checkerBoard.GetMinimaxClone();
                                clonedBoard.MakeMoveOnBoard((CheckersMove)jumpMove.GetMinimaxClone(), false);

                                List<CheckersMove> movesAfterJump = this.GetPossibleMoves(jumpMove.DestinationPoint, clonedBoard);

                                List<CheckersMove> processedList = GetJumpMoves(movesAfterJump);

                                if (processedList.Count > 0)
                                {
                                    foreach (CheckersMove move in processedList)
                                    {
                                        CheckersMove clonedMove = (CheckersMove)jumpMove.GetMinimaxClone();
                                        clonedMove.NextMove = move;
                                        list.Add(clonedMove);
                                    }
                                }
                                else
                                {
                                    list.Add(jumpMove);
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private List<CheckersMove> GetJumpMoves(List<CheckersMove> listToProcess)
        {
            List<CheckersMove> processedList = new List<CheckersMove>();

            foreach (CheckersMove move in listToProcess)
            {
                if (move.JumpedPoint != null) processedList.Add(move);
            }

            return processedList;
        }

        private List<CheckersMove> ProcessJumpMoves(List<CheckersMove> listToProcess)
        {
            List<CheckersMove> processedList = GetJumpMoves(listToProcess);

            if (processedList.Count == 0)
                return listToProcess;
            
            return processedList;
        }
    }
}
