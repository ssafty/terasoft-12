﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using UI.Animation;
using Physics;

namespace Mechanect.Exp3
{
    public class BallAnimation : ModelLinearAnimation
    {

        private Ball ball;
        private bool willFall;
        private int fallFactor;

        public BallAnimation(Ball ball, Vector3 velocity, float friction, Vector3 holePosition, float holeRadius)
            : base(ball, velocity, friction, LinearMotion.CalculateTime(velocity.Length(), 0, friction))
        {
            this.ball = ball;
            Vector3 totalDisplacement = LinearMotion.CalculateDisplacement(velocity, friction, Duration);
            Vector3 stopPosition = StartPosition + totalDisplacement;


            if (Vector3.Distance(stopPosition, holePosition) < (ball.Radius + holeRadius))
            {
                willFall = true;
            }

        }
        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);
            ball.Rotate(Displacement);

            if (!base.Finished())
            {
                if (willFall)
                {
                    if (ElapsedTime > Duration - TimeSpan.FromSeconds(3.2))
                    {
                        if (fallFactor < 120)
                        {
                            ball.SetHeight(ball.Position.Y - ball.Radius - (fallFactor * 0.15f));
                            fallFactor++;
                        }
                        else
                        {
                            ball.SetHeight(ball.Position.Y - ball.Radius - (fallFactor * 0.15f));
                        }
                    }
                }
            }
        }

    }
}
