﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using UI.Cameras;
using UI.Animation;
using Mechanect.Classes;
using Physics;

namespace Mechanect.Exp3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Bar bar;
        private Environment3 environment;
        private User3 user;

        private TargetCamera targetCamera;
        private BallAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool firstAnimation;

        private float arriveVelocity;
        private Vector3 shootVelocity;

        /// <summary>
        /// constructs a new Experiment3 screen
        /// </summary>
        /// <param name="user">user</param>
        /// <remarks>
        /// Author : Bishoy Bassem
        /// </remarks>
        public Experiment3(User3 user)
        {
            arriveVelocity = 10;
            firstAnimation = true;
            user.shootingPosition = new Vector3(0, 3, 62);
            this.user = user;
        }

        /// <summary>
        /// loads the experiment's content
        /// </summary>
        /// <remarks>
        /// Author : Bishoy Bassem
        /// </remarks>
        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 140, 250), new Vector3(0,80,0), ScreenManager.GraphicsDevice);

            environment = new Environment3(ScreenManager.Game.Content, ScreenManager.GraphicsDevice, user);
            environment.LoadContent();

            ball = new Ball(4, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            ball.GenerateIntialPosition(environment.terrainWidth, environment.terrainHeight);

            environment.ball = ball;

            Vector3 intialVelocity = LinearMotion.CalculateIntialVelocity(user.shootingPosition - ball.Position, arriveVelocity, environment.Friction);

            animation = new BallAnimation(ball, environment.HoleProperty, intialVelocity, environment.Friction);

            bar = new Bar(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width * 0.99f, ScreenManager.GraphicsDevice.Viewport.Width * 0.3f), ScreenManager.SpriteBatch, new Vector2(ball.Position.X, ball.Position.Z), new Vector2(ball.Position.X, ball.Position.Z), new Vector2(user.shootingPosition.X, user.shootingPosition.Z), ScreenManager.Game.Content);

            //environment = new Environment3(ball.Position, intialVelocity, ScreenManager.SpriteBatch, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, user, ball);
        }

        /// <summary>
        /// updates the experiment's screen
        /// </summary>
        /// <param name="gameTime">GameTime instance</param>
        /// <remarks>
        /// Author : Bishoy Bassem
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            environment.PlayerModel.Update();
            environment.PlayerAnimation.Update();
            ball.SetHeight(environment.GetHeight(ball.Position));
            if (firstAnimation)
            {
                float distance = animation.Displacement.Length();
                float totalDistance = (user.shootingPosition - animation.StartPosition).Length();
                if (distance / totalDistance > 0.5 && !pauseScreenShowed)
                {
                    pauseScreenShowed = true;
                    FreezeScreen();
                    ScreenManager.LoadScreen("pausescreen");
                    //add pause screen
                }
                bar.Update(new Vector2(ball.Position.X,ball.Position.Z));
                if (distance / totalDistance > 1)
                {
                    firstAnimation = false;
                    this.shootVelocity = new Vector3(10, 0, -10);
                    animation = new BallAnimation(ball, environment.HoleProperty, this.shootVelocity, environment.Friction);
                }
                /*if (environment.hasBallEnteredShootRegion())
                {
                    Vector3 shootVelocity = environment.Shoot(gameTime);
                    if (user.hasShot && shootVelocity.Length() != 0)
                    {
                        firstAnimation = false;
                        this.shootVelocity = environment.GetVelocityAfterCollision(shootVelocity);
                        new BallAnimation(ball, environment.HoleProperty, this.shootVelocity, environment.Friction);
                    }
                }*/
                if (animation.Finished())
                {
                    //add final screen
                }
            }
            else if (animation.Finished() && simulation == null)
            {
                simulation = new Simulation(ball, environment.HoleProperty, user.shootingPosition, shootVelocity, environment.Friction, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
            }
            
            if (simulation != null)
            {
                simulation.Update(gameTime);
                if (simulation.Finished())
                {
                    //add final screen
                }
            }
            else
            {
                animation.Update(gameTime.ElapsedGameTime);
            }
            
            targetCamera.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// draws the experiment's screen
        /// </summary>
        /// <param name="gameTime">GameTime instance</param>
        /// <remarks>
        /// Author : Bishoy Bassem
        /// </remarks>
        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulation != null)
            {
                camera = simulation.Camera;
            }
            environment.Draw(camera, gameTime);
            ball.Draw(camera);
            if (firstAnimation)
            {
                bar.Draw();
            }
            if (simulation != null)
            {
                simulation.Draw();
            }
        }

        public override void UnloadContent()
        {

        }

    }

}
