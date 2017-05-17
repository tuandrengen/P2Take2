using System;
using System.Runtime.Serialization;

namespace P2SeriousGame
{
    [Serializable]
    internal class EndOfMapException : Exception
    {
        public EndOfMapException()
        {
        }

        public EndOfMapException(string message) : base(message)
        {
        }

        public EndOfMapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EndOfMapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}