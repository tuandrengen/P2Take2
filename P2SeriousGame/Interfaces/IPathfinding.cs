namespace P2SeriousGame
{
    public interface IPathfinding
    {
       HexagonButton FindPath(HexagonButton[,] hexMap, HexagonButton startingHex);
    }
}
