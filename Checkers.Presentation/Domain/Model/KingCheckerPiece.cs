
using Checkers.Presentation.Abstractions;
using System.Collections.Generic;

namespace Checkers.Presentation.Domain.Model
{
    public abstract class KingCheckerPiece : CheckerPiece
    {
        public override List<CheckersMove> GetPossibleMoves(CheckersPoint currentLocation, CheckerBoard board)
        {
            List<CheckersMove> list = new List<CheckersMove>();

            list.AddRange(ProcessDownMoves(currentLocation, board));
            list.AddRange(ProcessUpMoves(currentLocation, board));

            return list;
        }
    }
}
