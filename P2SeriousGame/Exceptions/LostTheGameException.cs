using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2SeriousGame
{

    [Serializable]
    public class LostTheGameException : Exception
    {
        public LostTheGameException() { }
        public LostTheGameException(string message) : base(message) { }
        public LostTheGameException(string message, Exception inner) : base(message, inner) { }
        protected LostTheGameException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
