
using Checkers.Domain.Enum;
using Checkers.Presentation.Abstractions;
using Checkers.Presentation.Domain.Factory;
using Checkers.Presentation.Domain.Interface;
using System.Collections.Generic;

namespace Checkers.Presentation.Domain.Model
{
    public class CheckersPoint : IMinimaxClonable
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public CheckerPiece Checker { get; set; }

        public CheckersPoint() {}

        public CheckersPoint(int row, int column, CheckerPiece checker)
        {
            Row = row;
            Column = column;
            Checker = checker;
        }

        public CheckersPoint(int row, int column, CheckerPieceType checkerPieceType)
        {
            Row = row;
            Column = column;
            Checker = CheckerPieceFactory.GetCheckerPiece(checkerPieceType);
        }

        public CheckersPoint(int row, int column)
        {
            Row = row;
            Column = column;
            Checker = CheckerPieceFactory.GetCheckerPiece(CheckerPieceType.nullPiece);
        }

        public List<CheckersMove> GetPossibleMoves(CheckerBoard checkerBoard) => Checker.GetPossibleMoves(this, checkerBoard);
        

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is CheckersPoint otherPoint))
            {
                return this.Column == otherPoint.Column && this.Row == otherPoint.Row;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 13;
            hashCode += Row.GetHashCode();
            hashCode += Column.GetHashCode();
            return hashCode;
        }

        public object GetMinimaxClone()
        =>  new CheckersPoint()
            {
                Checker = (CheckerPiece)this.Checker.GetMinimaxClone(),
                Column = this.Column,
                Row = this.Row
            };
    }
}