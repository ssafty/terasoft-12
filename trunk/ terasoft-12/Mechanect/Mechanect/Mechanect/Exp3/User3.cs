﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Screens;
using Mechanect.Common;

namespace Mechanect.Exp3
{
    public class User3:User
    {
       

        private Joint trackedJoint;
        public Joint TrackedJoint  //  the user's tracked joint
        {
            get
            {
                return trackedJoint;
            }
            set
            {
                trackedJoint = value;
            }
        }

        private bool rightLeg;  // is the user using his right leg or left
        public bool RightLeg
        {
            get
            {
                return rightLeg;
            }
            set
            {
                rightLeg = value;
            }
        }

        private bool movedForward; // has the user moved his leg forward
        public bool MovedForward
        {
            get
            {
                return movedForward;
            }
            set
            {
                movedForward = value;
            }
        }

        //x position
        private double initialRightLegPositionX; //meters // stores the initial position of the right leg
        public double InitialRightLegPositionX
        {
            get
            {
                return initialRightLegPositionX;
            }
            set
            {
                initialRightLegPositionX = value;
            }
        }
        private double currentRightLegPositionX; //meters // stores the current position of the right leg 
        public double CurrentRightLegPositionX
        {
            get
            {
                return currentRightLegPositionX;
            }
            set
            {
                currentRightLegPositionX = value;
            }
        }

        //z position
        private double initialRightLegPositionZ; //meters
        public double InitialRightLegPositionZ
        {
            get
            {
                return initialRightLegPositionZ;
            }
            set
            {
                initialRightLegPositionZ = value;
            }
        }


        private double currentRightLegPositionZ; //meters
        public double CurrentRightLegPositionZ
        {
            get
            {
                return currentRightLegPositionZ;
            }
            set
            {
                currentRightLegPositionZ = value;
            }
        }

        //left leg

        //x position
        private double initialLeftLegPositionX; //meters // stores the initial position of the left leg
        public double InitialLeftLegPositionX
        {
            get
            {
                return initialLeftLegPositionX;
            }
            set
            {
                initialLeftLegPositionX = value;
            }
        }

        private double currentLeftLegPositionX; //meters // stores the current position of the left leg 
        public double CurrentLeftLegPositionX
        {
            get
            {
                return currentLeftLegPositionX;
            }
            set
            {
                currentLeftLegPositionX = value;
            }
        }

        //z position
        private double initialLeftLegPositionZ; //meters
        public double InitialLeftLegPositionZ
        {
            get
            {
                return initialLeftLegPositionZ;
            }
            set
            {
                initialLeftLegPositionZ = value;
            }
        }


        private double currentLeftLegPositionZ; //meters
        public double CurrentLeftLegPositionZ
        {
            get
            {
                return currentLeftLegPositionZ;
            }
            set
            {
                currentLeftLegPositionZ = value;
            }
        }



        public Vector3 velocity { get; set; }

        private bool trying; // is the user in the trying mode or shooting
        public bool Trying
        {
            get
            {
                return trying;
            }
            set
            {
                trying = value;
            }
        }

        private double angle; // angle of the leg
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        private double assumedLegMass;
        public double AssumedLegMass
        {
            get
            {
                return assumedLegMass;
            }
            set
            {
                assumedLegMass = value;
            }
        }
        private Vector3 shootingPosition;
        public Vector3 ShootingPosition
        {
            get
            {
                return shootingPosition;
            }
            set
            {
                shootingPosition = value;
            }
        }

        public double currentTime { get; set; }
        public double initialTime { get; set; }
        public int timeCounter { get; set; }
        public bool hasJustStarted { get; set; }
        public bool hasJustSlipped { get; set; }
        public bool hasShot { get; set; }
        public bool firstUpdate { get; set; }
        public int consecutiveFrame { get; set; }
        public double previousLeftLegPositionX;
        public double previousLeftLegPositionZ;
        public double previousRightLegPositionX;
        public double previousRightLegPositionZ;
        public bool hasMissed { get; set; }
        public bool hasPlayerMoved { get; set; }


        public User3()
        {



            initialLeftLegPositionX = 0;
            currentLeftLegPositionX = 0;
            initialLeftLegPositionZ = 0;
            currentLeftLegPositionZ = 0;

            initialRightLegPositionX = 0;
            currentRightLegPositionX = 0;
            initialRightLegPositionZ = 0;
            currentRightLegPositionZ = 0;

            velocity = Vector3.Zero;
            angle = 0;

            movedForward = false;
            trying = true;
            assumedLegMass = 0.01f;
            currentTime = 0;
            initialTime = 0;
            timeCounter = 0;
            hasJustSlipped = false;
            hasJustStarted = true;
            hasShot = false;
            firstUpdate = true;
            consecutiveFrame = 0;
            hasMissed = false;
            hasPlayerMoved = false;




        }
        /// <summary>
        /// Takes the minimum and maximum possible values for the mass of the foot and sets it to a random float number between these two numbers.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="minMass">
        /// The minimum possible value for the foot mass.
        /// </param>
        /// /// <param name="maxMass">
        /// The maximum possible value for the foot mass.
        /// </param>
     
         public void SetFootMassInThisRange(float minMass, float maxMass)
         {
             assumedLegMass = GenerateFootMass(minMass, maxMass);
         } 
         /// <summary>
         /// Takes the minimum and maximum possible values for the mass of the foot and generates a random float between these two numbers.
         /// </summary>
         /// <remarks>
         ///<para>AUTHOR: Khaled Salah </para>
         ///</remarks>
         /// <param name="minMass">
         /// The minimum possible value for the foot mass.
         /// </param>
         /// /// <param name="maxMass">
         /// The maximum possible value for the foot mass.
         /// </param>
         /// <returns>
         /// Float number which is the generated random value.
         /// </returns>
         public float GenerateFootMass(float min, float max)
         {
             if ((min >= 0) && (max >= 0) && (max > min))
             {
                 var random = new Random();
                 var generatedMass = ((float)(random.NextDouble() * (max - min))) + min;
                 return generatedMass;
             }
             else throw new ArgumentException("parameters have to be non negative numbers and max value has to be greater than min value");
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>>
         /// <summary>
         ///  Updates the velocity, angle variables of the User3 after each captured skeleton frame.
         /// </summary>

         public void UpdateMeasuringVelocityAndAngle(GameTime gameTime)
         {
             setSkeleton();
             Skeleton skeleton = USER;
             if (skeleton != null)
             {
                 if (GameScreen.frameNumber % 3 == 0)
                 {
                     StoreTime(gameTime);
                     if (!hasShot && !hasMissed) 
                     {
                         if (HasJustStarted())
                         {
                             StoreStartingPosition();
                             StoreInitialPosition();
                             SetStarted();
                         }
                         else
                         {
                             StoreCurrentPosition();
                             if (!hasPlayerMoved)
                                 HasPlayerMoved();
                             else
                             {
                                 if (IsMovingForward())
                                 {
                                    
                                     UpdateSpeed();
                                     UpdateAngle();
                                     StorePreviousPosition();
                                 }
                                 else
                                 {
                                     if (HasAlreadyMovedForward() && HasMovedMinimumDistance())
                                         hasShot = true;
                                     else
                                         if (HasAlreadyMovedForward() && !HasMovedMinimumDistance())
                                             hasMissed = true;
                                         else
                                             StoreInitialTime(gameTime);

                                 }
                             }
                         }
                     }

                 }

             }
             GameScreen.frameNumber++;
         }

         public double startRightLegPositionX;
         public double startRightLegPositionZ;
         public double startLeftLegPositionX;
         public double startLeftLegPositionZ;
         private void StoreStartingPosition()
         {
            startLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
            startLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
            startRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
            startRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }

         private bool HasMovedMinimumDistance()
         {
             if (rightLeg)
                 return InitialRightLegPositionZ - currentRightLegPositionZ > Constants3.minimumShootingDistance;
             return InitialLeftLegPositionZ - CurrentLeftLegPositionZ > Constants3.minimumShootingDistance;
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// Checks if the User3 is currently moving their leg forward or not
         /// </summary>
         /// <returns>returns true iff the User3 moved his leg forward</returns>

         public bool IsMovingForward()
         {
             if (this.USER != null)
             {
                 double currentZ = CurrentLeftLegPositionZ;
                 double previousZ = previousLeftLegPositionZ;
                 if (RightLeg)
                 {
                     currentZ = CurrentRightLegPositionZ;
                     previousZ = previousRightLegPositionZ;
                 }

                 if (currentZ - previousZ < (-1 * Constants3.movingForwardTolerance))
                 {
                     MovedForward = true;
                     return true;

                 }
                 if (!movedForward)
                 {
                     if (RightLeg)
                     {
                         initialRightLegPositionZ = CurrentRightLegPositionZ;
                         InitialRightLegPositionX = CurrentRightLegPositionX;
                     }
                     else
                     {
                         initialLeftLegPositionZ = CurrentLeftLegPositionZ;
                         InitialLeftLegPositionX = currentLeftLegPositionX;
                     }
                 }

             }
             return false;
         }

         public void UpdateAngle()
         {
             double positionX1 = InitialLeftLegPositionX;
             double positionX2 = currentLeftLegPositionX;
             double positionZ1 = initialLeftLegPositionZ;
             double positionZ2 = currentLeftLegPositionZ;
             if (rightLeg)
             {
                 positionX1 = initialRightLegPositionX;
                 positionX2 = CurrentRightLegPositionX;
                 positionZ1 = initialRightLegPositionZ;
                 positionZ2 = CurrentRightLegPositionZ;
             }
             if (positionZ2 != positionZ1)
                 Angle = Math.Atan((positionX2 - positionX1) / Math.Abs((positionZ2 - positionZ1)));
             else
                 if (positionX1 < positionX2)
                     Angle = (Math.PI / 2);
                 else
                     Angle = -(Math.PI / 2);

         }




         public void HasPlayerMoved()
         {

             Skeleton player = USER;
             if (Math.Abs(CurrentLeftLegPositionZ - startLeftLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = false;
                 TrackedJoint = player.Joints[JointType.AnkleLeft];
                 hasPlayerMoved = true;
                 return;
             }
             if (Math.Abs(CurrentRightLegPositionZ - startRightLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = true;
                 TrackedJoint = player.Joints[JointType.AnkleRight];
                 hasPlayerMoved = true;
                 return;
             }
            
         }




         public bool HasAlreadyMovedForward()
         {
             return movedForward;
         }
         public bool HasShot()
         {
             return hasShot;
         }
         public void UpdateSpeed()
         {
             double currentZ = CurrentLeftLegPositionZ;
             double initialZ = InitialLeftLegPositionZ;
             double currentX = CurrentLeftLegPositionX;
             double initialX = initialLeftLegPositionX;
             if (RightLeg)
             {
                 currentZ = CurrentRightLegPositionZ;
                 initialZ = InitialRightLegPositionZ;
                 currentX = currentRightLegPositionX;
                 initialX = InitialRightLegPositionX;
             }
             double deltaTime = Math.Abs(currentTime - initialTime);
             Vector3 deltaPosition = new Vector3((float)(currentX - initialX), 0, (float)(currentZ - initialZ));
             Vector3 finalVelocity = Tools3.GetVelocity(deltaPosition, deltaTime);
             velocity = finalVelocity;
         }


         public bool HasJustStarted()
         {
             return hasJustStarted;
         }
         public void SetStarted()
         {
             hasJustStarted = false;
         }

         public void StoreInitialPosition()
         {
             initialLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             initialLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             initialRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             initialRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
             previousLeftLegPositionX = initialLeftLegPositionX;
             previousLeftLegPositionZ = initialLeftLegPositionZ;
             previousRightLegPositionX = initialRightLegPositionX;
             previousRightLegPositionZ = InitialRightLegPositionZ;
         }


         public void StoreCurrentPosition()
         {
             CurrentLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             CurrentLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             CurrentRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             CurrentRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }


         public void StoreInitialTime(GameTime gameTime)
         {
             initialTime = gameTime.TotalGameTime.TotalSeconds;
         }
         public void StoreTime(GameTime gameTime)
         {
             if (firstUpdate)
             {
                 StoreInitialTime(gameTime);
                 firstUpdate = false;
             }
             else
                 currentTime = gameTime.TotalGameTime.TotalSeconds;
         }

         public void StorePreviousPosition()
         {
             if (currentRightLegPositionZ != 0)
             {
                 previousLeftLegPositionX = currentLeftLegPositionX;
                 previousLeftLegPositionZ = currentLeftLegPositionZ;
                 previousRightLegPositionX = currentRightLegPositionX;
                 previousRightLegPositionZ = currentRightLegPositionZ;
             }
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// scales the velocity of the leg's User3 relative to the assumed mass of the User3's leg and maps the velocity in meters to pixels
         /// </summary>
         /// <returns>returns the scales velocity</returns>
         public Vector3 SetVelocityRelativeToGivenMass()
         {
             float ratio = (float)(Constants3.normalLegMass / assumedLegMass);
             return new Vector3(velocity.X * ratio, 0, velocity.Z * ratio);

         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// initializes all the variables that stores the User3's movement inorder to try shooting again
         /// </summary>

         public void ResetUserForShootingOrTryingAgain()
         {

             InitialLeftLegPositionX = 0;
             CurrentLeftLegPositionX = 0;
             InitialLeftLegPositionZ = 0;
             CurrentLeftLegPositionZ = 0;

             InitialRightLegPositionX = 0;
             CurrentRightLegPositionX = 0;
             InitialRightLegPositionZ = 0;
             CurrentRightLegPositionZ = 0;

             currentTime = 0;
             initialTime = 0;
             timeCounter = 0;
             hasJustSlipped = false;
             hasJustStarted = true;
             hasShot = false;
             firstUpdate = true;
             consecutiveFrame = 0;

             velocity = Vector3.Zero;
             Angle = 0;

             MovedForward = false;
             Trying = true;
             GameScreen.frameNumber = 0;
             hasMissed = false;
             hasPlayerMoved = false;




         }
       
    }
}