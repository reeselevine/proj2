using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public enum GameObjectType
    {
        None, Player, Wall
    }

    // Super class for all game objects.
    abstract public class GameObject
    {
        public GameController game;
        public GameObjectType type = GameObjectType.None;
        public Vector3 pos;
        public BasicEffect basicEffect;

        public abstract void Update(GameTime gametime);
        public void Draw(GameTime gametime)
        {
           //do something
        }

       

        // These virtual voids allow any object that extends GameObject to respond to tapped and manipulation events
        public virtual void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {

        }

        public virtual void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {

        }

        public virtual void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {

        }

        public virtual void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }
    }
}
