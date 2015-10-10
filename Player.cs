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
        public Vector3 oldPos, currentTarget;
        private float prevX;
        private float prevY;
        private float speed;

        public Player(GameController game)
        {
            this.game = game;
            type = GameObjectType.Player;
            speed = 1;
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
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            // TASK 1: Determine velocity based on accelerometer reading
            //Tilt up and Down
            pos.X += (float)game.accelerometerReading.AccelerationX;
            pos.Y += (float)game.accelerometerReading.AccelerationY;
            float deltaX = (float) game.accelerometerReading.AccelerationX - prevX;
            float deltaY = (float)game.accelerometerReading.AccelerationY - prevY;
            //Move Forward
            if (game.keyboardState.IsKeyDown(Keys.W)) 
            {
                pos += speed * currentTarget * time;
            }
            basicEffect.World = Matrix.Translation(pos);

            float Yaw = ((float)Math.PI * 2 * deltaX);
            prevX = (float)game.accelerometerReading.AccelerationX;

            float Pitch = ((float)Math.PI * 2 * deltaY);
            prevY = (float)game.accelerometerReading.AccelerationY;

            Matrix translation = Matrix.RotationYawPitchRoll(Yaw, Pitch, 0);
            currentTarget = Vector3.TransformCoordinate(currentTarget, translation);

            //Camera update: Screen resize projection matrix changes
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, pos+currentTarget, Vector3.UnitY);
        }

        public override void Draw(GameTime gametime)
        {
            throw new NotImplementedException();
        }

    }
}