using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Animators.ConcreteAnimators;
using Space_Invaders.Animators;

namespace Space_Invaders.Models
{
     public class Player : ShooterPlayer, ICollidable2D
     {
          private const int k_ScoreLostOnDestroyed    = 1200;
          private const int k_RotationPerSeconed      = 4;
          private const float k_BlinksPerSecond       = 6f;
          private const float k_BlinkLength           = 1 / k_BlinksPerSecond;
          private const float k_BlinksAnimationLength = 2.5f;
          private const float k_DeadAnimationLength   = 2.5f;
          private const string k_DeadAnimatorName     = "Dead";
          private const string k_BlinkAnimatorName    = "Blink";
          private const string k_LifeLostSoundName    = "LifeDie";

          public Player(string i_AssetName, GameScreen i_GameScreen)
               : base(i_AssetName, i_GameScreen)
          {
               this.BlendState          = BlendState.NonPremultiplied;
               this.Lives               = 3;
               this.Score               = 0;
               this.Width               = 32;
               this.Height              = 32;
               this.ViewDirection       = Sprite.Up;
               this.GroupRepresentative = this;
               this.RotationOrigin      = new Vector2(this.Width / 2, this.Height / 2);
               this.LifeLostSoundName   = k_LifeLostSoundName;
               this.KilledSoundName     = k_LifeLostSoundName;
          }

          public override void Initialize()
          {
               base.Initialize();

               BlinkAnimator lostLife = new BlinkAnimator(TimeSpan.FromSeconds(k_BlinkLength), TimeSpan.FromSeconds(k_BlinksAnimationLength));
               CompositeAnimator dead = new CompositeAnimator(
                    k_DeadAnimatorName,
                    TimeSpan.FromSeconds(k_DeadAnimationLength),
                    this,
                    new RotationAnimator(k_RotationPerSeconed, TimeSpan.FromSeconds(k_DeadAnimationLength)),
                    new TransparencyAnimator(TimeSpan.FromSeconds(k_DeadAnimationLength)));

               dead.Finished += dead_Finished;
               lostLife.Finished += lostLife_Finished;

               this.Animations.Add(dead);
               this.Animations.Add(lostLife);
               this.Animations.Pause();
               this.Animations.Enabled = true;
          }

          private void lostLife_Finished(object i_Sender, EventArgs i_Args)
          {
               SpriteAnimator animator = i_Sender as SpriteAnimator;
               animator.Pause();
          }

          private void dead_Finished(object i_Sender, EventArgs i_Args)
          {
               CompositeAnimator animator = i_Sender as CompositeAnimator;
               animator.Pause();
               this.Visible = false;
               this.Enabled = false;
          }

          protected override void OnCollidedWithBullet(BaseBullet i_Bullet)
          {
               if (Lives > 0)
               {
                    Lives--;

                    if (Lives == 0)
                    {
                         executeDeadAnimation();
                    }
                    else if (Lives > 0)
                    {
                         executeLostLifeAnimation();
                    }

                    updateScore();
               }
          }

          private void updateScore()
          {
               if (Score >= k_ScoreLostOnDestroyed)
               {
                    Score -= k_ScoreLostOnDestroyed;
               }
               else
               {
                    Score = 0;
               }
          }

          private void executeDeadAnimation()
          {
               this.Animations[k_BlinkAnimatorName].Pause();
               this.Visible = true;
               this.Animations[k_DeadAnimatorName].Resume();
          }

          private void executeLostLifeAnimation()
          {
               Position = StartPosition;
               this.Animations[k_BlinkAnimatorName].Restart();
          }

          public object GroupRepresentative { get; set; }
     }
}
