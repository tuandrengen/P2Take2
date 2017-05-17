using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2SeriousGame
{
    public class Pathfinding : IPathfinding
    {
        private List<HexagonButton> _queue = new List<HexagonButton>();
        private List<HexagonButton> _pathsToEdge = new List<HexagonButton>();
        private List<HexagonButton> _reachableHexList = new List<HexagonButton>();
        private Random rnd = new Random();
        public HexagonButton FirstButtonInPath;

        /// <summary>
        /// Takes a HexagonButton grid, and the posistion to start from.
        /// </summary>
        /// <param name="hexMap"></param>
        /// <param name="startingHex"></param>
		public HexagonButton CalculateRoutes(HexagonButton[,] hexMap, HexagonButton startingHex)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ResetAllButtons(hexMap);
            _pathsToEdge.Clear();
            _reachableHexList.Clear();
            _queue.Add(startingHex);

            while (_queue.Any())
            {
                HexagonButton currentHex = _queue.First();
                _queue.Remove(_queue.First());
                currentHex.Visited = true;
                if (currentHex.IsEdgeTile == false)
                {
                    foreach (HexagonButton hex in currentHex.neighbourList)
                    {
                        if (hex.Visited == false && hex.Passable == true)
                        {
                            hex.parent = currentHex;
                            _queue.Add(hex);
                            hex.Visited = true;
                            _reachableHexList.Add(hex);
                        }
                    }
                }

                else
                {
                    _pathsToEdge.Add(currentHex);
                }
            }

            //FirstButtonInPath = FindTheRoute(_pathsToEdge, _reachableHexList);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = ts.TotalMilliseconds.ToString();
            Console.WriteLine("RunTime " + elapsedTime);

            return FindTheRoute(_pathsToEdge, _reachableHexList);
        }

        public static int gameTotalWins;
        public static bool gameRoundWin;

        public HexagonButton FindTheRoute(List<HexagonButton> pathsToEdge, List<HexagonButton> reachableHexList)
        {
            var bestRoutes = new List<HexagonButton>();

            //If at least one route can be found, there's is a route to an edge
            if (pathsToEdge.Count > 0)
            {
                bestRoutes = FindShortestRoutes(pathsToEdge);
            }
            //If there's no routes to the edge, but there's still other reachable hexes, the mouse is trapped, but not enclosed yet
            else if (reachableHexList.Count > 0)
            {
                bestRoutes = FindLongestRoutes(reachableHexList);
            }
            else
            {
                //You Won :)
                gameTotalWins += 1;
                gameRoundWin = true;
                throw new NotImplementedException();
            }
            try
            {
                List<HexagonButton> bestRouteByRand = ChooseRouteByRand(bestRoutes);
                return bestRouteByRand.First();
            }
            catch (NullReferenceException)
            {
                throw new EndOfMapException("Mouse reached end of map");
            }
        }

        //Reachable hexes that are not edges of the map. Used for finding the longest route when mouse is trapped
        private List<HexagonButton> FindLongestRoutes(List<HexagonButton> reachableHexList)
        {
            var longestRoutes = new List<HexagonButton>();
            foreach (HexagonButton hex in reachableHexList)
            {
                hex.CostToStart = CheckParent(0, hex);

                if (longestRoutes.Count == 0)
                    longestRoutes.Add(hex);
                else if (longestRoutes.First().CostToStart < hex.CostToStart)
                {
                    longestRoutes.Clear();
                    longestRoutes.Add(hex);
                }
                else if (longestRoutes.First().CostToStart == hex.CostToStart)
                    longestRoutes.Add(hex);
            }
            return longestRoutes;
        }


        /// <summary>
        /// Compares the different routes, and returns the shortest.
        /// </summary>
        /// <param name="edgeHexList"></param>
        /// <returns></returns>
		private List<HexagonButton> FindShortestRoutes(List<HexagonButton> edgeHexList)
        {
            var shortestRoutes = new List<HexagonButton>();
            foreach (HexagonButton hex in edgeHexList)
            {
                hex.CostToStart = CheckParent(0, hex);

                if (shortestRoutes.Count == 0)
                    shortestRoutes.Add(hex);
                else if (shortestRoutes.First().CostToStart > hex.CostToStart)
                {
                    shortestRoutes.Clear();
                    shortestRoutes.Add(hex);
                }
                else if (shortestRoutes.First().CostToStart == hex.CostToStart)
                    shortestRoutes.Add(hex);
            }
            return shortestRoutes;
        }

        /// <summary>
        /// Finds the cost of a route.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="hexToCheck"></param>
        /// <returns></returns>
		private int CheckParent(int count, HexagonButton hexToCheck)
        {
            if (hexToCheck.parent == null)
            {
                return count;
            }
            else
            {
                return CheckParent(count + 1, hexToCheck.parent);
            }
        }

        /// <summary>
        /// If there is multiple routes with the same cost, choose a random one.
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
		private List<HexagonButton> ChooseRouteByRand(List<HexagonButton> routes)
        {
            var routeByRand = new List<HexagonButton>();
            int routeToChoose = rnd.Next(0, routes.Count);

            HexagonButton edgeHex = routes.ElementAt(routeToChoose);
            HexagonButton currentHex = edgeHex;

            do
            {
                routeByRand.Add(currentHex);
                currentHex.BackColor = System.Drawing.Color.FromArgb(50, 205, 50);
                currentHex = currentHex.parent;
            } while (currentHex.parent != null);

            routeByRand.Reverse();

            return routeByRand;
        }


        /// <summary>
        /// Resets all HexagonButtons in regards to parent and visited.
        /// </summary>
        /// <param name="hexMap"></param>
		private void ResetAllButtons(HexagonButton[,] hexMap)
        {
            foreach (HexagonButton hex in hexMap)
            {
                hex.Visited = false;
                hex.parent = null;
                if (hex.BackColor == System.Drawing.Color.FromArgb(50, 205, 50))
                {
                    hex.BackColor = System.Drawing.Color.LightGray;
                }
            }
        }
    }
}
