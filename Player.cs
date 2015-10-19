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
        private float yTarget;
        private float scaleDown;
        private float collisionError;
        private float deltaError;
        public float prevY;
        public int ghostEncounters;
        public Boolean invincible;
        private int invincibilityCounter;
        private int invincibilityTimer;
        public Player(GameController game, int numLives)
        {
            this.game = game;
            type = GameObjectType.Player;
            Yaw = 0;
            yTarget = 1;
            scaleDown = .001f;
            collisionError = .2f;
            deltaError = .05f;
            prevY = 0f;
            ghostEncounters = numLives;
            invincible = false;
            invincibilityCounter = 0;
            invincibilityTimer = 120;
            //camera controller
            pos = new Vector3(5, 1, 5);
            currentTarget = new Vector3(30, 1, 30);
            View = Matrix.LookAtLH(pos, currentTarget, Vector3.UnitY);
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
            if (invincible)
            {
                invincibilityCounter++;
                if (invincibilityCounter % invincibilityTimer == 0)
                {
                    invincible = false;
                    invincibilityCounter = 0;
                }
            }
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
            if (game.keyboardState.IsKeyDown(Keys.S) || deltaY < -deltaError)
            {
                temp = (currentTarget - pos);
                Vector3 change = new Vector3(temp.X, 0, temp.Z);
                change.Normalize();
                change /= 6;
                pos -= change;
                currentTarget -= change;
                if (CollisionDetected())
                {
                    pos += change;
                    currentTarget += change;
                }
            }
            currentTarget.Y = yTarget;
            Matrix translation = Matrix.RotationYawPitchRoll(Yaw, 0, 0);
            currentTarget = Vector3.TransformCoordinate(currentTarget, translation);
            //Camera update: Screen resize projection matrix changes
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 1000.0f);
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

        public Boolean IsEncountering(Ghost ghost)
        {
            if (pos.X > ghost.pos.X && pos.X < ghost.pos.X + ghost.size &&
                pos.Z > ghost.pos.Z && pos.Z < ghost.pos.Z + ghost.size)
            {
                return true;
            }
            return false;
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            float deltaX = -(float)args.Delta.Translation.X * scaleDown;
            Yaw = (float)(Math.PI * 2 * deltaX);
            float deltaY = (float)args.Delta.Translation.Y / 6;
            yTarget +=  deltaY;
            // Ugly, but makes sure that the player can always look up and down 45 degrees
            float distance = (float)Math.Sqrt(Math.Pow(currentTarget.X, 2) + Math.Pow(currentTarget.Z, 2));
            if (Math.Tan(Math.PI / 4) < (yTarget / distance))
            {
                yTarget = (float)(distance * Math.Tan(Math.PI / 4));
            }
            if (Math.Tan(-Math.PI / 4) > (yTarget / distance))
            {
                yTarget = (float)(distance * -Math.Tan(Math.PI / 4));
            }
        }

        public override void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            Yaw = 0;
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
                float winBoundary = game.mazeController.cellsize / 4;
                float x = pos.X % game.mazeController.cellsize;
                float z = pos.Z % game.mazeController.cellsize;
                if (x > winBoundary && x < winBoundary * 3 &&
                    z > winBoundary && z < winBoundary * 3)
                {
                    return true;
                }
            }
            return false;
        }

    }
}