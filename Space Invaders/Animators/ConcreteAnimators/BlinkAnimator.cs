using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Models.BaseModels;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class BlinkAnimator : SpriteAnimator
     {
          private const string k_Name = "Blink";
          private readonly TimeSpan r_BlinkLength;
          private TimeSpan m_TimeLeftForNextBlink;

          public BlinkAnimator(string i_Name, TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
               : base(i_Name, i_AnimationLength)
          {
               r_BlinkLength = i_BlinkLength;
               this.m_TimeLeftForNextBlink = i_BlinkLength;
          }

          public BlinkAnimator(TimeSpan i_BlinkLength, TimeSpan i_AnimationLength)
               : this(k_Name, i_BlinkLength, i_AnimationLength)
          {
               r_BlinkLength = i_BlinkLength;
               this.m_TimeLeftForNextBlink = i_BlinkLength;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
          
               if (m_TimeLeftForNextBlink.TotalSeconds < 0)
               {
                    this.BoundSprite.Visible = !this.BoundSprite.Visible;
                    m_TimeLeftForNextBlink = r_BlinkLength;
               }
          }

          protected override void RevertToOriginal()
          {
               BasePlayer player = this.BoundSprite as BasePlayer;

               if (player != null && !player.IsAlive)
               {
                    player.Visible = false;
               }
               else
               {
                    this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
               }

               m_TimeLeftForNextBlink = r_BlinkLength;
          }
     }
}
