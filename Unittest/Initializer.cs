using System;
using System.Collections.Generic;
using P2SeriousGame;
using System.Windows.Forms;

namespace Unittest
{
    public class Initializer
    {
        //Reused variables
        public List<HexagonButton> queue = new List<HexagonButton>();
        public List<HexagonButton> pathsToEdge = new List<HexagonButton>();
        public List<HexagonButton> reachableHexList = new List<HexagonButton>();
        public GameForm window;
        public IPathfinding pathfinding;
        public Map map;
        public BreadthFirst bfs;
        public MouseButtons mouseBtn;
        public MouseEventArgs mouseArg;

        //Initializers
        public void InitializeMap(int x)
        {
            window = new GameForm(x);
            pathfinding = new Pathfinding(window);
            map = new Map(window, x, pathfinding);
        }
        public void InitializeBFS()
        {
            List<HexagonButton> queue = new List<HexagonButton>();
            List<HexagonButton> pathsToEdge = new List<HexagonButton>();
            List<HexagonButton> reachableHexList = new List<HexagonButton>();
            bfs = new BreadthFirst(queue, pathsToEdge, reachableHexList);
        }

        public void InitializeMouseEventArgs()
        {
            mouseBtn = new MouseButtons();
            mouseArg = new MouseEventArgs(mouseBtn, 0, 0, 0, 0);
        }
    }
}
