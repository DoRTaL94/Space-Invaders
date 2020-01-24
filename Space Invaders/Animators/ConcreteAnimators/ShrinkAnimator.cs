using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class ShrinkAnimator : SpriteAnimator
     {
          private const string k_Name = "Shrink";
          private const int k_InitialScale = 1;

          public ShrinkAnimator(TimeSpan i_AnimationLength)
               : this(k_Name, i_AnimationLength)
          {
          }

          public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength)
              : base(i_Name, i_AnimationLength)
          {
          }
          
          protected override void DoFrame(GameTime i_GameTime)
          {
               float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;
               float removeFromScale = (k_InitialScale / (float)this.AnimationLength.TotalSeconds) * totalSeconds;
               float newScaleX = MathHelper.Clamp(this.BoundSprite.Scales.X - removeFromScale, 0, 1);
               float newScaleY = MathHelper.Clamp(this.BoundSprite.Scales.Y - removeFromScale, 0, 1);

               this.BoundSprite.Scales = new Vector2(newScaleX, newScaleY);
          }

          protected override void RevertToOriginal()
          {
               this.BoundSprite.Scales = m_OriginalSpriteInfo.Scales;
          }
     }
}
