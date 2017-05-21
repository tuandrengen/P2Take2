using System;

namespace P2SeriousGame
{
    public class MustBePositiveException : Exception
    {
        public MustBePositiveException() { }

        public MustBePositiveException(string message) : base(message) { }
    }
}