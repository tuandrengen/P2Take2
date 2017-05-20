using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2SeriousGame
{
    class GameWonException : Exception
    {
        public GameWonException() { }
        public GameWonException(string message) : base(message) { }
        public GameWonException(string message, Exception inner) : base(message, inner) { }
        protected GameWonException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
