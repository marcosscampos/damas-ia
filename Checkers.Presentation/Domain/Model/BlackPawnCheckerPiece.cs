
using Checkers.Presentation.Abstractions;
using Checkers.Presentation.Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Checkers.Presentation.Domain.Model
{
    public class BlackPawnCheckerPiece : CheckerPiece, IBlackPiece
    {
        public BlackPawnCheckerPiece()
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            imageSource = Path.Combine(outputDirectory, "Resources\\black60p.png");
        }
        public override object GetMinimaxClone() => new BlackPawnCheckerPiece();
        public override List<CheckersMove> GetPossibleMoves(CheckersPoint currentLocation, CheckerBoard board)  
            => ProcessDownMoves(currentLocation, board);
        public override string GetStringRep() => "| b |";
    }
}
