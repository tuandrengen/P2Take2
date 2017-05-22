using System;

namespace P2SeriousGame
{
    public class MapDimensionsMustBeHigherException : Exception
    {
        public int value;

        public MapDimensionsMustBeHigherException() { }

        public MapDimensionsMustBeHigherException(string message) : base(message) { }

        public MapDimensionsMustBeHigherException(string message, Exception innerException) : base(message, innerException) { }

        public MapDimensionsMustBeHigherException(int value, string v) : base(v)
        {
            this.value = value;
        }
    }
}