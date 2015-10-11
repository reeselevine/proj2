using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    // Ground (not a wall) class
    using SharpDX.Toolkit.Graphics;
    class Ground : GameObject
    {
        private Buffer<VertexPositionColor> vertices;

        public Ground(GameController game, int width, int height)
        {
            this.game = game;
            Color color = Color.Black;
            vertices = Buffer.Vertex.New(game.GraphicsDevice, new[] 
            {
                new VertexPositionColor(new Vector3(0,0,0), color),
                new VertexPositionColor(new Vector3(0,0,height), color),
                new VertexPositionColor(new Vector3(width,0,height), color),
                new VertexPositionColor(new Vector3(0,0,0), color),
                new VertexPositionColor(new Vector3(width, 0, height), color),
                new VertexPositionColor(new Vector3(width, 0, 0), color)
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
