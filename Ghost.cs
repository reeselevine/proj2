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
    public class Ghost : GameObject
    {
        private Buffer<VertexPositionColor> vertices;
        public float size;
        private float height;
        private Color color;
        private int seek;
        private int delay;
        private float speed;
        private Vector3 direction;
        public Ghost(GameController game)
        {
            this.game = game;
            color = new Color(new Vector3(124, 0, 0), 0.05f);
            size = 2;
            height = 0.5f;
            seek = 0;
            delay = 240;
            Spawn();
            speed = .05f;
            direction = new Vector3(game.player.pos.X - pos.X, 0, game.player.pos.Z - pos.Z);
            UpdateVertices();

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.player.View,
                Projection = game.player.Projection,
                World = game.player.World,
                VertexColorEnabled = true
            };
        }

        // Spawns a ghost somewhere in the boundaries of the game map
        private void Spawn()
        {
            pos = new Vector3(game.random.NextFloat(0, game.size * game.mazeController.cellsize),
                1, game.random.NextFloat(0, game.size * game.mazeController.cellsize));
        }

        public override void Update(GameTime gameTime)
        {
            // changes the direction of the ghost on some updates
            if (seek == 0)
            {
                direction.X = game.player.pos.X - pos.X;
                direction.Z = game.player.pos.Z - pos.Z;
                direction.Normalize();
            }
            pos += speed * direction;
            UpdateVertices();
            basicEffect.World = game.player.World;
            basicEffect.View = game.player.View;
            basicEffect.Projection = game.player.Projection;
            seek = (seek + 1) % delay;
        }

        public override void Draw(GameTime gametime)
        {
            game.GraphicsDevice.SetBlendState(game.GraphicsDevice.BlendStates.AlphaBlend);
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }

        private void UpdateVertices()
        {
            Vector3 frontBottomLeft = new Vector3(pos.X, height, pos.Z);
            Vector3 frontTopLeft = new Vector3(pos.X, height + size, pos.Z);
            Vector3 frontTopRight = new Vector3(pos.X + size, height + size, pos.Z);
            Vector3 frontBottomRight = new Vector3(pos.X + size, height, pos.Z);
            Vector3 backBottomLeft = new Vector3(pos.X, height, pos.Z + size);
            Vector3 backBottomRight = new Vector3(pos.X + size, height, pos.Z + size);
            Vector3 backTopLeft = new Vector3(pos.X, height + size, pos.Z + size);
            Vector3 backTopRight = new Vector3(pos.X + size, height + size, pos.Z + size);

            vertices = Buffer.Vertex.New(game.GraphicsDevice, new[]
            {
            new VertexPositionColor(frontBottomLeft, color), // Front
            new VertexPositionColor(frontTopLeft, color),
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
        }
    }
}
