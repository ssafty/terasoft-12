﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;
using Mechanect.Common;
using Mechanect.Cameras;



namespace Mechanect.Screens
{
     class SimulationScreen : Mechanect.Common.GameScreen
     {

         private Environment3 environment;
         private GraphicsDevice graphics;

         private ResultSimulation sim;

         public SimulationScreen(Environment3 environment)
         {
             this.environment = environment;
         }
         public override void LoadContent()
         {

             graphics = this.ScreenManager.GraphicsDevice;

             Vector3 optimalVelocity = environment.ball.InitialVelocity;
             Vector3 shootVelocity = environment.ball.InitialVelocity;
             float friction = environment.Friction;
             environment.ball.ballModel.Position = environment.user.ShootingPosition;
             sim = new ResultSimulation(graphics, ScreenManager.SpriteBatch, ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1"), Color.Black, environment.ball.ballModel, shootVelocity, optimalVelocity, friction);
         }

         public override void UnloadContent()
         {

         }

         public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
         {

             sim.Update(gameTime);
             if (sim.SimulationFinished)
             {
                 ScreenManager.AddScreen(new LastScreen(environment.user, 3));
                 ExitScreen();
             }
             base.Update(gameTime, covered);

         }

         public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
         {
             environment.DrawEnvironment(sim.Camera, gameTime);
             environment.ball.Draw(gameTime, sim.Camera);
         }




         public override void Remove()
         {

             base.Remove();
         }
    }
        
    
}
