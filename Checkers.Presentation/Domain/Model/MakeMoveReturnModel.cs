using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Presentation.Domain.Model
{
    public class MakeMoveReturnModel
    {
        public bool WasMoveMade { get; set; }
        public bool IsTurnOver { get; set; }
    }
}
