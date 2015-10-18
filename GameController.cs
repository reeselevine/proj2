﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;

namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class GameController : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        public List<Ghost> ghosts;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        public Player player;
        public LightBeam lightBeam;
        public AccelerometerReading accelerometerReading;
        public GameInput input;
        public int score;
        public MainPage mainPage;
        public MazeController mazeController;
        private double generateGhost;
        public int maxGhosts;
        // TASK 4: Use this to represent difficulty
        public float difficulty;

   
        // Random number generator
        public Random random;

        // World boundaries that indicate where the edge of the screen is for the camera.
        public int size;
        public float boundaryNorth;
        public float boundaryEast;
        public float boundarySouth;
        public float boundaryWest;

        public bool started = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public GameController(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
 
            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            random = new Random();
            input = new GameInput();
            size = 10;
            maxGhosts = 10;
            generateGhost = 0.99;
            // Set boundaries.
            boundaryNorth = 2.6f;
            boundaryEast = size * 10 - 2.6f;
            boundarySouth = size * 10 - 2.6f;
            boundaryWest = 2.6f;

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;

            score = 0;
            difficulty = 1;
        }

        protected override void LoadContent()
        {
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();
            ghosts = new List<Ghost>();

            // Create game objects.
            player = new Player(this);
            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Our Game Sucks";

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = keyboardManager.GetState();
            if (started)
            {
                if (player.HasWon())
                {
                    mainPage.EndGame();
                    PrepareForNewGame();
                    return;
                }
                DoGhostStuff();
                flushAddedAndRemovedGameObjects();
                accelerometerReading = input.accelerometer.GetCurrentReading();
                player.Update(gameTime);
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);
                }
                mainPage.UpdateScore(score);
            }
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
                this.Dispose();
                App.Current.Exit();
            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.CornflowerBlue);
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
                }
            }
            // Handle base.Draw
            base.Draw(gameTime);
        }
        // Count the number of game objects for a certain type.
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

        public void PrepareForNewGame()
        {
            gameObjects.Clear();
            ghosts.Clear();
            player.pos = new Vector3(5, 1, 5);
            player.prevY = 0;
            player.ghostEncounters = 3;
            mazeController = new MazeController(this, size);
            lightBeam = new LightBeam(this, mazeController.cellsize, 1000);
            gameObjects.Add(mazeController.ground);
            gameObjects.AddRange(mazeController.walls);
            gameObjects.Add(lightBeam);
        }

        private void DoGhostStuff()
        {
            // check to see if player is dead
            foreach (Ghost ghost in ghosts)
            {
                if (!player.invincible && player.IsEncountering(ghost))
                {
                    player.ghostEncounters--;
                    if (player.ghostEncounters == 0)
                    {
                        mainPage.EndGame();
                        PrepareForNewGame();
                        return;
                    }
                    player.invincible = true;
                }
            }
            // add a ghost sometimes
            double next = random.NextDouble();
            if (ghosts.Count < maxGhosts && next > generateGhost)
            {
                Ghost ghost = new Ghost(this);
                gameObjects.Add(ghost);
                ghosts.Add(ghost);
            }
        }

        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            player.OnManipulationUpdated(sender, args);
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                if (obj.basicEffect != null) { obj.basicEffect.View = player.View; }
                obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            player.OnManipulationCompleted(sender, args);
        }

    }
}
