﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UI.Animation;
using UI.Cameras;
using Physics;

namespace Mechanect.Exp3
{
    /// <summary>
    /// represents the simulation of the result
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class Simulation
    {
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private Ball ball;
        private Vector3 shootPosition;

        private SpriteFont font;

        private BallAnimation animation1;
        private BallAnimation animation2;
        private bool secondAnimationStarted;
        /// <summary>
        /// the chase camera instance
        /// </summary>
        public ChaseCamera Camera { get; private set; }

        private String velocity1;
        private String velocity2;

        /// <summary>
        /// constructs a new Simulation instance and intializes it
        /// </summary>
        /// <param name="ball">ball instance</param>
        /// <param name="shootPosition">shoot position vector</param>
        /// <param name="holePosition">hole position vector</param>
        /// <param name="shootVelocity">shoot velocity vector</param>
        /// <param name="friction">friction magnitude</param>
        /// <param name="content">content manager</param>
        /// <param name="device">draphics device</param>
        /// <param name="spriteBatch">sprite batch</param>
        public Simulation(Ball ball, Hole hole, Vector3 shootPosition, Vector3 shootVelocity, float friction, ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            this.device = device;
            this.spriteBatch = spriteBatch;
            this.ball = ball;
            this.shootPosition = shootPosition;

            ball.Position = shootPosition;

            font = content.Load<SpriteFont>("SpriteFont1");

            Vector3 optimalVelocity = LinearMotion.CalculateIntialVelocity(hole.Position - 
                shootPosition, 0, friction);

            animation1 = new BallAnimation(ball, hole, shootVelocity, friction);
            animation2 = new BallAnimation(ball, hole, optimalVelocity, friction);
            Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);

            velocity1 = String.Format("<{0,4:0.0},{1,4:0.0},{2,4:0.0}>", shootVelocity.X, 
                shootVelocity.Y, shootVelocity.Z);
            velocity2 = String.Format("<{0,4:0.0},{1,4:0.0},{2,4:0.0}>", optimalVelocity.X, 
                optimalVelocity.Y, optimalVelocity.Z);
        }

        /// <summary>
        /// updates the chase camera and model's position and orientation according to the time elapsed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            ModelLinearAnimation current = animation1;
            if (animation1.Finished())
            {
                if (!secondAnimationStarted)
                {
                    secondAnimationStarted = true;
                    ball.Position = shootPosition;
                    Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);
                }
                current = animation2;
            }
            current.Update(gameTime.ElapsedGameTime);
            Camera.Move(ball.Position);
            Camera.Rotate(new Vector3(0, 0.005f, 0));
            Camera.Update();
        }

        /// <summary>
        /// draws the velocity values
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, 
                SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            if (secondAnimationStarted)
            {
                spriteBatch.DrawString(font, "Replay  ", new Vector2(5, 0), Color.Black, 0, 
                    Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if ((int)(animation2.ElapsedTime.TotalSeconds / 0.4) % 2 == 0)
                {
                    spriteBatch.DrawString(font, "Optimal ", new Vector2(5, font.MeasureString(velocity2).Y), 
                        Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
            else
            {
                if ((int)(animation1.ElapsedTime.TotalSeconds / 0.4) % 2 == 0)
                {
                    spriteBatch.DrawString(font, "Replay  ", new Vector2(5, 0), Color.Red, 0, 
                        Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
                spriteBatch.DrawString(font, "Optimal ", new Vector2(5, font.MeasureString(velocity2).Y), 
                    Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }

            spriteBatch.DrawString(font, velocity1, new Vector2(font.MeasureString("Optimal ").X, 0),
                (!secondAnimationStarted) ? Color.Red : Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(font, velocity2, new Vector2(font.MeasureString("Optimal ").X, 
                font.MeasureString(velocity2).Y), (secondAnimationStarted) ? Color.Red : Color.Black, 0, 
                Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

        public bool Finished()
        {
            return animation2.Finished();
        }

    }
}