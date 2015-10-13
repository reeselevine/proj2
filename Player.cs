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
        private float Yaw;
        private float Pitch;
        private float scaleDown;

        public Player(GameController game)
        {
            this.game = game;
            type = GameObjectType.Player;
            Yaw = 0;
            scaleDown = .001f;
            //camera controller
            pos = new Vector3(0, 20, 0);
            currentTarget = new Vector3(30, 0, 30);
            View = Matrix.LookAtLH(pos, pos + currentTarget, Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
            this.game = game;
        }

       

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(game.accelerometerReading.AccelerationY);
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            // TASK 1: Determine velocity based on accelerometer reading
            //Tilt up and Down
            //float deltaX = (float) game.accelerometerReading.AccelerationX - prevX;
            //float deltaY = (float)game.accelerometerReading.AccelerationY - prevY;
            //Move Forward
            Vector3 temp;
            if (game.keyboardState.IsKeyDown(Keys.W)) 
            {
                temp = (currentTarget - pos);
                //temp.Normalize();
                Vector3 change = new Vector3(temp.X, 0, temp.Z);
                change.Normalize();
                pos += change;
                currentTarget += change;
            }

            Matrix translation = Matrix.RotationYawPitchRoll(Yaw, Pitch, 0);
            currentTarget = Vector3.TransformCoordinate(currentTarget, translation);

            //Camera update: Screen resize projection matrix changes
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, currentTarget, Vector3.UnitY);
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            float deltaX = -(float)args.Delta.Translation.X * scaleDown;
            Yaw = (float)(Math.PI * 2 * deltaX);
            float deltaY = (float)args.Delta.Translation.Y * scaleDown;
            Pitch = (float)(Math.PI * deltaY);
        }

        public override void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            Yaw = 0;
            Pitch = 0;
        }

        public override void Draw(GameTime gametime)
        {
            throw new NotImplementedException();
        }

    }
}