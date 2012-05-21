﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using UI.Cameras;
using UI.Components;

namespace Mechanect.Exp3
{
    public class Hole :CustomModel
    {
        public int Radius { get; set; }
        private Vector3 shootingPos;
        int terrainWidth;
        int terrainHeight;

        public Hole(ContentManager content, GraphicsDevice device, int terrainWidth, int terrainHeight, int radius, Vector3 shootingPos)
            : base(content.Load<Model>(@"Models/holemodel"), GeneratePosition(radius,terrainWidth,terrainHeight,shootingPos),
            Vector3.Zero,  new Vector3((float)Constants3.scaleRatio*radius), device)
        {
            Radius = radius;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
            this.shootingPos = shootingPos ;
        }
       
        /// <summary>
        /// Generates a position for the hole according to certain constraints given the terrain width, terrain height, shooting position and the hole radius.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///<param name="radius">The radius of the hole.</param>
        ///<param name="terrainWidth">The width of the terrain or the environment where the hole should be generated within.</param>
        ///<param name="terrainHeight">The height of the terrain or the environment where the hole should be generated within.</param>
        ///<param name="shootingPosition">The shooting position of the user.</param>
        ///</remarks>
        /// <returns>
        /// Vector which is the randomly generated position of the hole.
        /// </returns>

        public static Vector3 GeneratePosition(int radius,int terrainWidth, int terrainHeight,Vector3 shootingPosition)
        {
            float X = Tools3.GenerateRandomValue(-terrainWidth / 4, terrainWidth / 4);
            float Y = 0;
            float Z = Tools3.GenerateRandomValue(-(terrainHeight- radius)/2, (shootingPosition.Z - radius));
            
              return new Vector3(X, Y, Z);
        }
    }
}