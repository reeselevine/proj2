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
  
        public Wall(GameController game, Vector3 pos)
        {
            this.game = game;
            type = GameObjectType.Wall;
            this.pos = pos;
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
