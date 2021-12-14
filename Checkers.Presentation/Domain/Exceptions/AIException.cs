using System;
using System.Collections.Generic;
using System.Text;

namespace Checkers.Presentation.Domain.Exceptions
{
    [Serializable]
    public class AIException : Exception
    {
        public AIException(string message) : base(message){}
    }
}
