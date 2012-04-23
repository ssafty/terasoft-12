﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace Mechanect.Common
{
    class MainMenuButton : Button
    {

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// calling the super constructor with requiered parameters
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        public MainMenuButton(ContentManager c, Vector2 p, int sw, int sh)
            : base(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-s"),
            c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-m"), p, sw, sh,
            c.Load<Texture2D>("Textures/Buttons/hand")) { }
    }
}
