using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;

namespace UnitTests
{
    //Finde ud af hvad bfs.FindShortestRoutes() retunerer. Svar: den returnere de mulige edgetiles hvortil der er kortest afstand.
    //Finde ud af hvad bfs.FindLongestRoutes() retunerer. Svar: den returnere de mulige edgetiles hvortil der er længst afstand.

    [TestFixture]
    public class PathfindingTests
    {
        [TestCase(9, 9)]
        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        [TestCase(17, 17)]
        [TestCase(19, 19)]
        [TestCase(21, 21)]
        [TestCase(23, 23)]
        [TestCase(25, 25)]
        public void CalculateRoutes_HexMapWithEdges_FindsLongestRoutes(int x, int y)
        {
            List<HexagonButton> queue = new List<HexagonButton>();
            List<HexagonButton> pathsToEdge = new List<HexagonButton>();
            List<HexagonButton> reachableHexList = new List<HexagonButton>();
            List<HexagonButton> edgeTiles = new List<HexagonButton>();
            GameForm window = new GameForm(x);
            IPathfinding pathfindning = new Pathfinding();
            Map map = new Map(window, x, pathfindning);
            BreadthFirst bfs = new BreadthFirst(queue, pathsToEdge, reachableHexList);
            int addValue = (x / 4) - 1;
            int fromMiddleToTileNextToEdge = (x / 2) + addValue;
            int tempCost = 0;

            foreach (var hexagonButton in Map.hexMap)
            {
                if (hexagonButton.IsEdgeTile == true)
                {
                    hexagonButton.Passable = false;
                    edgeTiles.Add(hexagonButton);
                }
            }
            bfs.CalculateRoutes(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            Assert.AreNotEqual(edgeTiles, bfs.FindTheRoutes());
            foreach (HexagonButton hex in bfs.FindLongestRoutes())
            {
                Assert.AreEqual(fromMiddleToTileNextToEdge, hex.CostToStart);
            }
            foreach (HexagonButton hex in bfs.FindLongestRoutes())
            {
                Assert.IsTrue(hex.CostToStart < x || hex.CostToStart < y);
            }

            foreach (HexagonButton hex in bfs.FindLongestRoutes())
            {
                if (tempCost == 0)
                    tempCost = hex.CostToStart;
                Assert.AreEqual(tempCost, hex.CostToStart);
            }
        }

        [TestCase(9, 9)]
        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        [TestCase(17, 17)]
        [TestCase(19, 19)]
        [TestCase(21, 21)]
        [TestCase(23, 23)]
        [TestCase(25, 25)]
        public void FindShortestRoutes_HexMapWithEdges_FindsRightEndTilesAndRightAmount(int x, int y)
        {
            List<HexagonButton> queue = new List<HexagonButton>();
            List<HexagonButton> pathsToEdge = new List<HexagonButton>();
            List<HexagonButton> reachableHexList = new List<HexagonButton>();
            GameForm window = new GameForm(x);
            IPathfinding pathfindning = new Pathfinding();
            Map map = new Map(window, x, pathfindning);
            HexagonButton[,] notStaticHexMap = Map.hexMap;
            BreadthFirst bfs = new BreadthFirst(queue, pathsToEdge, reachableHexList);

            bfs.CalculateRoutes(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            //Check if each route has the lowest and the same cost.
            foreach (HexagonButton hex in bfs.FindShortestRoutes())
            {
                Assert.AreEqual(x / 2, hex.CostToStart);
            }
            //Check if each endtile if found from startpoint.
            Assert.AreEqual(x + 5, bfs.FindShortestRoutes().Count);
            //x + 5 different edgetiles the mouse can go to from startpoint.
        }

        [TestCase(1, 1, false, 0)]
        [TestCase(2, 2, false, 1)]
        [TestCase(3, 3, false, 5)]
        [TestCase(4, 4, false, 10)]
        public void CheckParents_ChainOfHex_FindsCurrentAmountOfParents(int x, int y, bool edge, int length)
        {
            List<HexagonButton> queue = new List<HexagonButton>();
            List<HexagonButton> pathsToEdge = new List<HexagonButton>();
            List<HexagonButton> reachableHexList = new List<HexagonButton>();
            BreadthFirst bfs = new BreadthFirst(queue, pathsToEdge, reachableHexList);

            List<HexagonButton> hexes = new List<HexagonButton>();

            if (length == 0)
            {
                HexagonButton hex = new HexagonButton(x, y, edge);
                hexes.Add(hex);
                hexes[0].parent = null;
                Assert.AreEqual(length, bfs.CheckParent(hexes[0]));
            }

            for (int i = 0; i < length; i++)
            {
                HexagonButton hex = new HexagonButton(x, y, edge);
                hexes.Add(hex);
            }

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    hexes[i].parent = null;
                }
                else
                {
                    hexes[i].parent = hexes[i + 1];
                }
            }

            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    Assert.AreEqual(0, bfs.CheckParent(hexes[i]));
                }
                else
                {
                    //length-i-1, -i because the length should be 1 less each time.
                    //Minus 1 because the variable "length" is not 0-indexet but the list is.
                    Assert.AreEqual(length - i - 1, bfs.CheckParent(hexes[i]));
                }

            }
        }

        [TestCase(7, 7)]
        [TestCase(35, 35)]
        public void FindPath_FindsMouseButtonsNextHexTile_FindTileNextToMouse(int x, int y)
        {
            GameForm window = new GameForm(x);
            IPathfinding pathfindning = new Pathfinding();
            Map map = new Map(window, x, pathfindning);

            HexagonButton nextTile = pathfindning.FindPath(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            Assert.IsTrue(x/2 - 1 <= nextTile.XCoordinate && nextTile.XCoordinate <= x/2 + 1);
            Assert.IsTrue(y/2 - 1 <= nextTile.YCoordinate && nextTile.YCoordinate <= y/2 + 1);
        }
    }
}
