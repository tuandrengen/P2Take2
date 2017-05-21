using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace P2SeriousGame
{
    /// <summary>
    /// Class for calculating a route out of a HexagonButton grid from Mad.cs.
    /// </summary>
	public class Pathfinding : IPathfinding
	{
		private List<HexagonButton> _queue = new List<HexagonButton>();
		private List<HexagonButton> _pathsToEdge = new List<HexagonButton>();
        private List<HexagonButton> _reachableHexList = new List<HexagonButton>();
		private Random rnd = new Random();
        public HexagonButton FirstButtonInPath;

        public static int gameTotalWins;
        public static int gameTotalLosses;
        public static bool gameRoundWin;

        GameForm game;

        public Pathfinding(GameForm game)
        {
            this.game = game;
        }

		/// <summary>
		/// Finds the shortest route to the edge of the map. If there are more routes at the same length, a route will randomly be chosen.
		/// </summary>
		/// <param name="hexMap">The map that the search will be performed on</param>
		/// <param name="startingHex">The field that the search should be performed from</param>
		/// <returns></returns>
        public HexagonButton FindPath(HexagonButton[,] hexMap, HexagonButton startingHex)
        {
            ResetAllButtons(hexMap);
            CalculateRoutes(hexMap, startingHex);
            return FirstButtonInPath;
        }

        private void CalculateRoutes(HexagonButton[,] hexMap, HexagonButton startingHex)
        {
            try
            {
                BreadthFirst breadthFirst = new BreadthFirst(_queue, _pathsToEdge, _reachableHexList);
                breadthFirst.CalculateRoutes(hexMap, startingHex);
                List<HexagonButton> routes = breadthFirst.FindTheRoutes();
                FirstButtonInPath = ChooseRouteByRand(routes).First();                
            }
            catch (GameWonException e)
            {
                Map.ResetMouse();
                game.WinNotification();
            }
            catch (LostTheGameException e)
            {
                Map.ResetMouse();
                game.LoseNotification();
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
            if ((currentHex.parent != null))
            {
                do
                {
                    routeByRand.Add(currentHex);
                    currentHex.BackColor = System.Drawing.Color.FromArgb(50, 205, 50);
                    currentHex = currentHex.parent;
                } while (currentHex.parent != null);
            }
            else
            {
                throw new LostTheGameException("You lost the game");
            }
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
