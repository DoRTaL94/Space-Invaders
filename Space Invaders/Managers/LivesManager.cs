using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Interfaces;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Screens;

namespace Space_Invaders.Managers
{
     public class LivesManager : DrawableGameComponent, ILivesManager
     {
          public event Action AllPlayersDied;

          private const int k_SpaceBetweenLives                              = 10;
          private const int k_LivesStartingY                                 = 10;
          private const float k_LivesAlpha                                   = 0.5f;
          private readonly List<BasePlayer> r_Players                        = new List<BasePlayer>();
          private readonly HashSet<BasePlayer> r_PlayersSetForCheckExistance = new HashSet<BasePlayer>();
          private readonly Vector2 r_LivesScale                              = new Vector2(0.5f, 0.5f);
          private readonly Color r_Color                                     = new Color(Color.White, k_LivesAlpha);
          private readonly GameScreen r_GameScreen;
          private bool m_IsAllPlayerAlive = false;
          private IGameSettings m_GameSettings;

          public LivesManager(GameScreen i_GameScreen)
               : base(i_GameScreen.Game)
          {
               this.Game.Services.AddService(typeof(ILivesManager), this);
               r_GameScreen = i_GameScreen;
               i_GameScreen.Add(this);
               this.DrawOrder = this.UpdateOrder = int.MaxValue;
          }

          public void AddPlayer(BasePlayer i_Player)
          {
               if (!IsPlayerAlreadyAdded(i_Player))
               {
                    r_Players.Add(i_Player);
                    r_PlayersSetForCheckExistance.Add(i_Player);
               }
          }

          public void RemovePlayer(BasePlayer i_Player)
          {
               if (IsPlayerAlreadyAdded(i_Player))
               {
                    r_Players.Remove(i_Player);
               }
          }

          public bool IsPlayerAlreadyAdded(BasePlayer i_Player)
          {
               return r_PlayersSetForCheckExistance.Contains(i_Player);
          }

          public override void Initialize()
          {
               m_GameSettings = this.Game.Services.GetService(typeof(IGameSettings)) as IGameSettings;
               base.Initialize();
          }

          public override void Update(GameTime i_GameTime)
          {
               for (int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    BasePlayer player = r_Players[i];

                    if (player.IsAlive || player.Visible)
                    {
                         m_IsAllPlayerAlive = true;
                         break;
                    }
               }

               if (!m_IsAllPlayerAlive && AllPlayersDied != null)
               {
                    AllPlayersDied.Invoke();
               }

               m_IsAllPlayerAlive = false;

               base.Update(i_GameTime);
          }

          public override void Draw(GameTime i_GameTime)
          {
               float yPos = k_LivesStartingY;
               r_GameScreen.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

               for (int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    BasePlayer player = r_Players[i];

                    for (int life = 1; life <= player.Lives; life++)
                    {
                         float xPos = this.Game.GraphicsDevice.Viewport.Width - (life * player.Width);
                         Vector2 positionForDraw = new Vector2(xPos, yPos);

                         r_GameScreen.SpriteBatch.Draw(
                              player.Texture, 
                              positionForDraw, 
                              player.SourceRectangle, 
                              r_Color,
                              0, 
                              Vector2.Zero,
                              r_LivesScale, 
                              SpriteEffects.None, 
                              player.LayerDepth);
                    }

                    float nextLivesPos = (player.Height * r_LivesScale.Y) + k_SpaceBetweenLives;
                    yPos += nextLivesPos;
               }

               r_GameScreen.SpriteBatch.End();
               base.Draw(i_GameTime);
          }
     }
}
