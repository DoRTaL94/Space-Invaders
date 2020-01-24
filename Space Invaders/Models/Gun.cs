using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Models.BaseModels;

namespace Space_Invaders.Models
{
     public class Gun : BaseGun
     {
          public Gun(int i_Capacity, Sprite i_Shooter, Action<ICollidable> i_ExecuteOnBulletCollided = null) 
               : base(i_Shooter, i_ExecuteOnBulletCollided)
          {
               this.Capacity = i_Capacity;
               this.BulletType = typeof(Bullet);
          }

          protected override void InitializeBullets()
          {
               ConstructorInfo[] constructors = BulletType.GetConstructors();

               for (int i = 0; i < Capacity; i++)
               {
                    BaseBullet bullet = constructors[0].Invoke(new object[] { r_Shooter.GameScreen }) as BaseBullet;
                    this.AddBullet(bullet);

                    if (r_Shooter is Player)
                    {
                         bullet.TintColor = Color.Red;
                    }
                    else if (r_Shooter is Enemy)
                    {
                         bullet.TintColor = Color.Blue;
                    }

                    if (BulletsVelocity == Vector2.Zero)
                    {
                         BulletsVelocity = bullet.Velocity;
                    }
               }
          }
     }
}