using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class JumpMovementAnimator : SpriteAnimator
     {
          private const string k_Name = "JumpMovement";
          private readonly TimeSpan r_JumpIntervals;
          private TimeSpan m_TimeUntilNextJump;

          public JumpMovementAnimator(string i_Name, TimeSpan i_AnimationLength) 
               : base(i_Name, i_AnimationLength)
          {
          }

          public JumpMovementAnimator(TimeSpan i_JumpsIntervals, TimeSpan i_AnimationLength)
               : this(k_Name, i_AnimationLength)
          {
               m_TimeUntilNextJump = r_JumpIntervals = i_JumpsIntervals;
          }

          protected override void RevertToOriginal()
          {
               m_TimeUntilNextJump = r_JumpIntervals;
               this.BoundSprite.Position = m_OriginalSpriteInfo.Position;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               if (m_TimeUntilNextJump != TimeSpan.Zero)
               {
                    m_TimeUntilNextJump -= i_GameTime.ElapsedGameTime;

                    if (m_TimeUntilNextJump.TotalSeconds <= 0)
                    {
                         m_TimeUntilNextJump = r_JumpIntervals;
                         jump();
                    }
               }
          }

          private void jump()
          {
               this.BoundSprite.Position += (this.BoundSprite.Velocity * this.BoundSprite.MoveDirection) / 2;
          }
     }
}
