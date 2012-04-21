﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mechanect.Classes
{
    class Environment3 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Hole hole;
        private Ball ball;
        private User user;
        private float wind;
        private float friction;
        private bool hasCollidedWithBall, ballShot;
        private double ballMass, assumedLegMass;
        private Vector2 tolerance;
        /*
        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        Effect effect;
        VertexPositionColorNormal[] vertices;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        int[] indices;

        private float angle = 0f;
        private int terrainWidth = 4;
        private int terrainHeight = 3;
        private float[,] heightData; //2D array
        VertexBuffer myVertexBuffer;
        IndexBuffer myIndexBuffer;

        Vector3 cameraPosition = new Vector3(130, 30, -50);
        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.3f;
        const float moveSpeed = 30.0f;
        MouseState originalMouseState;

        Texture2D[] skyboxTextures;
        Model skyboxModel;*/

        public Environment3(Microsoft.Xna.Framework.Game game, User user, float minBallMass, float maxBallMass) : base(game)
        {

            this.user = user;
            ball = new Ball(minBallMass,maxBallMass);
           
        }


        /// <summary>
        /// This method verifies wether a method is solvable or not
        /// </summary>
        /// <returns>Retuns an int that represents the type of the problem with the experiment</returns>
        public int IsSolvable()
        {

            if (ball.Radius <= 0)
                return Constants3.negativeBRradius;
            if (ball.Mass <= 0)
                return Constants3.negativeBMass;
            if (hole.Radius <= 0)
                return Constants3.negativeHRadius;
            if (user.AssumedLegMass <= 0)
                return Constants3.negativeLMass;
            //hole position not before the leg position
            if (hole.Position.Z - user.ShootingPosition.Z > 0)
                return Constants3.negativeHPosZ;
            if (friction <= 0)
                return Constants3.negativeFriction;
            if (ball.Radius > hole.Radius)
                return Constants3.negativeRDifference;

            Vector3 finalPos = Vector3.Zero;//to be changed with the position returned from getFinalPostion();            
             //finalPos = getBallFinalPosition(getVelocityAfterCollision(new Vector3(Constants3.maxVelocityX, 0, Constants3.maxVelocityZ)));
            
            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared())
                return Constants3.holeOutOfFarRange;
            
            //finalPos = getBallFinalPosition(getVelocityAfterCollision(new Vector3(Constants3.mixVelocityX, 0, Constants3.mixVelocityZ)));
            
            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared()) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;

            return Constants3.solvableExperiment;//means solvable
        }

        /// <summary>
        /// Generates a solvable experiment
        /// </summary>
        public void GenerateSolvable()
        {
            int x = Constants3.solvableExperiment;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.holeOutOfNearRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Z == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(1, 0, 0)));
                        else friction++; break;
                    case Constants3.holeOutOfFarRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Y == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(1, 0, 0)));
                        else if (wind != 0)
                            wind--;
                        else if (friction != 1)
                            friction--;
                        else if (friction <= 1)
                            friction -= 0.1f;
                        break;
                    case Constants3.negativeRDifference: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.AssumedLegMass *= -1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: friction *= -1; break;
                }
            } while (x != Constants3.solvableExperiment);
        }


        /// <remarks>
        ///<para>AUTHOR: NAME </para>
        ///</remarks>
        /// <summary>
        /// Checks whether or not the ball will reach the hole with zero velocity, by checking if the user shot it with the optimum velocity, and calls methods to inform the user if he won or not.
        /// </summary>
        private void hasScored()
        {
            Vector3 hole = this.hole.Position;
            Vector3 ballVelocity = ball.Velocity;
            Vector3 InitialPosition = ball.Position;
            float optimumVx = (float)Math.Sqrt((2 * (wind + friction)) * (hole.X - InitialPosition.X));
            float optimumVy = (float)Math.Sqrt((2 * (wind + friction)) * (hole.Y - InitialPosition.Y));

            if (ballVelocity.X <= (optimumVx + tolerance.X) && ballVelocity.Y <= (optimumVy + tolerance.X + this.hole.Radius)
            && ballVelocity.X >= (optimumVx - tolerance.Y) && ballVelocity.Y >= (optimumVy - tolerance.Y + this.hole.Radius))
            {
                ballFallIntoHole();
                //winningWord(); to be implemented by Hegazy, commented to remove error
            }

            else
            {
                //losingWord(); to be implemented by Hegazy, commented to remove error
            }
        }

        #region Environment Generation Code
        
        /*protected void InitializeEnvironment()
        {
            device = graphics.GraphicsDevice;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Environment";
        }

        protected void LoadEnvironmentContent()
        {
            effect = Content.Load<Effect>("effects");
            SetUpCamera();


            UpdateViewMatrix();
            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            Texture2D heightMap = Content.Load<Texture2D>("heightmap");
            LoadHeightData(heightMap);

            skyboxModel = LoadModel("skybox2", out skyboxTextures);

            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();
        }


        protected void UpdateEnvironment(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Delete))
                angle += 0.05f;
            if (keyState.IsKeyDown(Keys.PageDown))
                angle -= 0.05f;
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            ProcessInput(timeDifference);
        }

        protected void DrawEnvironment(GameTime gameTime)
        {
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawSkybox();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;
            device.RasterizerState = rs;
            Matrix worldMatrix = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f) * Matrix.CreateRotationY(angle);
            effect.CurrentTechnique = effect.Techniques["Colored"];
            Vector3 lightDirection = new Vector3(1.0f, -1.0f, -1.0f);
            lightDirection.Normalize();
            effect.Parameters["xLightDirection"].SetValue(lightDirection);
            effect.Parameters["xAmbient"].SetValue(0.1f);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(worldMatrix);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = myIndexBuffer;
                device.SetVertexBuffer(myVertexBuffer);

                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }
        }

        private void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    if (heightData[x, y] < minHeight)
                        minHeight = heightData[x, y];
                    if (heightData[x, y] > maxHeight)
                        maxHeight = heightData[x, y];
                }
            }

            vertices = new VertexPositionColorNormal[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);

                    if (heightData[x, y] < minHeight + (maxHeight - minHeight) / 4)
                        vertices[x + y * terrainWidth].Color = Color.Blue;
                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 2 / 4)
                        vertices[x + y * terrainWidth].Color = Color.Green;
                    else if (heightData[x, y] < minHeight + (maxHeight - minHeight) * 3 / 4)
                        vertices[x + y * terrainWidth].Color = Color.Brown;
                    else
                        vertices[x + y * terrainWidth].Color = Color.White;
                }
            }
        }

        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
        }

        private void CopyToBuffers()
        {
            myVertexBuffer = new VertexBuffer(device, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            myVertexBuffer.SetData(vertices);
            myIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            myIndexBuffer.SetData(indices);
        }

        public struct VertexPositionColorNormal
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
            );
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;

            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        private void ProcessInput(float amount)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftrightRot -= rotationSpeed * xDifference * amount;
                updownRot -= rotationSpeed * yDifference * amount;
                Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
                UpdateViewMatrix();
            }
            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private Model LoadModel(string assetName, out Texture2D[] textures)
        {

            Model newModel = Content.Load<Model>(assetName);
            textures = new Texture2D[newModel.Meshes.Count];
            int i = 0;
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (BasicEffect currentEffect in mesh.Effects)
                    textures[i++] = currentEffect.Texture;

            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();

            return newModel;
        }

        private void DrawSkybox()
        {
            SamplerState ss = new SamplerState();
            ss.AddressU = TextureAddressMode.Clamp;
            ss.AddressV = TextureAddressMode.Clamp;
            device.SamplerStates[0] = ss;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            device.DepthStencilState = dss;

            Matrix[] skyboxTransforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(skyboxTransforms);
            int i = 0;
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = skyboxTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(cameraPosition);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(skyboxTextures[i++]);
                }
                mesh.Draw();
            }

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            device.DepthStencilState = dss;
        }

        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
        }
        */
        #endregion



        #region Omar's Methods


        public override void Update(GameTime gameTime)
        {
            Tools3.update_MeasuringVelocityAndAngle(user);
            checkCollision();
            shoot();
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            hasCollidedWithBall = false;
            ballShot = false;
            assumedLegMass = user.AssumedLegMass;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void shoot()
        {
            Vector3 initialLegVelocity; //This variable represents the velocity of the leg with which the user has shot the ball.
            initialLegVelocity = new Vector3((float)(user.Velocity * Math.Cos(user.Angle)), 0, -(float)(user.Velocity * Math.Sin(user.Angle)));
            if (hasCollidedWithBall && !ballShot)
            {
                ballMass = ball.Mass; //get the mass of the ball
                ballShot = true;
                Vector3 velocityAfterCollision = getVelocityAfterCollision(initialLegVelocity); //calculate the velocity of the ball right after the collision
                ball.Velocity = velocityAfterCollision; // update the velocity of the ball
            }
        }

        public void checkCollision()
        {

            Vector3 legPosition; //Current position of leg.
            legPosition = new Vector3((float)user.CurrentRightLegPositionX, 0, (float)user.CurrentRightLegPositionZ);

            if (Math.Abs(Vector3.Subtract(ball.Position, legPosition).Length()) < 150f)
            {
                hasCollidedWithBall = true;
                user.ShootingPosition = legPosition;
            }
            else
                hasCollidedWithBall = false;
        }

        public Vector3 getVelocityAfterCollision(Vector3 initialVelocity)
        {
            double initialVelocityLeg, initialVelocityBall, finalVelocityBall, angle;

            double acceleration = -(friction + wind); //Deceleration of the ball due to resistance.

            //Get the velocity of the ball right before the collision. 
            //If shooting the ball .. initial balls velocity is its current velocity.. else calculate it.
            if (!ballShot)
                initialVelocityBall = Math.Sqrt((ball.InitialVelocity.Length() + (2 * acceleration * Math.Abs(Vector3.Distance(ball.Position, user.ShootingPosition)))));
            else
                initialVelocityBall = ball.Velocity.Length();

            initialVelocityLeg = initialVelocity.Length();

            //Calculate the angle with which the user has shot the ball.
            angle = Math.Atan2(-initialVelocity.Z, initialVelocity.X);

            //Calculate what will the ball's speed be after collision using conservation of momentum equation.
            finalVelocityBall = ((assumedLegMass * initialVelocityLeg) + (ballMass * initialVelocityBall) - (assumedLegMass * (initialVelocityLeg * (1 - ballMass / ball.maxMass)))) / ballMass;

            //Return a vector containing the ball's speed and direction.
            return new Vector3((float)(finalVelocityBall * Math.Cos(angle)), 0, -(float)(finalVelocityBall * Math.Sin(angle)));
        }
        public void ballFallIntoHole()
        {
            //Waiting for completed class Hole from Khaled Salah

            //Get the balls Velocity.
            Vector3 newVelocity = ball.Velocity;

            //Set the balls velocity to the old velocity plus -10j (Downward Motion)
            newVelocity = Vector3.Add(newVelocity, new Vector3(0, -10, 0));

            //Update the balls velocity
            ball.Velocity = newVelocity;
        }
        #endregion
    }
}
