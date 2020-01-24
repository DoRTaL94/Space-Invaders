using System;
using Space_Invaders.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Animators;
using Space_Invaders.Animators.ConcreteAnimators;

namespace Space_Invaders.Models
{
     public class RedMotherShip : MotherShip
     {
          private const string k_KilledSoundName    = "MotherShipKill";
          private const string k_AssetName          = @"Sprites\MotherShip_32x120";
          private const string k_DeadAnimatorName   = "Dead";
          private const float k_BlinksPerSecond     = 10f;
          private const float k_BlinkLength         = 1 / k_BlinksPerSecond;
          private const float k_DeadAnimationLength = 2.2f;

          public RedMotherShip(GameScreen i_GameScreen)
               : base(k_AssetName, i_GameScreen)
          {
               this.BlendState                   = BlendState.NonPremultiplied;
               this.TintColor                    = Color.Red;
               this.Score                        = 800;
               this.StartVelocity                = new Vector2(100, 0);
               this.Width                        = 120;
               this.Height                       = 32;
               this.StartPosition                = new Vector2(-Width, 32);
               this.RotationOrigin               = new Vector2(this.Width / 2, this.Height / 2);
               this.PausePositionDuringAnimation = true;
               this.KilledSoundName              = k_KilledSoundName;
          }

          public override void Initialize()
          {
               base.Initialize();
               CompositeAnimator deadAnimator = new CompositeAnimator(
                    k_DeadAnimatorName,
                    TimeSpan.FromSeconds(k_DeadAnimationLength),
                    this,
                    new BlinkAnimator(TimeSpan.FromSeconds(k_BlinkLength), TimeSpan.FromSeconds(k_DeadAnimationLength)),
                    new ShrinkAnimator(TimeSpan.FromSeconds(k_DeadAnimationLength)),
                    new TransparencyAnimator(TimeSpan.FromSeconds(k_DeadAnimationLength)));
               deadAnimator.Finished += deadAnimator_Finished;
               this.Animations.Add(deadAnimator);
               this.Animations.Pause();
               this.Animations.Enabled = true;
          }

          private void deadAnimator_Finished(object i_Sender, EventArgs i_Args)
          {
               CompositeAnimator spriteAnimator = i_Sender as CompositeAnimator;
               spriteAnimator.Pause();
               this.Visible = false;
               this.IsDuringAnimation = false;
          }
     }
}
