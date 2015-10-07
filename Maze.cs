using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
   

    class Maze
    {
        //fields
        public Dictionary<string, int> DIRECTION;
        private Dictionary<string, int> DX;
        private Dictionary<string, int> DY;
        private Dictionary<string, string> OPPOSITE;

        private Random rng;

        private class Edge
        {
            public int x;
            public int y;
            public string direction;

           public Edge(int x,int y,string direction) 
           {
               this.x = x;
               this.y = y;
               this.direction = direction;
           }

        }

        private List<Edge> edges;
        private int height;
        private int width;
        public int[,] grid;
        public Tree[,] sets;

        public Maze() 
        {

            DIRECTION =
                new Dictionary<string, int>() 
                {
                    {"N", 1}, {"S",2}, {"E", 4}, {"W", 8}
                };
            DX = new Dictionary<string, int>()
            {
                {"E", 1}, {"W", -1}, {"N", 0}, {"S", 0}
            };
            DY = new Dictionary<string, int>()
            {
                {"E", 0}, {"W", 0}, {"N", -1}, {"S", 1}
            };
           OPPOSITE =
                new Dictionary<string, string>()
                {
                    {"E", "W"}, {"W", "E"}, {"N", "S"}, {"S", "N"}
                };
           rng = new Random();
           height = 10;
           width = 10;
           grid = new int[height,width];
           sets = new Tree[height,width];
           edges = new List<Edge>();
           populateGrid();
           populateSets();
           populateEdges();
           Shuffle<Edge>(edges);

           for (int i = 0; i < edges.Count; i++)
           {
               Edge nEdge = edges[i];
               int x = nEdge.x, y = nEdge.y;
               string direction = nEdge.direction;

               int nx = x + DX[direction];
               int ny = y + DY[direction];

               Tree set1 = sets[y,x], set2 = sets[ny,nx];

               if (!set1.isConnected(set2))
               {
                   set1.connect(set2);
                   grid[y, x] = DIRECTION[direction];
                   grid[ny,nx] = DIRECTION[OPPOSITE[direction]];
               }
           }
        }

       


        //populates a Grid with 0's            
       private void populateGrid() {
            for (int h = 0; h < height; h++)
			{
			    for (int w = 0; w < width; w++)
			    {
			        grid[h,w] = 0;
			    }
			}
        }

        //populates sets with empty trees, all sets are disjoint
        private void populateSets() 
        {
            for (int h = 0; h < height; h++)
			{
			    for (int w = 0; w < width; w++)
			    {
			        sets[h,w] = new Tree();
			    }
			}
        }

        private void populateEdges()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if (h > 0)
                    {
                        edges.Add(new Edge(w,h,"N"));

                    }
                    if (w > 0)
                    {
                        edges.Add(new Edge(w, h, "W"));
                    }
                }
            }
        }

        //Fisher Yates Shuffle
        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
