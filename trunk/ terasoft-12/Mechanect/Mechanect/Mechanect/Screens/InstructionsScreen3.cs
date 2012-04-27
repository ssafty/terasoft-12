﻿using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : GameScreen
    {
        string instructions = " The point of this game is to shoot the ball that it reaches the hole with zero velocity";
        Instruction instruction;
        User3 user3;

        public InstructionsScreen3(User3 user3)
        {
            this.user3 = user3;
        }
        public InstructionsScreen3(string instructions, User3 user)
            
        {
            this.instructions = instructions;
            user3 = user;
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        
        public override void LoadContent()

        {
            instruction = new Instruction(instructions, ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.GraphicsDevice, user3);
            instruction.Font1 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1");
            instruction.MyTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/screen");
           
        }
        

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>
        
        public override void Update(GameTime gameTime, bool covered)
        {
            if (instruction.Button.IsClicked())
                    {
                        ExitScreen();
              ScreenManager.AddScreen(new Settings3(user3));
                   }
            instruction.Button.Update(gameTime);
            base.Update(gameTime, false);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {

            instruction.Draw(gameTime);
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// This is called when you want to exit the screen.
        /// </summary>  
        public override void Remove()
        {
            base.Remove();
            
        }

    }
}