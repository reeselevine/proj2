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
            Color color = new Color(new Vector3(124, 124, 0), 0.1f);
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
            new VertexPositionColor(frontBottomLeft, color), // Front
            new VertexPositionColor(frontTopLeft,color),
            new VertexPositionColor(frontTopRight, color),
            new VertexPositionColor(frontBottomLeft, color),
            new VertexPositionColor(frontTopRight, color),
            new VertexPositionColor(frontBottomRight, color),
            new VertexPositionColor(backBottomLeft, color), // BACK
            new VertexPositionColor(backTopRight, color),
            new VertexPositionColor(backTopLeft, color),
            new VertexPositionColor(backBottomLeft, color),
            new VertexPositionColor(backBottomRight, color),
            new VertexPositionColor(backTopRight, color),
            new VertexPositionColor(frontTopLeft, color), // Top
            new VertexPositionColor(backTopLeft, color),
            new VertexPositionColor(backTopRight, color),
            new VertexPositionColor(frontTopLeft, color),
            new VertexPositionColor(backTopRight, color),
            new VertexPositionColor(frontTopRight, color),
            new VertexPositionColor(frontBottomLeft, color), // Bottom
            new VertexPositionColor(backBottomRight, color),
            new VertexPositionColor(backBottomLeft, color),
            new VertexPositionColor(frontBottomLeft, color),
            new VertexPositionColor(frontBottomRight, color),
            new VertexPositionColor(backBottomRight, color),
            new VertexPositionColor(frontBottomLeft, color), // Left
            new VertexPositionColor(backBottomLeft, color),
            new VertexPositionColor(backTopLeft, color),
            new VertexPositionColor(frontBottomLeft, color),
            new VertexPositionColor(backTopLeft, color),
            new VertexPositionColor(frontTopLeft, color),
            new VertexPositionColor(frontBottomRight, color), // Right
            new VertexPositionColor(backTopRight, color),
            new VertexPositionColor(backBottomRight, color),
            new VertexPositionColor(frontBottomRight, color),
            new VertexPositionColor(frontTopRight, color),
            new VertexPositionColor(backTopRight, color),
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
