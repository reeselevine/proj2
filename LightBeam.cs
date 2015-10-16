using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public class LightBeam : GameObject
    {
        private Buffer<VertexPositionColor> vertices;

        public LightBeam(GameController game, float width, int height)
        {
            this.game = game;
           
            float xPos = (game.size - 1) * game.mazeController.cellsize;
            float zPos = (game.size - 1) * game.mazeController.cellsize;
            float offset = width;

            Color color = Color.LightYellow;
            Vector3 frontBottomLeft = new Vector3(xPos - offset, -1.0f, zPos - offset);
            Vector3 frontTopLeft = new Vector3(xPos - offset, height, zPos - offset);
            Vector3 frontTopRight = new Vector3(xPos + offset, height, zPos - offset);
            Vector3 frontBottomRight = new Vector3(xPos + offset, -1.0f, zPos - offset);
            Vector3 backBottomLeft = new Vector3(xPos - offset, -1.0f, zPos + offset);
            Vector3 backBottomRight = new Vector3(xPos + offset, -1.0f, zPos + offset);
            Vector3 backTopLeft = new Vector3(xPos - offset, height, zPos + offset);
            Vector3 backTopRight = new Vector3(xPos + offset, height, zPos + offset);
            
            vertices = Buffer.Vertex.New(game.GraphicsDevice, new[] 
            {
            new VertexPositionColor(frontBottomLeft, Color.Orange), // Front
            new VertexPositionColor(frontTopLeft, Color.Orange),
            new VertexPositionColor(frontTopRight, Color.Orange),
            new VertexPositionColor(frontBottomLeft, Color.Orange),
            new VertexPositionColor(frontTopRight, Color.Orange),
            new VertexPositionColor(frontBottomRight, Color.Orange),
            new VertexPositionColor(backBottomLeft, Color.Orange), // BACK
            new VertexPositionColor(backTopRight, Color.Orange),
            new VertexPositionColor(backTopLeft, Color.Orange),
            new VertexPositionColor(backBottomLeft, Color.Orange),
            new VertexPositionColor(backBottomRight, Color.Orange),
            new VertexPositionColor(backTopRight, Color.Orange),
            new VertexPositionColor(frontTopLeft, Color.OrangeRed), // Top
            new VertexPositionColor(backTopLeft, Color.OrangeRed),
            new VertexPositionColor(backTopRight, Color.OrangeRed),
            new VertexPositionColor(frontTopLeft, Color.OrangeRed),
            new VertexPositionColor(backTopRight, Color.OrangeRed),
            new VertexPositionColor(frontTopRight, Color.OrangeRed),
            new VertexPositionColor(frontBottomLeft, Color.OrangeRed), // Bottom
            new VertexPositionColor(backBottomRight, Color.OrangeRed),
            new VertexPositionColor(backBottomLeft, Color.OrangeRed),
            new VertexPositionColor(frontBottomLeft, Color.OrangeRed),
            new VertexPositionColor(frontBottomRight, Color.OrangeRed),
            new VertexPositionColor(backBottomRight, Color.OrangeRed),
            new VertexPositionColor(frontBottomLeft, Color.DarkOrange), // Left
            new VertexPositionColor(backBottomLeft, Color.DarkOrange),
            new VertexPositionColor(backTopLeft, Color.DarkOrange),
            new VertexPositionColor(frontBottomLeft, Color.DarkOrange),
            new VertexPositionColor(backTopLeft, Color.DarkOrange),
            new VertexPositionColor(frontTopLeft, Color.DarkOrange),
            new VertexPositionColor(frontBottomRight, Color.DarkOrange), // Right
            new VertexPositionColor(backTopRight, Color.DarkOrange),
            new VertexPositionColor(backBottomRight, Color.DarkOrange),
            new VertexPositionColor(frontBottomRight, Color.DarkOrange),
            new VertexPositionColor(frontTopRight, Color.DarkOrange),
            new VertexPositionColor(backTopRight, Color.DarkOrange),
                });

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.player.View,
                Projection = game.player.Projection,
                World = game.player.World,
                VertexColorEnabled = true
            };
        }

        public override void Update(GameTime gameTime)
        {
            basicEffect.World = game.player.World;
            basicEffect.View = game.player.View;
            basicEffect.Projection = game.player.Projection;
        }

        public override void Draw(GameTime gametime)
        {
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
