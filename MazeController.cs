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
    class MazeController : GameObject
    {
        public List<GameObject> walls;
        public GameObject ground;
        private Maze maze;
        private int cellsize;

        // Constructor.
        public MazeController(GameController game)
        {
            this.game = game;
            this.walls = new List<GameObject>();
            this.ground = new Ground(game, 100, 100);
            cellsize = 10;
            Generate(10, 10);
        }


       public void Generate(int width, int height) {
           maze = new Maze(width, height);
           for (int row = 0; row < width; row++)
           {
               walls.Add(new Wall(game, new Vector3(row * cellsize, cellsize, 0),GameObjectType.EastWall));
               for (int col = 0; col < height; col++)
               {
                   if ((maze.grid[row, col] & Maze.S) == 0)
                   {
                       walls.Add(new Wall(game, new Vector3(row * cellsize, cellsize, col * cellsize), GameObjectType.SouthWall));
                   }
                   else
                   {

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