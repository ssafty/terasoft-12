﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Components;

namespace Mechanect.Exp3
{
    public class Ball : CustomModel
    {
        public int Radius { get; set; }
        public double Mass { get; set; }

        public Ball(Vector3 intialPosition, int radius,  GraphicsDevice device, ContentManager content)
            : base(content.Load<Model>(@"Models/ball"), intialPosition, Vector3.Zero, new Vector3(0.02f), device)
        {
            Radius = radius;
        }

        public void Rotate(Vector3 displacement)
        {
            Rotation = new Vector3(displacement.Length() / 3, (float)Math.Atan2(displacement.X, displacement.Z), 0);
        }

        public void setHeight(float height)
        {
            Position = new Vector3(Position.X, height, Position.Z);
        }
    }
}