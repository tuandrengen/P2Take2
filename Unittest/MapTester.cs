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
        Initializer initializer = new Initializer();
        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(21, 21)]
        public void MapTest_ValidDimension_HexmapRightSize(int x, int y)
        {
            initializer.InitializeMap(x);
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
                initializer.InitializeMap(x);
            }
            catch (MapDimensionsMustBeHigherException e)
            {
                Assert.AreEqual(typeof(MapDimensionsMustBeHigherException), e.GetType());
            }
        }

        [TestCase(6, 6)]
        [TestCase(10, 10)]
        public void MapTest_WrongDimension_ThrowMapDimensionsMustBeOdd(int x, int y)
        {
            try
            {
                initializer.InitializeMap(x);
            }
            catch (MapDimensionsMustBeOddException e)
            {
                Assert.AreEqual(typeof(MapDimensionsMustBeOddException), e.GetType());
            }
        }

        [TestCase(11, 11)]
        [TestCase(13, 13)]
        public void FindNeighbours_ValidDimension_RightAmountOfNeighboursForEachTileOnRoute(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeMouseEventArgs();
            HexagonButton onlyForParameter = new HexagonButton(x / 2, y / 2, false);
            int numberOfTilesOnRute = (x / 2);

            //To indicate it's a newgame.
            Map.newGame = true;
                        
            for (int i = 0; i < numberOfTilesOnRute; i++)
            {
                initializer.map.MousePositioner(onlyForParameter, initializer.mouseArg);

                if (!Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].IsEdgeTile)
                {
                    //Non-edgetile do always have 6 neighbours.
                    Assert.AreEqual(6, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].neighbourList.Count);
                }
                else if (Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].IsEdgeTile)
                {
                    //Edgetiles has zero neighbours
                    Assert.AreEqual(0, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].neighbourList.Count);
                }
            }
        }

        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        public void FindNeighbours_CorrectDimensionRightAmountOfNeighbours_RightCoordinatesForTheFoundNeighbours(int x, int y)
        {
            initializer.InitializeMap(x);
                        
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (!Map.hexMap[i, j].IsEdgeTile)
                    {
                        //Always got theses neighbours. 
                        Assert.AreEqual(i - 1, Map.hexMap[i, j].neighbourList[0].XCoordinate);
                        Assert.AreEqual(j, Map.hexMap[i, j].neighbourList[0].YCoordinate);
                        Assert.AreEqual(i + 1, Map.hexMap[i, j].neighbourList[1].XCoordinate);
                        Assert.AreEqual(j, Map.hexMap[i, j].neighbourList[1].YCoordinate);

                        if (j % 2 == 1)
                        {
                            // odd numbered rows do also have these neighbours 
                            Assert.AreEqual(i, Map.hexMap[i, j].neighbourList[2].XCoordinate);
                            Assert.AreEqual(j - 1, Map.hexMap[i, j].neighbourList[2].YCoordinate);
                            Assert.AreEqual(i + 1, Map.hexMap[i, j].neighbourList[3].XCoordinate);
                            Assert.AreEqual(j - 1, Map.hexMap[i, j].neighbourList[3].YCoordinate);
                            Assert.AreEqual(i, Map.hexMap[i, j].neighbourList[4].XCoordinate);
                            Assert.AreEqual(j + 1, Map.hexMap[i, j].neighbourList[4].YCoordinate);
                            Assert.AreEqual(i + 1, Map.hexMap[i, j].neighbourList[5].XCoordinate);
                            Assert.AreEqual(j + 1, Map.hexMap[i, j].neighbourList[5].YCoordinate);
                        }
                        else if (j % 2 == 0)
                        {
                            // even numbered rows do also have these neighbours 
                            Assert.AreEqual(i, Map.hexMap[i, j].neighbourList[2].XCoordinate);
                            Assert.AreEqual(j - 1, Map.hexMap[i, j].neighbourList[2].YCoordinate);
                            Assert.AreEqual(i - 1, Map.hexMap[i, j].neighbourList[3].XCoordinate);
                            Assert.AreEqual(j - 1, Map.hexMap[i, j].neighbourList[3].YCoordinate);
                            Assert.AreEqual(i, Map.hexMap[i, j].neighbourList[4].XCoordinate);
                            Assert.AreEqual(j + 1, Map.hexMap[i, j].neighbourList[4].YCoordinate);
                            Assert.AreEqual(i - 1, Map.hexMap[i, j].neighbourList[5].XCoordinate);
                            Assert.AreEqual(j + 1, Map.hexMap[i, j].neighbourList[5].YCoordinate);
                        }
                    }
                }
            }
        }


        [TestCase(13, 13, 11, 11)]
        [TestCase(9, 9, 7, 7)]
        [TestCase(7, 7, 2, 2)]
        [TestCase(5, 5, 3, 3)]
        public void FindNeighbours_CoordinatesOfNonEdgeTileAndValidDimension_RightAmountOfNeighboursForNonEdgeTile(int x, int y, int buttomX, int buttomY)
        {
            initializer.InitializeMap(x);
            
            //All non-edgetiles got 6 neighbours.
            Assert.AreEqual(6, Map.hexMap[buttomX, buttomY].neighbourList.Count);

        }
        [TestCase(13, 13, 12, 12)]
        [TestCase(9, 9, 8, 8)]
        [TestCase(7, 7, 6, 6)]
        [TestCase(5, 5, 4, 4)]
        public void FindNeighbours_CoordinatesOfEdgeTileAndValidDimension_RightAmountOfNeighboursForEdgeTile(int x, int y, int buttomX, int buttomY)
        {
            initializer.InitializeMap(x);

            //All edgetiles got 0 neighbours.
            Assert.AreEqual(0, Map.hexMap[buttomX, buttomY].neighbourList.Count);
        }

        [TestCase(11, 11)]
        [TestCase(13,13)]
        [TestCase(15,15)]
        public void CreateMap_CalculateButtonDimensionWorksAndValidDimension_RightCoordinates(int x, int y)
        {

            initializer.InitializeMap(x);
            //Double for-loop is used to run throw all coordinates on a 2-Dimensional map.
            //varible i represents x-coordinate, and j represents y-coordinate.
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Assert.AreEqual(i, Map.hexMap[i, j].XCoordinate);
                    Assert.AreEqual(j, Map.hexMap[i, j].YCoordinate);
                }
            }
        }

        [TestCase(11, 11)]
        [TestCase(13, 13)]
        [TestCase(15, 15)]
        public void CreateMap_ValidMapDimension_RightEdgeTiles(int x, int y)
        {
            initializer.InitializeMap(x);
            //Double for-loop is used to run throw all coordinates on a 2-Dimensional map.
            //varible i represents x-coordinate, and j represents y-coordinate.

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    //The coordinates that an edgetile can have.
                    if (i == 0 || i == x - 1 || j == 0 || j == y - 1)
                    {
                        Assert.AreEqual(true, Map.hexMap[i, j].IsEdgeTile);
                    }
                    else
                    {
                        Assert.AreEqual(false, Map.hexMap[i, j].IsEdgeTile);
                    }
                }
            }
        }

        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(21, 21)]
        public void MapTest_PositiveOddValues_ConstructedRight(int x, int y)
        {
            initializer.InitializeMap(x);
            Assert.AreEqual(x, Map.TotalHexagonColumns);
            Assert.AreEqual(y, Map.TotalHexagonRows);
            //Checks if number of cells in array in each direction (x,y) is correct.
            Assert.AreEqual(x, Map.hexMap.GetLength(0));
            Assert.AreEqual(y, Map.hexMap.GetLength(1));
        }

        //This tests 
        /*
         * 
         * 
         * 
         * 
         */
        //Denne tester også at musen følger den rigtige rute ved at man kan se, at musen bliver flyttet hvergang til næste sted på pathen, da MouseYCoordinate og X er dannet ud fra den første hex i pathen.
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(21, 21)]
        public void MousePositioner_ValidMapSize_MouseMovementColorsCorrectly(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeMouseEventArgs();
            HexagonButton onlyForParameter = new HexagonButton(x / 2, y / 2, false);
            int LastX;
            int LastY;
            int startX = x / 2;
            int startY = y / 2;
            int numberOfTilesOnRute = (x / 2) - 1;
            Map.newGame = true;

            initializer.map.MousePositioner(onlyForParameter, initializer.mouseArg);
            Assert.AreEqual(false, Map.newGame);
            Assert.AreEqual(Color.LightGray, Map.hexMap[startX, startY].BackColor);
            Assert.AreEqual(true, Map.hexMap[startX, startY].Enabled);
            Assert.AreEqual(Color.Aqua, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].BackColor);
            Assert.AreEqual(false, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].Enabled);

            /* Saves the current mouseposition before it gets overriden in the next call. 
             * They are going to be used to check if the current mouseposition will be colored grey and enabled after next call.
             * Because MouseX and MouseY are equal the coordinates of the first hex in the path. 
             * In that way we kinda test that the mouse will move along the path. */
            LastX = initializer.map.MouseXCoordinate;
            LastY = initializer.map.MouseYCoordinate;

            for (int i = 0; i < numberOfTilesOnRute; i++)
            {
                initializer.map.MousePositioner(onlyForParameter, initializer.mouseArg);
                Assert.AreEqual(Color.LightGray, Map.hexMap[LastX, LastY].BackColor);
                Assert.AreEqual(true, Map.hexMap[LastX, LastY].Enabled);
                Assert.AreEqual(Color.Aqua, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].BackColor);
                Assert.AreEqual(false, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].Enabled);
                LastX = initializer.map.MouseXCoordinate;
                LastY = initializer.map.MouseYCoordinate;
            }
        }

        [Test]
        public void ResetMouse_None_SetCorrectValue()
        {
            Map.ResetMouse();
            Assert.AreEqual(true, Map.newGame);
        }

    }
}
