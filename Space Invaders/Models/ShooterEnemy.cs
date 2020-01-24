using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Utils;
using Space_Invaders.Screens;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Managers;
using Space_Invaders.Animators;

namespace Space_Invaders.Models
{
     public abstract class ShooterEnemy : Enemy
     {
          private const string k_DeadAnimationName = "Dead";
          private readonly Rectangle r_SourceRectangle;
          private readonly ICollisionsManager r_CollisionsManager;
          private readonly ISoundManager r_SoundManager;
          private BaseGun m_Gun;

          protected ShooterEnemy(string i_AssetName, Rectangle i_SourceRectangle, GameScreen i_GameScreen) 
               : base(i_AssetName, i_GameScreen)
          {
               r_SourceRectangle = i_SourceRectangle;
               r_CollisionsManager = this.Game.Services.GetService(typeof(ICollisionsManager)) as ICollisionsManager;
               r_SoundManager = this.Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
          }

          public BaseGun Gun
          {
               get
               {
                    return m_Gun;
               }

               set
               {
                    if (m_Gun != null)
                    {
                         r_SoundManager.RemoveSoundEmitter(m_Gun);
                    }

                    m_Gun = value;
                    r_SoundManager.AddSoundEmitter(m_Gun);
               }
          }

          public void Shoot()
          {
               Gun.Shoot();
          }

          public override void ResetProperties()
          {
               base.ResetProperties();
               this.Gun.Reset();
               this.MoveDirection = Sprite.Right;
          }

          public int MaxShotsInMidAir
          {
               get
               {
                    return this.Gun.Capacity;
               }

               set
               {
                    this.Gun.Capacity = value;
               }
          }

          public override void Collided(ICollidable i_Collidable)
          {
               bool amIAliveAndCollidedWithBullet = this.IsAlive && i_Collidable.GroupRepresentative != this.GroupRepresentative && i_Collidable is Bullet;

               if (amIAliveAndCollidedWithBullet)
               {
                    onCollidedWithBullet(i_Collidable);
               }
               else if (i_Collidable is Barrier)
               {
                    onCollidedWithBarrier(i_Collidable);
               }
          }

          private void onCollidedWithBullet(ICollidable I_Collidable)
          {
               SpriteAnimator deadAnimator = this.Animations[k_DeadAnimationName];

               if (deadAnimator != null)
               {
                    deadAnimator.Restart();
               }

               this.Lives--;
          }

          private void onCollidedWithBarrier(ICollidable i_Collidable)
          {
               Barrier barrier = i_Collidable as Barrier;
               Texture2DPixels barrierPixels = barrier.TexturePixels;
               Rectangle intersectedRect;

               r_CollisionsManager.GetIntersectedRect(barrier, this, out intersectedRect);

               for (int y = 0; y < intersectedRect.Height; y++)
               {
                    for (int x = 0; x < intersectedRect.Width; x++)
                    {
                         int currY = intersectedRect.Y + y;
                         int currX = intersectedRect.X + x;

                         if (currY >= 0 && currY < barrierPixels.Rows &&
                              currX >= 0 && currX < barrierPixels.Cols)
                         {
                              barrierPixels[currY, currX] = new Color(barrierPixels[currY, currX], 0);
                         }
                    }
               }

               barrier.Texture.SetData(barrierPixels.Pixels);
          }

          protected override void InitSourceRectangle()
          {
               this.SourceRectangle = r_SourceRectangle;
          }
     }
}
