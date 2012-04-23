﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
    class User3:User
    {
        private bool hasSetInitialPositionForAngle;
        public bool HasSetInitialPositionForAngle
        {
            get
            {
                return hasSetInitialPositionForAngle;
            }
            set
            {
                hasSetInitialPositionForAngle = value;
            }
        }


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

        // store 

        private double storeX1;
        public double StoreX1
        {
            get
            {
                return storeX1;
            }
            set
            {
                storeX1 = value;

            }
        }
        private double storeX2;
        public double StoreX2
        {
            get
            {
                return storeX2;
            }
            set
            {
                storeX2 = value;

            }
        }
        private double storeZ1;
        public double StoreZ1
        {
            get
            {
                return storeZ1;
            }
            set
            {
                storeZ1 = value;

            }
        }
        public double storeZ2;
        public double StoreZ2
        {
            get
            {
                return storeZ2;
            }
            set
            {
                storeZ2 = value;

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



        private double velocity;
        public double Velocity // the velocity of the users leg
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
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
         public User3(float assumedLegMass)
        {
           
            this.assumedLegMass = assumedLegMass;

            initialLeftLegPositionX = 0;
            currentLeftLegPositionX = 0;
            initialLeftLegPositionZ = 0;
            currentLeftLegPositionZ = 0;

            initialRightLegPositionX = 0;
            currentRightLegPositionX = 0;
            initialRightLegPositionZ = 0;
            currentRightLegPositionZ = 0;

            storeX1 = 0;
            storeX2 = 0;
            storeZ1 = 0;
            storeZ2 = 0;
            
            velocity = 0;
            angle = 0;
        
            movedForward = false;
            trying = true;
            hasSetInitialPositionForAngle = false;
         

        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Takes the minimum and maximum possible values for the mass of the foot and sets it to a random float number between these two numbers.
        /// </summary>
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
         /// <remarks>
         ///<para>AUTHOR: Khaled Salah </para>
         ///</remarks>
         /// <summary>
         /// Takes the minimum and maximum possible values for the mass of the foot and generates a random float between these two numbers.
         /// </summary>
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
             var random = new Random();
             var generatedMass = ((float)(random.NextDouble() * (max - min))) + min;
             return generatedMass;
         }
       
    }
}
