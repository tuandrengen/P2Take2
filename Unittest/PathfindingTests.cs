using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;

namespace Unittest
{
    //Finde ud af hvad bfs.FindShortestRoutes() retunerer. Svar: den returnere de mulige edgetiles hvortil der er kortest afstand.
    //Finde ud af hvad bfs.FindLongestRoutes() retunerer. Svar: den returnere de mulige edgetiles hvortil der er længst afstand.

    [TestFixture]
    public class PathfindingTests
    {

        Initializer initializer = new Initializer();
        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        [TestCase(17, 17)]
        public void CalculateRoutes_HexMapWithEdges_FindsLongestRoutes(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeBFS();
            List<HexagonButton> edgeTiles = new List<HexagonButton>();

            int addValue = (x / 4) - 1;
            //Lenght from middle to the tile furthest away.
            int fromMiddleToTileNextToEdge = (x / 2) + addValue;
            int tempCost = 0;

            foreach (var hexagonButton in Map.hexMap)
            {
                //Makes every edgetile inaccessible.
                if (hexagonButton.IsEdgeTile == true)
                {
                    hexagonButton.Passable = false;
                    edgeTiles.Add(hexagonButton);
                }
            }
            initializer.bfs.CalculateRoutes(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            //Makes sure, that FindTheRoutes wont find any of the dissabled edgetiles.
            Assert.AreNotEqual(edgeTiles, initializer.bfs.FindTheRoutes());
            foreach (HexagonButton hex in initializer.bfs.FindLongestRoutes())
            {
                Assert.AreEqual(fromMiddleToTileNextToEdge, hex.CostToStart);
            }
            foreach (HexagonButton hex in initializer.bfs.FindLongestRoutes())
            {
                //Tjecks that the route found are not longer than any of map dimension
                Assert.IsTrue(hex.CostToStart < x || hex.CostToStart < y);
            }

            //Tjecks if all hex got same cost.
            foreach (HexagonButton hex in initializer.bfs.FindLongestRoutes())
            {
                if (tempCost == 0)
                {
                    tempCost = hex.CostToStart;
                }
                Assert.AreEqual(tempCost, hex.CostToStart);
            }
        }


        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        [TestCase(17, 17)]
        public void FindShortestRoutes_HexMapWithEdges_FindsRightEndTiles(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeBFS();
            initializer.bfs.CalculateRoutes(Map.hexMap, Map.hexMap[x / 2, y / 2]);
            
            //Check if each route has the lowest and the same cost.
            foreach (HexagonButton hex in initializer.bfs.FindShortestRoutes())
            {
                Assert.AreEqual(x / 2, hex.CostToStart);
            }
        }

        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        [TestCase(17, 17)]
        public void FindShortestRoutes_HexMapWithEdges_FindsRightAmountOfEndTiles(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeBFS();

            initializer.bfs.CalculateRoutes(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            //Check if each endtile if found from startpoint.
            Assert.AreEqual(x + 5, initializer.bfs.FindShortestRoutes().Count);
            //x + 5 different edgetiles the mouse can go to from startpoint.
        }



        [TestCase(1, 1, false, 0)]
        [TestCase(2, 2, false, 1)]
        [TestCase(3, 3, false, 5)]
        [TestCase(4, 4, false, 10)]
        public void CheckParents_ChainOfHex_FindsCurrentAmountOfParents(int x, int y, bool edge, int length)
        {
            initializer.InitializeBFS();

            List<HexagonButton> hexes = new List<HexagonButton>();
            
            //When path length is zero.
            if (length == 0)
            {
                HexagonButton hex = new HexagonButton(x, y, edge);
                hexes.Add(hex);
                hexes[0].parent = null;
                Assert.AreEqual(length, initializer.bfs.CheckParent(hexes[0]));
            }

            //Create hexes.
            for (int i = 0; i < length; i++)
            {
                HexagonButton hex = new HexagonButton(x, y, edge);
                hexes.Add(hex);
            }

            //Creates parents to hexes.
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

            //Checks if correct amount of parents for each hex is found.
            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                {
                    Assert.AreEqual(0, initializer.bfs.CheckParent(hexes[i]));
                }
                else
                {
                    //length-i-1, -i because the length should be 1 less each time.
                    //Minus 1 because the variable "length" is not 0-indexet but the list is.
                    Assert.AreEqual(length - i - 1, initializer.bfs.CheckParent(hexes[i]));
                }
            }
        }

        [TestCase(9, 9)]
        [TestCase(11, 11)]
        [TestCase(13,13)]
        public void FindPath_FindsMouseButtonsNextHexTile_FindTileNextToMouse(int x, int y)
        {
            initializer.InitializeMap(x);
            
            HexagonButton nextTile = initializer.pathfinding.FindPath(Map.hexMap, Map.hexMap[x / 2, y / 2]);

            Assert.IsTrue(x/2 - 1 <= nextTile.XCoordinate && nextTile.XCoordinate <= x/2 + 1);
            Assert.IsTrue(y/2 - 1 <= nextTile.YCoordinate && nextTile.YCoordinate <= y/2 + 1);
        }
    }
}
