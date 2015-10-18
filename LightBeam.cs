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
            float adjust = game.mazeController.cellsize / 3;
            Color color = Color.LightYellow;
            Vector3 frontBottomLeft = new Vector3(xPos + adjust, -1.0f, zPos + adjust);
            Vector3 frontTopLeft = new Vector3(xPos + adjust, height, zPos + adjust);
            Vector3 frontTopRight = new Vector3(xPos + offset - adjust, height, zPos + adjust);
            Vector3 frontBottomRight = new Vector3(xPos + offset - adjust, -1.0f, zPos + adjust);
            Vector3 backBottomLeft = new Vector3(xPos + adjust, -1.0f, zPos + offset - adjust);
            Vector3 backBottomRight = new Vector3(xPos + offset - adjust, -1.0f, zPos + offset - adjust);
            Vector3 backTopLeft = new Vector3(xPos + adjust, height, zPos + offset - adjust);
            Vector3 backTopRight = new Vector3(xPos + offset - adjust, height, zPos + offset - adjust);
            
            vertices = Buffer.Vertex.New(game.GraphicsDevice, new[] 
            {
            new VertexPositionColor(frontBottomLeft, Color.Orange), // Front
            new VertexPositionColor(frontTopLeft, Color.Orange),
            new VertexPositionColor(frontTopRight, Color.Orange),
            new VertexPositionColor(frontBottomLeft, Color.Orange),
            new VertexPositionColor(frontTopRight, Color.Orange),
            new VertexPositionColor(frontBottomRight, Color.Orange),
            new VertexPositionColor(backBottomLeft, Color.Blue), // BACK
            new VertexPositionColor(backTopRight, Color.Blue),
            new VertexPositionColor(backTopLeft, Color.Blue),
            new VertexPositionColor(backBottomLeft, Color.Blue),
            new VertexPositionColor(backBottomRight, Color.Blue),
            new VertexPositionColor(backTopRight, Color.Blue),
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
            new VertexPositionColor(frontBottomLeft, Color.Green), // Left
            new VertexPositionColor(backBottomLeft, Color.Green),
            new VertexPositionColor(backTopLeft, Color.Green),
            new VertexPositionColor(frontBottomLeft, Color.Green),
            new VertexPositionColor(backTopLeft, Color.Green),
            new VertexPositionColor(frontTopLeft, Color.Green),
            new VertexPositionColor(frontBottomRight, Color.Yellow), // Right
            new VertexPositionColor(backTopRight, Color.Yellow),
            new VertexPositionColor(backBottomRight, Color.Yellow),
            new VertexPositionColor(frontBottomRight, Color.Yellow),
            new VertexPositionColor(frontTopRight, Color.Yellow),
            new VertexPositionColor(backTopRight, Color.Yellow),
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
