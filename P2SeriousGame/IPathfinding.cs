using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2SeriousGame
{
    public interface IPathfinding
    {
        HexagonButton CalculateRoutes(HexagonButton[,] hexMap, HexagonButton startingHex);
    }
}
