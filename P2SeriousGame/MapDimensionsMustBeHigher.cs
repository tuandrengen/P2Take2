using System;
using System.Runtime.Serialization;

namespace P2SeriousGame
{
    public class MapDimensionsMustBeHigher : Exception
    {
        public int value;

        public MapDimensionsMustBeHigher()
        {
        }

        public MapDimensionsMustBeHigher(string message) : base(message)
        {
        }

        public MapDimensionsMustBeHigher(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MapDimensionsMustBeHigher(int value, string v) : base(v)
        {
            this.value = value;
            
        }
    }
}