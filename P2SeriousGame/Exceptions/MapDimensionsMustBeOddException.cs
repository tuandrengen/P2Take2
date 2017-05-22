using System;

namespace P2SeriousGame
{
    public class MapDimensionsMustBeOddException : Exception
    {
        private int value;

        public MapDimensionsMustBeOddException() { }

        public MapDimensionsMustBeOddException(string message) : base(message) { }

        public MapDimensionsMustBeOddException(int value, string message) : base(message)
        {
            this.value = value;
        }

        public MapDimensionsMustBeOddException(string message, Exception innerException) : base(message, innerException) { }
    }
}
