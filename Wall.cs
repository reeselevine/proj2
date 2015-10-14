﻿using System;
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
        private Buffer<VertexPositionNormalColor> vertices;
        private static float height = 5;
        private float cellsize;
        //Vertex Normals for Walls
        Vector3 BOTTOM_NORTHWEST_NORMAL = new Vector3(-0.333f, -0.333f, -0.333f);
        Vector3 TOP_NORTHWEST_NORMAL = new Vector3(-0.333f, 0.333f, -0.333f);
        Vector3 TOP_SOUTHWEST_NORMAL = new Vector3(0.333f, 0.333f, -0.333f);
        Vector3 BOTTOM_SOUTHWEST_NORMAL = new Vector3(0.333f, -0.333f, -0.333f);
        Vector3 BOTTOM_NORTHEAST_NORMAL = new Vector3(-0.333f, -0.333f, 0.333f);
        Vector3 BOTTOM_SOUTHEAST_NORMAL = new Vector3(0.333f, -0.333f, 0.333f);
        Vector3 TOP_NORTHEAST_NORMAL = new Vector3(-0.333f, 0.333f, 0.333f);
        Vector3 TOP_SOUTHEAST_NORMAL = new Vector3(0.333f, 0.333f, 0.333f);

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
            effect = game.Content.Load<Effect>("Phong");
            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                View = game.player.View,
                Projection = game.player.Projection,
                World = game.player.World,
                VertexColorEnabled = true,
                LightingEnabled = false
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
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    // east side
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, eastSide), BOTTOM_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    // top
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    // north side
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    // south side
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, eastSide), BOTTOM_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color)
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
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    // south side
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, eastSide), BOTTOM_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color),
                    // east side
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, eastSide), BOTTOM_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, eastSide), BOTTOM_NORTHEAST_NORMAL, color),
                    // west side
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, 0, westSide), BOTTOM_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, 0, westSide), BOTTOM_NORTHWEST_NORMAL, color),
                    // top
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, eastSide), TOP_NORTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, eastSide), TOP_SOUTHEAST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(southSide, height, westSide), TOP_SOUTHWEST_NORMAL, color),
                    new VertexPositionNormalColor(new Vector3(northSide, height, westSide), TOP_NORTHWEST_NORMAL, color)
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
            // Setup the effect parameters
            effect.Parameters["World"].SetValue(game.player.World);
            effect.Parameters["Projection"].SetValue(game.player.Projection);
            effect.Parameters["View"].SetValue(game.player.View);
            effect.Parameters["cameraPos"].SetValue(game.player.pos);
            Matrix WorldInverseTranspose = Matrix.Transpose(Matrix.Invert(game.player.World));
            effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);

            
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}

