using Checkers.Presentation.Domain.Interface;
using System.IO;
using System.Reflection;

namespace Checkers.Presentation.Domain.Model
{
    public class RedKingCheckerPiece : KingCheckerPiece, IRedPiece
    {
        public RedKingCheckerPiece() 
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            imageSource = Path.Combine(outputDirectory, "Resources\\red60p_king.png");
        }
        public override object GetMinimaxClone() => new RedKingCheckerPiece();
        public override string GetStringRep() => "| R |";
    }
}
