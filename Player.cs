using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    public class Player : GameObject
    {
        //private float speed = 0.006f;
        public Matrix View;
        public Matrix Projection;
        public Matrix World;
        public Vector3 pos;
        public Vector3 oldPos;


        public Player(GameController game)
        {
            this.game = game;
            type = GameObjectType.Player;
            pos = new SharpDX.Vector3(0, game.boundaryBottom + 0.5f, 0);

            //camera controller
            pos = new Vector3(0, 0, -10);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
            this.game = game;
        }

       

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (game.keyboardState.IsKeyDown(Keys.Space)) { }

            // TASK 1: Determine velocity based on accelerometer reading
            pos.X += (float)game.accelerometerReading.AccelerationX;

            // Keep within the boundaries.
            if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            basicEffect.World = Matrix.Translation(pos);

            //Camera update: Screen resize projection matrix changes
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
        }

 
    }
}