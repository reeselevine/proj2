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

        /** pos.y is actually the size of the cell!!! */
        public Wall(GameController game, Vector3 pos, GameObjectType wallType)
        {
            this.game = game;
            this.pos = pos;
            type = wallType;

            if (type == GameObjectType.EastWall)
            {
                vertices = makeEastWall(pos);
            }
            else
            {
                vertices = makeSouthWall(pos);
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

        private Buffer<VertexPositionColor> makeEastWall(Vector3 pos)
        {

        }

        private Buffer<VertexPositionColor> makeSouthWall(Vector3 pos)
        {

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
        }
    }
}

