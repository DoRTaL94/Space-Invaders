using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Space_Invaders.Interfaces;

namespace Space_Invaders.Components
{
     public abstract class LoadableDrawableComponent : DrawableGameComponent
     {
          public event EventHandler<EventArgs> SizeChanged;

          public event EventHandler<EventArgs> PositionChanged;

          public event EventHandler<EventArgs> Disposed;

          protected string m_AssetName;

          public LoadableDrawableComponent(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
               : base(i_Game)
          {
               this.AssetName = i_AssetName;
               this.UpdateOrder = i_UpdateOrder;
               this.DrawOrder = i_DrawOrder;
          }

          public LoadableDrawableComponent(string i_AssetName, Game i_Game, int i_CallsOrder)
              : this(i_AssetName, i_Game, i_CallsOrder, i_CallsOrder)
          { 
          }

          protected virtual void OnDisposed(object i_Sender, EventArgs i_Args)
          {
               if (Disposed != null)
               {
                    Disposed.Invoke(i_Sender, i_Args);
               }
          }

          protected override void Dispose(bool i_Disposing)
          {
               base.Dispose(i_Disposing);
               OnDisposed(this, EventArgs.Empty);
          }

          protected ContentManager ContentManager
          {
               get { return this.Game.Content; }
          }

          protected virtual void OnPositionChanged()
          {
               if (PositionChanged != null)
               {
                    PositionChanged(this, EventArgs.Empty);
               }
          }

          protected virtual void OnSizeChanged()
          {
               if (SizeChanged != null)
               {
                    SizeChanged(this, EventArgs.Empty);
               }
          }

          public string AssetName
          {
               get { return m_AssetName; }
               set { m_AssetName = value; }
          }

          public override void Initialize()
          {
               base.Initialize();

               if (this is ICollidable)
               {
                    ICollisionsManager collisionMgr =
                        this.Game.Services.GetService(typeof(ICollisionsManager))
                            as ICollisionsManager;

                    if (collisionMgr != null)
                    {
                         collisionMgr.AddObjectToMonitor(this as ICollidable);
                    }
               }
               
               InitBounds(); 
          }

          protected abstract void InitBounds();

          public bool IsInitialized { get; set; }
     }
}