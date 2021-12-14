
using Checkers.Presentation.Abstractions;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Checkers.Presentation.Domain.Model
{
    public class NullCheckerPiece : CheckerPiece
    {
        public NullCheckerPiece() => imageSource = null;
        public override ImageSource BuildCheckerImageSource() => null;
        public override object GetMinimaxClone() => new NullCheckerPiece();
        public override List<CheckersMove> GetPossibleMoves(CheckersPoint currentLocation, CheckerBoard checkerBoard) => new List<CheckersMove>();
        public override string GetStringRep() => "|   |";

    }
}
