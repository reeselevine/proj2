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

        // Constructor.
        public MazeController(GameController game)
        {
            this.game = game;
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