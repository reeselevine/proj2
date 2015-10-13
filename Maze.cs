using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{


    public class Maze
    {
        /** Directions are stored as powers of 2 (so that a cell can encode its
         * walls as a 4 bit number. ie 0110 means there are no South and East 
         * edges present in this cell */
        public const int N = 1, S = 2, E = 4, W = 8;

        //Helper mapper to access a cell +/- 1 across a vertical wall
        private Dictionary<int, int> DX;
        //Helper mapper to access a cell +/- 1 across a horizontal wall
        private Dictionary<int, int> DY;
        //Opposite directional mapping
        private Dictionary<int, int> OPPOSITE;

        private Random rng;

        /** An Edge is a wall that connects two cells. It belongs to a cell at
         * position x,y and is facing DIRECTION */
        struct Edge
        {
            public int x;
            public int y;
            public int direction;
        }

        //Other fields
        private List<Edge> edges;
        private int height;
        private int width;
        //Used to display and draw the map
        public int[,] grid;
        //Used to join sets of the maze
        public Tree[,] sets;

        /** Constructs a WIDTH by HEIGHT maze */
        public Maze(int width, int height)
        {
            DX = new Dictionary<int, int>()
            {
                {E, 1}, {W, -1}, {N, 0}, {S, 0}
            };
            DY = new Dictionary<int, int>()
            {
                {E, 0}, {W, 0}, {N, -1}, {S, 1}
            };
            OPPOSITE =
                 new Dictionary<int, int>()
                {
                    {E, W}, {W, E}, {N, S}, {S, N}
                };

            //Random number used for Fisher-Yates List shuffle
            rng = new Random();
            this.height = height;
            this.width = width;
            grid = new int[height, width];
            sets = new Tree[height, width];
            edges = new List<Edge>();
            populate();
            Shuffle<Edge>(edges);
            joinSet();
        }

        /** Get a cell and its neighbor via nEdge, if a cell and its neighbor
         * are not connected, then join them via Tree.connect. Encodes the path
         * via bitwise operation | on direction. */
        private void joinSet()
        {
            for (int i = 0; i < edges.Count; i++)
            {
                Edge nEdge = edges[i];
                int x = nEdge.x, y = nEdge.y;
                int direction = nEdge.direction;

                int nx = x + DX[direction];
                int ny = y + DY[direction];

                Tree set1 = sets[y, x];
                Tree set2 = sets[ny, nx];
                if (!set1.isConnected(set2))
                {
                    set1.connect(set2);
                    grid[y, x] = direction | grid[y, x];
                    grid[ny, nx] = OPPOSITE[direction] | grid[ny, nx];
                }
            }
        }



        /** populates this.grid with 0's
         *  populates this.sets with new Tree()
         *  populates this.edges with new edge */
        private void populate()
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    grid[h, w] = 0;
                    sets[h, w] = new Tree();
                    if (h > 0)
                    {
                        edges.Add(new Edge { x = w, y = h, direction = N });

                    }
                    if (w > 0)
                    {
                        edges.Add(new Edge { x = w, y = h, direction = W });
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

        public override string ToString()
        {
            string str = "";
            str += " ";
            for (int i = 0; i < width * 2 - 1; i++)
            {
                str += "_";
            }
            str += "\n";
            for (int row = 0; row < height; row++)
            {
                str += "|";
                for (int col = 0; col < width; col++)
                {
                    if ((grid[row, col] & S) != 0)
                    {
                        str += " ";
                    }
                    else
                    {
                        str += "_";
                    }

                    if ((grid[row, col] & E) != 0)
                    {
                        if (((grid[row, col] | grid[row, col + 1]) & S) != 0)
                        {
                            str += " ";
                        }
                        else
                        {
                            str += "_";
                        }
                    }
                    else
                    {
                        str += "|";
                    }
                }
                str += "\n";
            }
            return str;
        }
    }
}
