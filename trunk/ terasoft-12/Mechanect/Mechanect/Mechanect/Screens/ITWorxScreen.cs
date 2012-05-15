﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Screens
{
    /// <summary>
    /// This is a screen containg the GUC logo.
    /// </summary>
    class ITworxScreen : FadingScreen
    {
        /// <summary>
        /// This is the default construtor for ITWorx class.
        /// </summary>
        public ITworxScreen()
            : base("Resources/Images/ITWorx", 1f, 0, 0, -70)
        { }

        /// <summary>
        /// Updates the content of this screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        /// <param name="covered">specifies wether the screen is covered.</param>
        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, false);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new GUCScreen());
            }
        }
    }
}