using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;

namespace Space_Invaders.Models
{
     public abstract class MotherShip : Enemy
     {
          private readonly IRandomBehavior r_RandomBehavior;
          private readonly int r_MaxLives;

          public MotherShip(string i_AssetName, GameScreen i_GameScreen) 
               : base(i_AssetName, i_GameScreen)
          {
               r_RandomBehavior   = this.Game.Services.GetService(typeof(IRandomBehavior)) as IRandomBehavior;
               r_MaxLives         = Lives;
               this.MoveDirection = Sprite.Right;
               this.Visible       = false;
          }

          protected bool IsDuringAnimation { get; set; } = false;

          protected bool PausePositionDuringAnimation { get; set; } = false;

          protected override void OnUpdate(float i_TotalSeconds)
          {
               if (!IsDuringAnimation || !PausePositionDuringAnimation)
               {
                    if (Visible && IsAlive)
                    {
                         m_Position += MoveDirection * Velocity * i_TotalSeconds;

                         if (isCollideWithRightBound())
                         {
                              Visible = false;
                         }
                    }
                    else if (!Visible)
                    {
                         trySpawn();
                    }
               }
          }

          private void trySpawn()
          {
               if (r_RandomBehavior.Roll(1, 0, 500))
               {
                    m_Position.X = -Width;
                    Visible = true;

                    if (Lives < r_MaxLives)
                    {
                         Lives++;
                    }
               }
          }

          private bool isCollideWithRightBound()
          {
               return m_Position.X >= Game.GraphicsDevice.Viewport.Width;
          }

          public override void Collided(ICollidable i_Collidable)
          {
               if (Lives > 0)
               {
                    this.IsDuringAnimation = true;
                    this.Lives--;
                    this.Animations.Restart();
               }
          }
     }
}
