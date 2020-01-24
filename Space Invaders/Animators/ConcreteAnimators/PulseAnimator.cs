using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class PulseAnimator : SpriteAnimator
     {
          private const string k_Name = "Pulse";
          private bool m_Shrinking;
          private float m_TargetScale;
          private float m_SourceScale;
          private float m_DeltaScale;

          public PulseAnimator(TimeSpan i_AnimationLength, float i_TargetScale, float i_PulsePerSecond)
             : this(k_Name, i_AnimationLength, i_TargetScale, i_PulsePerSecond)
          {
          }

          public PulseAnimator(string i_Name, TimeSpan i_AnimationLength, float i_TargetScale, float i_PulsePerSecond)
             : base(i_Name, i_AnimationLength)
          {
               Scale = i_TargetScale;
               PulsePerSecond = i_PulsePerSecond;
          }

          public float Scale { get; set; }

          public float PulsePerSecond { get; set; }

          protected override void RevertToOriginal()
          {
               this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;

               m_SourceScale = m_OriginalSpriteInfo.Scales.X;
               m_TargetScale = Scale;
               m_DeltaScale = m_TargetScale - m_SourceScale;
               m_Shrinking = m_DeltaScale < 0;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

               if (m_Shrinking)
               {
                    if (this.BoundSprite.Scales.X > m_TargetScale)
                    {
                         this.BoundSprite.Scales -= new Vector2(totalSeconds * 2 * PulsePerSecond * m_DeltaScale);
                    }
                    else
                    {
                         this.BoundSprite.Scales = new Vector2(m_TargetScale);
                         m_Shrinking = false;
                         m_TargetScale = m_SourceScale;
                         m_SourceScale = this.BoundSprite.Scales.X;
                    }
               }
               else
               {
                    if (this.BoundSprite.Scales.X < m_TargetScale)
                    {
                         this.BoundSprite.Scales += new Vector2(totalSeconds * 2 * PulsePerSecond * m_DeltaScale);
                    }
                    else
                    {
                         this.BoundSprite.Scales = new Vector2(m_TargetScale);
                         m_Shrinking = true;
                         m_TargetScale = m_SourceScale;
                         m_SourceScale = this.BoundSprite.Scales.X;
                    }
               }
          }
     }
}
