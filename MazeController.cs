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

        // Constructor.
        public MazeController(GameController game)
        {
            this.game = game;
            this.walls = new List<GameObject>();
            this.ground = new Ground(game, 10, 10);
            Generate(10, 10);
        }


       public void Generate(int width, int height) {

       }

       public List<GameObject> get()
       {
           return new List<GameObject>();
       } 



        // Frame update method.
        public override void Update(GameTime gameTime)
        {
        }

        // Method for when the game ends. MOVE THIS
        private void gameOver()
        {
            game.Exit();
        }
    }
}