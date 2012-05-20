﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mechanect.Common;

namespace Mechanect.Screens
{
    /// <summary>
    /// This class represents the ITWorx screen.
    /// </summary>
    class ITworxScreen : FadingScreen
    {
        /// <summary>
        /// Creates a new instance of ITWorxScreen.
        /// </summary>
        public ITworxScreen()
            : base("Resources/Images/ITWorx", 1f, 0, 0, -0.06f)
        { }
        /// <summary>
        /// Updates this screen
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        /// <param name="covered">specifies wether the screen is covered.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new AllExperiments(new User()));
            }
        }
        /// <summary>
        /// Draws the content of this screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }
    }
}