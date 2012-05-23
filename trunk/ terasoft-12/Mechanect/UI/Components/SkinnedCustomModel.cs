﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
using UI.Cameras;

namespace UI.Components
{
    public class SkinnedCustomModel : CustomModel
    {
        # region Fields

        private SkinningData skinningData;

        private Matrix[] originalBones;
        private Matrix[] boneTransforms;
        private Matrix[] worldTransforms;
        private Matrix[] skinTransforms;

        #endregion

        #region Initialization

        /// <summary>
        /// Holds a 3D model with skeleton and skin.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="model">The model with skin and skeleton.</param>
        /// <param name="position">The position of the model.</param>
        /// <param name="rotation">The rotation of the model.</param>
        /// <param name="scale">The scale of the model</param>
        public SkinnedCustomModel(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(model, position, rotation, scale)
        {
            this.skinningData = model.Tag as SkinningData;

            this.originalBones = new Matrix[skinningData.BindPose.Count];
            this.skinningData.CopyBindPose(originalBones);

            this.boneTransforms = new Matrix[skinningData.BindPose.Count];
            this.skinningData.CopyBindPose(boneTransforms);

            this.worldTransforms = new Matrix[skinningData.BindPose.Count];
            this.skinTransforms = new Matrix[skinningData.BindPose.Count];

            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the model if any achanges occurred to its skin or bones.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        public void Update()
        {
            UpdateWorldTransforms();
            UpdateSkinTransforms();
        }

        /// <summary>
        /// Updates the world view of every bone according to the changes to its parent bone.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void UpdateWorldTransforms()
        {
            Matrix rootTransform = Matrix.Identity;
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningData.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }

        /// <summary>
        /// Updates the changes of the skin according to the changes in the bones.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        private void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningData.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        /// <summary>
        /// Draws the model.
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="camera">Applies the camer's view and projection to the model.</param>        
        public override void Draw(Camera camera)
        {
            Matrix[] bones = skinTransforms;

            // Render the skinned mesh.
            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix world = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y,
                    Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = world;

                    effect.EnableDefaultLighting();
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
        }

        #endregion

        #region Animation

        /// <summary>
        /// moving a specific bone 
        /// </summary>
        /// <remarks>
        /// <para>Author: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="boneName">The name of the bone, same as in the model.</param>
        /// <param name="offset">The number of extra bone information.</param>
        /// <param name="bend">The binding of the bone in 3D.</param>
        public void MoveBone(string boneName, int offset, Vector3 bend)
        {
            int boneId = model.Bones[boneName].Index + offset;
            boneTransforms[boneId] = Matrix.CreateFromYawPitchRoll(bend.X, bend.Y, bend.Z)
                * originalBones[boneId];
        }

        #endregion
    }
}
