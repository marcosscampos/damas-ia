using Checkers.Presentation.Domain.Interface;
using System.IO;
using System.Reflection;

namespace Checkers.Presentation.Domain.Model
{
    public class BlackKingCheckerPiece : KingCheckerPiece, IBlackPiece
    {
        public BlackKingCheckerPiece() 
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            imageSource = Path.Combine(outputDirectory, "Resources\\black60p_king.png");
        }
        public override object GetMinimaxClone() => new BlackKingCheckerPiece();
        public override string GetStringRep() => "| B |";
    }
}
