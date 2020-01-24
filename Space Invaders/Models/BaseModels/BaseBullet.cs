using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;

namespace Space_Invaders.Models.BaseModels
{
     public abstract class BaseBullet : Sprite, ICollidable2D
     {
          public event EventHandler LeftWindowBounds;

          public event Action<ICollidable> CollidedWithSprite;

          protected BaseBullet(string i_GraphicPath, GameScreen i_GameScreen) 
               : base(i_GraphicPath, i_GameScreen)
          {
               this.LeftWindowBounds += OnLeftBounds;
          }

          public override void Initialize()
          {
               base.Initialize();
               this.Position = new Vector2(-Width, -Height);
          }

          public object GroupRepresentative { get; set; }

          protected virtual void OnLeftBounds(object i_Sender, EventArgs i_Args)
          {
          }

          public override void Update(GameTime i_GameTime)
          {
               base.Update(i_GameTime);

               if (LeftWindowBounds != null)
               {
                    if (this.Position.Y > this.Game.GraphicsDevice.Viewport.Height ||
                         this.Position.Y < 0 ||
                         this.Position.X > this.Game.GraphicsDevice.Viewport.Width ||
                         this.Position.X < 0)
                    {
                         LeftWindowBounds.Invoke(this, null);
                    }
               }
          }

          public override void Collided(ICollidable i_Collidable)
          {
               this.Enabled = false;
               this.Visible = false;
               this.Position = Vector2.Zero;

               if (CollidedWithSprite != null)
               {
                    CollidedWithSprite.Invoke(i_Collidable);
               }
          }
     }
}