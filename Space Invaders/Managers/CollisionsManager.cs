using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Space_Invaders.Components;
using Space_Invaders.Interfaces;
using Space_Invaders.Models;

namespace Space_Invaders.Managers
{
     public class CollisionsManager : GameService, ICollisionsManager
     {
          protected readonly List<ICollidable> r_Collidables = new List<ICollidable>();

          public CollisionsManager(Game i_Game)
               : base(i_Game, int.MaxValue)
          { 
          }

          protected override void RegisterAsService()
          {
               this.Game.Services.AddService(typeof(ICollisionsManager), this);
          }

          public void AddObjectToMonitor(ICollidable i_Collidable)
          {
               if (!this.r_Collidables.Contains(i_Collidable))
               {
                    this.r_Collidables.Add(i_Collidable);
                    i_Collidable.PositionChanged += collidable_Changed;
                    i_Collidable.SizeChanged += collidable_Changed;
                    i_Collidable.VisibleChanged += collidable_Changed;
                    i_Collidable.Disposed += collidable_Disposed;
               }
          }

          private void collidable_Disposed(object i_Sender, EventArgs i_Args)
          {
               ICollidable collidable = i_Sender as ICollidable;

               if (collidable != null
                   &&
                   this.r_Collidables.Contains(collidable))
               {
                    collidable.PositionChanged -= collidable_Changed;
                    collidable.SizeChanged -= collidable_Changed;
                    collidable.VisibleChanged -= collidable_Changed;
                    collidable.Disposed -= collidable_Disposed;

                    r_Collidables.Remove(collidable);
               }
          }

          private void collidable_Changed(object i_Sender, EventArgs i_Args)
          {
               if (i_Sender is ICollidable)
               {
                    Entity entity = i_Sender as Entity;
                    bool isEntity = entity != null;
                    bool isEntityAlive = isEntity && entity.IsAlive;

                    if (!isEntity || isEntityAlive)
                    {
                         checkCollision(i_Sender as ICollidable);
                    }
               }
          }

          private void checkCollision(ICollidable i_Source)
          {
               if (i_Source.Visible)
               {
                    List<ICollidable> collidedComponents = new List<ICollidable>();

                    foreach (ICollidable target in r_Collidables)
                    {
                         Entity entity = target as Entity;
                         bool isEntity = entity != null;
                         bool isEntityAlive = isEntity && entity.IsAlive;

                         if ((i_Source != target && !isEntity && target.Visible) || isEntityAlive)
                         {
                              if (target.CheckCollision(i_Source))
                              {
                                   if (isPixelCollided(target, i_Source))
                                   {
                                        collidedComponents.Add(target);
                                   }
                              }
                         }
                    }

                    foreach (ICollidable target in collidedComponents)
                    {
                         target.Collided(i_Source);
                         i_Source.Collided(target);
                    }
               }
          }

          private bool isPixelCollided(ICollidable i_Target, ICollidable i_Source)
          {
               bool pixelCollided = false;

               Sprite target = i_Target as Sprite;
               Sprite source = i_Source as Sprite;

               if (target != null && source != null)
               {
                    Rectangle targetIntersectedRect; 
                    Rectangle sourceIntersectedRect; 
                    GetIntersectedRect(target, source, out targetIntersectedRect);
                    GetIntersectedRect(source, target, out sourceIntersectedRect);

                    int height = MathHelper.Min(targetIntersectedRect.Height, sourceIntersectedRect.Height);
                    int width = MathHelper.Min(targetIntersectedRect.Width, sourceIntersectedRect.Width);

                    for (int row = 0; row < height && !pixelCollided; row++)
                    {
                         for (int col = 0; col < width && !pixelCollided; col++)
                         {
                              int targetRow = targetIntersectedRect.Y + row;
                              int targetCol = targetIntersectedRect.X + col;
                              int sourceRow = sourceIntersectedRect.Y + row;
                              int sourceCol = sourceIntersectedRect.X + col;
                              Color targetColor = Color.White;
                              Color sourceColor = Color.White;

                              if (targetRow < target.TexturePixels.Rows && targetCol < target.TexturePixels.Cols)
                              {
                                   targetColor = target.TexturePixels[targetRow, targetCol];
                              }

                              if (sourceRow < source.TexturePixels.Rows && sourceCol < source.TexturePixels.Cols)
                              {
                                   sourceColor = source.TexturePixels[sourceRow, sourceCol];
                              }

                              if (targetColor.A != 0 && sourceColor.A != 0)
                              {
                                   pixelCollided = true;
                              }
                         }
                    }
               }

               return pixelCollided;
          }

          public void GetIntersectedRect(Sprite i_First, Sprite i_Second, out Rectangle o_IntersectedRect)
          {
               o_IntersectedRect = new Rectangle();
               Rectangle first = new Rectangle((int)i_First.Position.X, (int)i_First.Position.Y, i_First.SourceRectangle.Width, i_First.SourceRectangle.Height);
               Rectangle second = new Rectangle((int)i_Second.Position.X, (int)i_Second.Position.Y, i_Second.SourceRectangle.Width, i_Second.SourceRectangle.Height);

               Rectangle.Intersect(ref first, ref second, out o_IntersectedRect);
               o_IntersectedRect.X -= first.X;
               o_IntersectedRect.Y -= first.Y;
          }

          public bool IsCollideWithWindowEdge(Sprite i_Sprite)
          {
               return IsCollideWithWindowLeftEdge(i_Sprite) ||
                    IsCollideWithWindowRightEdge(i_Sprite) ||
                    IsCollideWithWindowTopEdge(i_Sprite) ||
                    IsCollideWithWindowBottomEdge(i_Sprite);
          }

          public bool IsCollideWithWindowBottomEdge(Sprite i_Sprite)
          {
               return i_Sprite.Position.Y + i_Sprite.Height >= this.Game.GraphicsDevice.Viewport.Height;
          }

          public bool IsCollideWithWindowTopEdge(Sprite i_Sprite)
          {
               return i_Sprite.Position.Y <= 0;
          }

          public bool IsCollideWithWindowRightEdge(Sprite i_Sprite)
          {
               return i_Sprite.Position.X + i_Sprite.Width >= this.Game.GraphicsDevice.Viewport.Width;
          }

          public bool IsCollideWithWindowLeftEdge(Sprite i_Sprite)
          {
               return i_Sprite.Position.X <= 0;
          }
     }
}
