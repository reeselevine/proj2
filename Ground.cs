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
        private Buffer<VertexPositionNormalColor> vertices;

        public Ground(GameController game, int width, int height)
        {
            this.game = game;
            Color color = Color.Black;
            Vector3 normal = new Vector3(0, 1, 0);
            vertices = Buffer.Vertex.New(game.GraphicsDevice, new[] 
            {
                new VertexPositionNormalColor(new Vector3(0,0,0), normal, color),
                new VertexPositionNormalColor(new Vector3(0,0,height), normal, color),
                new VertexPositionNormalColor(new Vector3(width,0,height), normal, color),
                new VertexPositionNormalColor(new Vector3(0,0,0), normal, color),
                new VertexPositionNormalColor(new Vector3(width, 0, height), normal, color),
                new VertexPositionNormalColor(new Vector3(width, 0, 0), normal, color)
            });
            effect = game.Content.Load<Effect>("Gouraud");
            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
        }

        public override void Update(GameTime gameTime)
        {
            effect.Parameters["Projection"].SetValue(game.player.Projection);
            effect.Parameters["World"].SetValue(game.player.World);
            effect.Parameters["View"].SetValue(game.player.View);
            effect.Parameters["cameraPos"].SetValue(game.player.pos);
            Matrix WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(game.player.World));
            effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);
        }

        public override void Draw(GameTime gametime)
        {
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
