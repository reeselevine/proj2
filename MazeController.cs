using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
  
    
    // Enemy Controller class.
    public class MazeController : GameObject
    {
        public List<GameObject> walls;
        public GameObject ground;
        public Maze maze;
        public int cellsize;

        // Constructor.
        public MazeController(GameController game, int size)
        {
            this.game = game;
            cellsize = 10;
            this.walls = new List<GameObject>();
            this.ground = new Ground(game, size * cellsize, size * cellsize);
            Generate(size, size);
        }


       public void Generate(int width, int height) {
           maze = new Maze(width, height);
           for (int i = 0; i < width; i++)
            {
                walls.Add(new Wall(game, new Vector3(0, cellsize, i * cellsize), GameObjectType.SouthWall));
            }
           for (int row = 0; row < height; row++)
           {
               walls.Add(new Wall(game, new Vector3(row * cellsize, cellsize, 0),GameObjectType.EastWall));
               for (int col = 0; col < width; col++)
               {
                    if ((maze.grid[row, col] & Maze.S) == 0)
                    {
                        walls.Add(new Wall(game, new Vector3((row + 1) * cellsize, cellsize, col * cellsize), GameObjectType.SouthWall));
                    }
                   if ((maze.grid[row, col] & Maze.E) == 0)
                    {
                        walls.Add(new Wall(game, new Vector3(row * cellsize, cellsize, (col + 1) * cellsize), GameObjectType.EastWall));
                    }
               }
           }
       }

       public List<GameObject> get()
       {
           return new List<GameObject>();
       } 



        // Frame update method.
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        // Method for when the game ends. MOVE THIS
        private void gameOver()
        {
            game.Exit();
        }
    }
}