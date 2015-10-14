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
        public Matrix View;
        public Matrix Projection;
        public Matrix World;
        public Vector3 oldPos, currentTarget;
        private float Yaw;
        private float Pitch;
        private float scaleDown;
        private float collisionError;
        private float prevY;
        private float deltaError;

        public Player(GameController game)
        {
            this.game = game;
            type = GameObjectType.Player;
            Yaw = 0;
            scaleDown = .001f;
            collisionError = .2f;
            deltaError = .01f;
            prevY = 0f;
            //camera controller
            pos = new Vector3(5, 1, 5);
            currentTarget = new Vector3(30, 0, 30);
            View = Matrix.LookAtLH(pos, pos + currentTarget, Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
            this.game = game;
        }

       

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            if (prevY == 0)
            {
                prevY = (float)game.accelerometerReading.AccelerationY;
            }
            // TASK 1: Determine velocity based on accelerometer reading
            //Tilt up and Down
            //float deltaX = (float) game.accelerometerReading.AccelerationX - prevX;
            float deltaY = (float)game.accelerometerReading.AccelerationY - prevY;
            //Move Forward
            Vector3 temp;
            if (game.keyboardState.IsKeyDown(Keys.W) || deltaY > deltaError) 
            {
                temp = (currentTarget - pos);
                Vector3 change = new Vector3(temp.X, 0, temp.Z);
                change.Normalize();
                change /= 6;
                pos += change;
                currentTarget += change;
                if (CollisionDetected())
                {
                    pos -= change;
                    currentTarget -= change;
                }
            }

            Matrix translation = Matrix.RotationYawPitchRoll(Yaw, 0, 0);
            currentTarget = Vector3.TransformCoordinate(currentTarget, translation);

            //Camera update: Screen resize projection matrix changes
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(pos, currentTarget, Vector3.UnitY);
        }

        private Boolean CollisionDetected()
        {
            if (pos.X <= game.boundaryNorth || pos.X >= game.boundarySouth ||
                pos.Z <= game.boundaryWest || pos.Z >= game.boundaryEast)
            {
                return true;
            }
            float collisionRadius = (float)game.mazeController.cellsize / 4f + collisionError;
            int row = (int)Math.Floor(pos.X / game.mazeController.cellsize);
            int col = (int)Math.Floor(pos.Z / game.mazeController.cellsize);
            float x = pos.X % game.mazeController.cellsize;
            float z = pos.Z % game.mazeController.cellsize;
            if (x >= (3 * collisionRadius - 4 * collisionError) &&
                (game.mazeController.maze.grid[row, col] & Maze.S) == 0)
            {
                return true;
            }
            else if (x <= collisionRadius && row != 0 &&
                (game.mazeController.maze.grid[row - 1, col] & Maze.S) == 0)
            {
                return true;
            }
            else if (z <= collisionRadius && col != 0 &&
                (game.mazeController.maze.grid[row, col - 1] & Maze.E) == 0)
            {
                return true;
            }
            else if (z >= (3 * collisionRadius - 4 * collisionError) &&
                (game.mazeController.maze.grid[row, col] & Maze.E) == 0)
            {
                return true;
            }
            return false;
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

        public Boolean HasWon()
        {
            int row = (int)Math.Floor(pos.X / game.mazeController.cellsize);
            int col = (int)Math.Floor(pos.Z / game.mazeController.cellsize);
            if (row == game.size - 1 && col == game.size - 1)
            {
                return true;
            }
            return false;
        }

    }
}