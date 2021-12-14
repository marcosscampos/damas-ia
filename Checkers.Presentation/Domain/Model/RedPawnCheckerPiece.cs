using Checkers.Presentation.Abstractions;
using Checkers.Presentation.Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Checkers.Presentation.Domain.Model
{
    public class RedPawnCheckerPiece : CheckerPiece, IRedPiece
    {
        public RedPawnCheckerPiece()
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            imageSource = Path.Combine(outputDirectory, "Resources\\red60p.png");
        }
        public override object GetMinimaxClone() => new RedPawnCheckerPiece();
        public override List<CheckersMove> GetPossibleMoves(CheckersPoint currentLocation, CheckerBoard board)
            => ProcessUpMoves(currentLocation, board);
        public override string GetStringRep() => "| r |";
    }
}
