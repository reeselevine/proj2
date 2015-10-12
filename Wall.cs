using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX;
namespace Project
{
    // Wall class
    using SharpDX.Toolkit.Graphics;
    class Wall : GameObject
    {
        private Buffer<VertexPositionColor> vertices;
        private static float height = 5;
        private float cellsize;

        /** pos.y is actually the size of the cell!!! */
        public Wall(GameController game, Vector3 pos, GameObjectType wallType)
        {
            this.game = game;
            this.pos = pos;
            cellsize = pos.Y;
            type = wallType;

            if (type == GameObjectType.EastWall)
            {
                makeEastWall();
            }
            else
            {
                makeSouthWall();
            }

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.player.View,
                Projection = game.player.Projection,
                World = game.player.World,
                VertexColorEnabled = true
            };
        }

        private void makeEastWall()
        {
            Color color = Color.Plum;
            float northSide = pos.X;
            float southSide = pos.X + cellsize;
            float westSide = pos.Z - cellsize / 4;
            float eastSide = pos.Z + cellsize / 4;
            vertices = Buffer.Vertex.New(game.GraphicsDevice,
                new[] {
                    // west side
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    // east side
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    // top
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    // north side
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    // south side
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color)
                });
        }

        private void makeSouthWall()
        {
            Color color = Color.Aqua;
            float northSide = pos.X - cellsize / 4;
            float southSide = pos.X + cellsize / 4;
            float westSide = pos.Z;
            float eastSide = pos.Z + cellsize;
            vertices = Buffer.Vertex.New(game.GraphicsDevice,
                new[] {
                    // north side
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    // south side
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color),
                    // east side
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, eastSide), color),
                    // west side
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(southSide, 0, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, 0, westSide), color),
                    // top
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, eastSide), color),
                    new VertexPositionColor(new Vector3(southSide, height, westSide), color),
                    new VertexPositionColor(new Vector3(northSide, height, westSide), color)
        });
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

