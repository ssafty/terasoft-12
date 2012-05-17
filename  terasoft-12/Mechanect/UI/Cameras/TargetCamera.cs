﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI.Cameras
{
    /// <summary>
    /// represents the target camera type
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class TargetCamera : Camera
    {

        private Vector3 target;

        /// <summary>
        /// constructs a TargetCamera instance
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="position">is the position of the camera</param>
        /// <param name="target">is the postion the camera is looking at</param>
        public TargetCamera(Vector3 position, Vector3 target, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = position;
            this.target = target;
        }

        /// <summary>
        /// causes the camera to update its view and projection matrices
        /// </summary>
        public override void Update()
        {
            this.View = Matrix.CreateLookAt(Position, target, Vector3.Up);
        }

    }

}