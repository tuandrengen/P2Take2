﻿using NUnit.Framework;
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
        public void MapTest_PositiveOddValue_HexmapRightSize(int x, int y)
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
        public void MapTest_WrongDimensions_ThrowMapDimensionsMustBeOdd(int x, int y)
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
        [TestCase(13,13)]
        public void FindNeighbours_PositiveOddValues_RightAmountOfNeighboursForEachTileOnRoute(int x, int y)
        {
            initializer.InitializeMap(x);
            initializer.InitializeMouseEventArgs();
            HexagonButton onlyForParameter = new HexagonButton(x / 2, y / 2, false);
            int numberOfTilesOnRute = (x / 2);
            

            for (int i = 0; i <= numberOfTilesOnRute; i++)
            {
                initializer.map.MousePositioner(onlyForParameter, initializer.mouseArg);

                if (!Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].IsEdgeTile)
                {
                    //Non-edgetile do always have 6 neighbours.
                    Assert.AreEqual(6, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].neighbourList.Count);
                }
                else if (Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].IsEdgeTile)
                {
                    //Åbenbart har alle edgetiles 0 neighbours.
                    //Det er fordi, at vi bare sætter neightbours til 0 for edgetiles, da vi ikke skal gå videre når vi når en edgetile og det er derfor ikke nødvendigt at udregne antal naboer.
                    Assert.AreEqual(0, Map.hexMap[initializer.map.MouseXCoordinate, initializer.map.MouseYCoordinate].neighbourList.Count);
                    
                    //The different amount of neighbours a edgetile can have.
                    //int n = Map.hexMap[map.MouseXCoordinate, map.MouseYCoordinate].neighbourList.Count;
                    //switch (n)
                    //{
                    //    case 0:
                    //    case 1:
                    //    case 2:
                    //    case 3:
                    //    case 4:
                    //    case 5:
                    //        {
                    //            Assert.AreEqual(true, true);
                    //            break;
                    //        }
                    //    default:
                    //        {
                    //            Assert.AreEqual(1, n);
                    //            break;
                    //        }
                    //}
                }
            }
        }

        //Burde nok gøre sådan, at man ikke kan lave mappen 0,0.
        //tror ikke knappen med koordinaterne 0,0 er en edgetile.
        [TestCase(13, 13, 11, 11)]
        [TestCase(9, 9, 8, 8)]
        [TestCase(7, 7, 2, 2)]
        [TestCase(5, 5, 4, 4)]
        public void FindNeighbours_PositiveOddValues_RightAmountOfNeighboursForNonEdgeTile(int x, int y, int buttomX, int buttomY)
        {
            initializer.InitializeMap(x);

            if (!Map.hexMap[buttomX, buttomY].IsEdgeTile)
            {
                //All non-edgetiles got 6 neighbours.
                Assert.AreEqual(6, Map.hexMap[buttomX, buttomY].neighbourList.Count);
            }
        }

        [TestCase(11, 11)]
        public void CreateMap_CalculateButtonDimensionWorks_RightCoordinatesAndMode(int x, int y)
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

                    Assert.AreEqual(i, Map.hexMap[i, j].XCoordinate);
                    Assert.AreEqual(j, Map.hexMap[i, j].YCoordinate);
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
            //Tjeks if number of cells in array in each direction (x,y) is correct.
            Assert.AreEqual(x, Map.hexMap.GetLength(0));
            Assert.AreEqual(y, Map.hexMap.GetLength(1));
        }

        //Denne tester også at musen følger den rigtige rute ved at man kan se, at musen bliver flyttet hvergang til næste sted på pathen, da MouseYCoordinate og X er dannet ud fra den første hex i pathen.
        [TestCase(5, 5)]
        [TestCase(11, 11)]
        [TestCase(21, 21)]
        public void MousePositioner_CalculateRoutesWorks_ColorsAndDisablesRightFollowsRightPath(int x, int y)
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
            //Saves the current mouseposition before it gets overriden in the next call. 
            //They are going to be used to tjeck if the current mouseposition will be colored grey and enabled after next call.
            //Because MouseX and MouseY are equal the coordinates of the first hex in the path. 
            //In that way we kinda test that the mouse will move along the path. 
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
