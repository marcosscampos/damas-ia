using Checkers.Domain.Enum;
using Checkers.Presentation.Abstractions;
using Checkers.Presentation.Domain.Model;
using System;

namespace Checkers.Presentation.Domain.Factory
{
    public static class CheckerPieceFactory
    {
        public static CheckerPiece GetCheckerPiece(CheckerPieceType type)
        {
            return type switch
            {
                CheckerPieceType.RedPawn => new RedPawnCheckerPiece(),
                CheckerPieceType.RedKing => new RedKingCheckerPiece(),
                CheckerPieceType.BlackPawn => new BlackPawnCheckerPiece(),
                CheckerPieceType.BlackKing => new BlackKingCheckerPiece(),
                CheckerPieceType.nullPiece => new NullCheckerPiece(),
                _ => throw new ArgumentException("Enum to Checker Piece not defined"),
            };
        }
    }
}
