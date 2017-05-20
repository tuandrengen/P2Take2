using System.Collections.Generic;
using System.Linq;

namespace P2SeriousGame
{
    public class BreadthFirst
    {
        public List<HexagonButton> _queue = new List<HexagonButton>();
        public List<HexagonButton> _reachableEdgeTiles = new List<HexagonButton>();
        public List<HexagonButton> _reachableHexList = new List<HexagonButton>();

        public BreadthFirst(List<HexagonButton> queue, List<HexagonButton> pathsToEdge, List<HexagonButton> reachableHexList)
        {
            this._queue = queue;
            this._reachableEdgeTiles = pathsToEdge;
            this._reachableHexList = reachableHexList;
        }
        
        /// <summary>
        /// Takes a HexagonButton grid, and the posistion to start from.
        /// </summary>
        /// <param name="hexMap"></param>
        /// <param name="startingHex"></param>
		public void CalculateRoutes(HexagonButton[,] hexMap, HexagonButton startingHex)
        {
            _reachableEdgeTiles.Clear();
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
                    _reachableEdgeTiles.Add(currentHex);
                }
            }
        }

        public List<HexagonButton> FindTheRoutes()
        {
            var bestRoutes = new List<HexagonButton>();

            /// If at least one route can be found, there's is a route to an edge
            if (_reachableEdgeTiles.Count > 0)
            {
                bestRoutes = FindShortestRoutes();
            }
            /// If there's no routes to the edge, but there's still other reachable hexes, 
            /// the mouse is trapped, but not enclosed yet.
            else if (_reachableHexList.Count > 0)
            {
                bestRoutes = FindLongestRoutes();
            }
            else
            {
                /// You won the game! 
                Pathfinding.gameTotalWins += 1;
                Pathfinding.gameRoundWin = true;
                throw new GameWonException("You won the game");
            }
            ///List<HexagonButton> bestRouteByRand = ChooseRouteByRand(bestRoutes);
            ///return bestRouteByRand.First();
            return bestRoutes;
        }
        
        /// Reachable hexes that are not edges of the map. Used for finding the longest route when mouse is trapped
        public List<HexagonButton> FindLongestRoutes()
        {
            var longestRoutes = new List<HexagonButton>();
            
            foreach (HexagonButton hex in _reachableHexList)
            {
                hex.CostToStart = CheckParent(hex);

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
		public List<HexagonButton> FindShortestRoutes()
        {
            /// Input parametren er edgehexes.
            var shortestRoutes = new List<HexagonButton>();
            foreach (HexagonButton hex in _reachableEdgeTiles)
            {
                hex.CostToStart = CheckParent(hex);

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
		public int CheckParent(HexagonButton hexToCheck, int count = 0)
        {
            if (hexToCheck.parent == null)
            {
                return count;
            }
            else
            {
                return CheckParent (hexToCheck.parent, count + 1);
            }
        }
    }
}
