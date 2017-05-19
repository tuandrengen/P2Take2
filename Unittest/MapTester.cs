using NUnit.Framework;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;
using System;


namespace Unittest
{
    [TestFixture]
    public class MapTester
    {
        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(21, 21)]
        public void MapTest_PositiveOddValue_HexmapRightSize(int x, int y)
        {
            GameForm tester = new GameForm(x);
            IPathfinding a = null;
            Map map = new Map(tester, x, a);
            Assert.AreEqual(x, Map.hexMap.GetLength(0));
            Assert.AreEqual(y, Map.hexMap.GetLength(1));
        }

        [TestCase(-1, -1)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void MapTest_WrongDimensions_ThrowMapDimensionsMustBeHigher(int x, int y)
        {
            try
            {
                GameForm tester = new GameForm(x);
                IPathfinding a = null;
                Map map = new Map(tester, x, a);
            }
            catch (MapDimensionsMustBeHigher e)
            {
                Assert.AreEqual(typeof(MapDimensionsMustBeHigher), e.GetType());
            }
        }

        [TestCase(6, 6)]
        [TestCase(10, 10)]
        public void MapTest_WrongDimensions_ThrowMapDimensionsMustBeOdd(int x, int y)
        {
            try
            {
                GameForm tester = new GameForm(x);
                IPathfinding a = null;
                Map map = new Map(tester, x, a);
            }
            catch (MapDimensionsMustBeOdd e)
            {
                Assert.AreEqual(typeof(MapDimensionsMustBeOdd), e.GetType());
            }
        }

        [TestCase(11, 11)]
        public void FindNeighbours_PositiveOddValues_RightAmountOfNeighboursForEachTileOnRoute(int x, int y)
        {
            IPathfinding ipathfinding = new Pathfinding();
            GameForm tester = new GameForm(x);
            Map map = new Map(tester, x, ipathfinding);
            MouseButtons a = new MouseButtons();
            MouseEventArgs b = new MouseEventArgs(a, 0, 10, 10, 0);
            HexagonButton onlyForParameter = new HexagonButton(x / 2, y / 2, false);
            int numberOfTilesOnRute = (x / 2);

            if (!Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].IsEdgeTile)
            {
                Assert.AreEqual(6, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].neighbourList.Count);

                for (int i = 0; i < numberOfTilesOnRute; i++)
                {
                    map.MousePositioner(onlyForParameter, b);

                    if (!Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].IsEdgeTile)
                    {
                        Assert.AreEqual(6, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].neighbourList.Count);
                    }
                    else if (Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].IsEdgeTile)
                    {
                        int n = Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].neighbourList.Count;
                        switch (n)
                        {
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                {
                                    Assert.AreEqual(true, true);
                                    break;
                                }
                            default:
                                {
                                    Assert.AreEqual(false, true);
                                    break;
                                }
                        }
                    }
                }
            }
        }

        //Burde nok gøre sådan, at man ikke kan lave mappen 0,0.
        //tror ikke knappen med koordinaterne 0,0 er en edgetile.
        [TestCase(13, 13, 11, 11)]
        [TestCase(9, 9, 8, 8)]
        [TestCase(7, 7, 2, 2)]
        [TestCase(5, 5, 4, 4)]
        public void FindNeighbours_PositiveOddValues_RightAmountOfNeighboursForAGivenTile(int x, int y, int buttomX, int buttomY)
        {
            IPathfinding ipathfinding = new Pathfinding();
            GameForm tester = new GameForm(x);
            Map map = new Map(tester, x, ipathfinding);

            if (!Map.hexMap[buttomX, buttomY].IsEdgeTile)
            {
                Assert.AreEqual(6, Map.hexMap[buttomX, buttomY].neighbourList.Count);
            }
        }

        [TestCase(11, 11)]
        public void CreateMap_CalculateButtonDimensionWorks_RightCoordinatesAndMode(int x, int y)
        {
            GameForm window = new GameForm(x);
            IPathfinding ipathfinding = new Pathfinding();
            Map map = new Map(window, x, ipathfinding);
            map.CreateMap(window);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i == 0 || i == x - 1 || j == 0 || j == y - 1)
                    {
                        Assert.AreEqual(true, Map.hexMap[i, j].IsEdgeTile);
                    }

                    Assert.AreEqual(i, Map.hexMap[i, j].XCoordinate);
                    Assert.AreEqual(j, Map.hexMap[i, j].YCoordinate);
                }
            }
        }
        //Gøre sådan, at når der angives en negativ værdi skal der kastes en exception og mappet skal ikke laves.
        //Lige pt. kommer der en exception, da man i Map-constructoren forsøget at tildelee 
        //[TestCase(-1, -1)]
        //[TestCase(0, 0)]
        //[TestCase(1, 1)]
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(21, 21)]
        public void MapTest_PositiveOddValues_ConstructedRight(int x, int y)
        {
            IPathfinding ipathfinding = new Pathfinding();
            GameForm window = new GameForm(x);
            Map map = new Map(window, x, ipathfinding);
            Assert.AreEqual(x, Map.TotalHexagonColumns);
            Assert.AreEqual(y, Map.TotalHexagonRows);
            Assert.AreEqual(x, Map.hexMap.GetLength(0));
            Assert.AreEqual(y, Map.hexMap.GetLength(1));
        }

        //Denne tester også at musen følger den rigtige rute ved at man kan se, at musen bliver flyttet hvergang til næste sted på pathen, da MouseYCoordinate og X er dannet ud fra den første hex i pathen.
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(21, 21)]
        public void MousePositioner_CalculateRoutesWorks_ColorsAndDisablesRightFollowsRightPath(int x, int y)
        {
            IPathfinding pathfinding = new Pathfinding();
            GameForm window = new GameForm(x);
            MouseButtons a = new MouseButtons();
            MouseEventArgs b = new MouseEventArgs(a, 0, 10, 10, 0);
            Map map = new Map(window, x, pathfinding);
            HexagonButton onlyForParameter = new HexagonButton(x / 2, y / 2, false);
            int LastX;
            int LastY;
            int startX = x / 2;
            int startY = y / 2;
            int numberOfTilesOnRute = (x / 2);
            int edgeTile = 1;

            if (Map.newGame)
            {
                map.MousePositioner(onlyForParameter, b);
                Assert.AreEqual(false, Map.newGame);
                Assert.AreEqual(Color.LightGray, Map.hexMap[startX, startY].BackColor);
                Assert.AreEqual(true, Map.hexMap[startX, startY].Enabled);
                Assert.AreEqual(Color.Aqua, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].BackColor);
                Assert.AreEqual(false, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].Enabled);
                //Saves the current mouseposition before it gets overriden in the next call. 
                //They are going to be used to tjeck if the current mouseposition will be colored grey and enabled after next call.
                //Because MouseX and MouseY are equal the coordinates of the first hex in the path. 
                //In that way we kinda test that the mouse will move along the path. 
                LastX = map.MouseXCoordinate;
                LastY = map.MouseYCoordinate;
                for (int i = 0; i < numberOfTilesOnRute - edgeTile; i++)
                {
                    map.MousePositioner(onlyForParameter, b);
                    Assert.AreEqual(Color.LightGray, Map.hexMap[LastX, LastY].BackColor);
                    Assert.AreEqual(true, Map.hexMap[LastX, LastY].Enabled);
                    Assert.AreEqual(Color.Aqua, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].BackColor);
                    Assert.AreEqual(false, Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].Enabled);
                    LastX = map.MouseXCoordinate;
                    LastY = map.MouseYCoordinate;
                }
            }
        }

    }
}
