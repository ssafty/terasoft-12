﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    public class KineckAnimation
    {
        #region Fields

        private SkinnedCustomModel model;
        private User user;

        #endregion

        #region Initialization

        /// <summary>
        /// Used to animate the SkinnedCustomModel according to the movement of the player.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="model">the model to animate</param>
        /// <param name="user">the user which is tracked</param>
        public KineckAnimation(SkinnedCustomModel model, User user)
        {
            this.model = model;
            this.user = user;
        }

        #endregion

        #region update

        /// <summary>
        /// Updating the movement
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        public void Update()
        {
            MoveRightLeg();
        }

        #endregion

        #region Animating Bones

        /// <summary>
        /// Animating the the right leg
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void MoveRightLeg()
        {
            Skeleton skeleton = user.Kinect.requestSkeleton();
            if (skeleton != null)
            {
                Joint foot = skeleton.Joints[JointType.FootRight];
                Joint hip = skeleton.Joints[JointType.HipRight];
                float value = (hip.Position.Z - foot.Position.Z) * 3;
                if (value < 0)
                    model.MoveBone("R_Knee", -2, new Vector3(0, 0, value));
                else
                    model.MoveBone("R_Thigh", -2, new Vector3(0, 0, value));

            }
        }


        /// <summary>
        /// Animating the left leg
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void MoveLeftLeg()
        {
            Skeleton skeleton = user.Kinect.requestSkeleton();
            if (skeleton != null)
            {
                Joint foot = skeleton.Joints[JointType.FootLeft];
                Joint hip = skeleton.Joints[JointType.HipLeft];
                float value = (hip.Position.Z - foot.Position.Z) * 3;
                if (value < 0)
                    model.MoveBone("L_Knee", -2, new Vector3(0, 0, value));
                else
                    model.MoveBone("L_Thigh", -2, new Vector3(0, 0, value));

            }
        }

        #endregion
    }
}
