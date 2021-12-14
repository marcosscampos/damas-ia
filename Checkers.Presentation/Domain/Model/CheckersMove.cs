using Checkers.Presentation.Domain.Interface;

namespace Checkers.Presentation.Domain.Model
{
    public class CheckersMove : IMinimaxClonable
    {
        public CheckersPoint SourcePoint { get; set; }
        public CheckersPoint DestinationPoint { get; set; }
        public CheckersPoint JumpedPoint { get; set; }
        public CheckersMove NextMove { get; set; }

        public CheckersMove(CheckersPoint sourcePoint, CheckersPoint destinationPoint)
        {
            SourcePoint = sourcePoint;
            DestinationPoint = destinationPoint;
            JumpedPoint = null;
            NextMove = null;
        }
        public CheckersMove(CheckersPoint sourcePoint, CheckersPoint destinationPoint, CheckersPoint jumpedPoint)
        {
            SourcePoint = sourcePoint;
            DestinationPoint = destinationPoint;
            JumpedPoint = jumpedPoint;
            NextMove = null;
        }

        public CheckersMove(CheckersPoint sourcePoint, CheckersPoint destinationPoint, CheckersPoint jumpedPoint, CheckersMove nextMove)
        {
            SourcePoint = sourcePoint;
            DestinationPoint = destinationPoint;
            JumpedPoint = jumpedPoint;
            NextMove = nextMove;
        }
        public CheckersMove() { }

        public object GetMinimaxClone()
        {
            CheckersMove clone = new CheckersMove();

            if (this.DestinationPoint != null)
            {
                clone.DestinationPoint = (CheckersPoint)this.DestinationPoint.GetMinimaxClone();
            }

            if (this.SourcePoint != null)
            {
                clone.SourcePoint = (CheckersPoint)this.SourcePoint.GetMinimaxClone();
            }

            if (this.JumpedPoint != null)
            {
                clone.JumpedPoint = (CheckersPoint)this.JumpedPoint.GetMinimaxClone();
            }

            if (this.NextMove != null)
            {
                clone.NextMove = (CheckersMove)this.NextMove.GetMinimaxClone();
            }

            return clone;
        }
    }
}