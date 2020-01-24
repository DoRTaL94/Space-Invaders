using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Utils;
using Space_Invaders.Managers;
using Space_Invaders.Screens;
using Space_Invaders.Models.BaseModels;

namespace Space_Invaders.Models
{
     public class Bullet : BaseBullet
     {
          private const string k_GraphicPath             = @"Sprites\Bullet";
          private const int k_RandomFactor               = 1;
          private const int k_MaxRandom                  = 5;
          private const int k_MinRandom                  = 0;
          private const float k_DestroyBarrierPercentage = 0.7f;
          private const float k_YVelocity                = 160;
          private const float k_XVelocity                = 0;
          private readonly IRandomBehavior r_RandomBehavior;

          public Bullet(GameScreen i_GameScreen) 
               : this(Color.Red, i_GameScreen)
          {
          }

          public Bullet(Color i_TintColor, GameScreen i_GameScreen) 
               : base(k_GraphicPath, i_GameScreen)
          {
               r_RandomBehavior = this.Game.Services.GetService(typeof(IRandomBehavior)) as IRandomBehavior;

               this.Width     = 6;
               this.Height    = 16;
               this.Enabled   = false;
               this.Visible   = false;
               this.TintColor = i_TintColor;
               this.Velocity  = new Vector2(k_XVelocity, k_YVelocity);
          }

          public override void Collided(ICollidable i_Collidable)
          {
               Barrier barrier = i_Collidable as Barrier;

               if (barrier != null)
               {
                    handleBarrierCollision(barrier);
               }
               else if (i_Collidable is Bullet && i_Collidable.GroupRepresentative is BasePlayer && this.GroupRepresentative is EnemyManager)
               {
                    if (r_RandomBehavior.Roll(k_RandomFactor, k_MinRandom, k_MaxRandom))
                    {
                         base.Collided(i_Collidable);
                    }
               }
               else
               {
                    base.Collided(i_Collidable);
               }
          }

          private void handleBarrierCollision(Barrier i_Barrier)
          {
               this.Enabled = false;

               Texture2DPixels barrierPixels = i_Barrier.TexturePixels;
               int maxYPixelsToDestroy       = (int)(this.Height * k_DestroyBarrierPercentage);
               int yDirection                = this.MoveDirection == Sprite.Down ? (int)Sprite.Down.Y : (int)Sprite.Up.Y;
               int bulletStartPixelY         = this.MoveDirection == Sprite.Down ? (int)this.Position.Y - 2 : (int)this.Position.Y + 2;
               int bulletStartPixelX         = (int)this.Position.X;
               int barrierStartPixelY        = (int)i_Barrier.Position.Y;
               int barrierStartPixelX        = (int)i_Barrier.Position.X;
               int maxXPixelsToDestory       = bulletStartPixelX + (int)this.Width;
               int pixelsToDestroyIfUp       = bulletStartPixelY - maxYPixelsToDestroy;
               int pixelsToDestroyIfDown     = bulletStartPixelY + maxYPixelsToDestroy;

               for (int currY = bulletStartPixelY; currY < pixelsToDestroyIfDown && currY >= pixelsToDestroyIfUp; currY += yDirection)
               {
                    for(int currX = bulletStartPixelX; currX < maxXPixelsToDestory; currX++)
                    {
                         int barrierY = currY - barrierStartPixelY;
                         int barrierX = currX - barrierStartPixelX;
                         
                         barrierX = barrierX < 0 ? barrierX + (int)this.Width - 1 : barrierX;

                         if (this.MoveDirection == Sprite.Down)
                         {
                              barrierY = barrierY + (int)this.Height - 1;
                         }

                         barrierY = (int)MathHelper.Clamp(barrierY, 0, i_Barrier.Height - 1);
                         barrierX = (int)MathHelper.Clamp(barrierX, 0, i_Barrier.Width - 1);

                         if (barrierPixels[barrierY, barrierX].A != 0)
                         {
                              barrierPixels[barrierY, barrierX] = new Color(barrierPixels[barrierY, barrierX], 0);
                         }
                    }
               }

               i_Barrier.Texture.SetData(i_Barrier.TexturePixels.Pixels);
               this.Visible = false;
          }
     }
}