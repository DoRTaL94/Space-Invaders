using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Interfaces;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Screens;
using Space_Invaders.Utils;
using Space_Invaders.Components;

namespace Space_Invaders.Managers
{
     public class ScoreManager : GameService, IScoreManager
     {
          private const string k_PlayerNum                                   = "Player{0}  ";
          private const string k_TieMsg                                      = "It's a tie between:";
          private const string k_ScoreString                                 = "P{0} Score: {1}";
          private const string k_ResultScoreString                           = "Player{0} Scored {1} points";
          private const string k_FontAssetName                               = @"Fonts\ComicSansMsFont";
          private const string k_WinnerString                                = @"The Winner is player {0}";
          private const int k_SpaceFactor                                    = 12;
          private const int k_SpaceBetweenScores                             = 10;
          private const int k_LivesStartingY                                 = 10;
          private const int k_StartingXForDraw                               = 10;
          private readonly List<BasePlayer> r_Players                        = new List<BasePlayer>();
          private readonly HashSet<BasePlayer> r_PlayersSetForCheckExistance = new HashSet<BasePlayer>();
          private readonly List<GameScreen> r_GameScreens                    = new List<GameScreen>();
          private SpriteFont m_ComicSansMsFont;
          private IGameSettings m_GameSettings;
          private StrokeSpriteFont m_Result;
          private bool m_Initialize;

          public ScoreManager(Game i_Game)
               : base(i_Game)
          {
               this.Game.Services.AddService(typeof(IScoreManager), this);
               this.UpdateOrder = int.MaxValue;
          }

          public void AddScreen(GameScreen i_GameScreen)
          {
               r_GameScreens.Add(i_GameScreen);
               i_GameScreen.Add(this);
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
               BasePlayer toRemove = null;
               int scoreIndex = 0;

               foreach (BasePlayer player in r_Players)
               {
                    if (player == i_Player)
                    {
                         scoreIndex++;
                         toRemove = player;
                         break;
                    }
               }

               if (toRemove != null)
               {
                    r_Players.Remove(toRemove);
                    r_PlayersSetForCheckExistance.Remove(toRemove);
               }
          }

          public bool IsPlayerAlreadyAdded(BasePlayer i_Player)
          {
               return r_PlayersSetForCheckExistance.Contains(i_Player);
          }

          public override void Initialize()
          {
               if (!m_Initialize)
               {
                    m_ComicSansMsFont = this.Game.Content.Load<SpriteFont>(k_FontAssetName);
                    m_GameSettings = this.Game.Services.GetService(typeof(IGameSettings)) as IGameSettings;
                    base.Initialize();
                    m_Initialize = true;
               }
          }

          public void DrawScores(GameScreen i_GameScreen)
          {
               float yPos = k_LivesStartingY;
               int playerCounter = 1;

               i_GameScreen.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

               for (int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    BasePlayer player = r_Players[i];
                    string scoreString = string.Format(k_ScoreString, playerCounter.ToString(), player.Score.ToString());
                    Vector2 scoresDrawPosition = new Vector2(k_StartingXForDraw, yPos);

                    i_GameScreen.SpriteBatch.DrawString(m_ComicSansMsFont, scoreString, scoresDrawPosition, player.RepresentativeColor);

                    float nextScorePos = k_SpaceFactor + k_SpaceBetweenScores;
                    yPos += nextScorePos;
                    playerCounter++;
               }

               i_GameScreen.SpriteBatch.End();
          }

          public Vector2 ResultPosition { get; set; }

          private bool isSomeoneAlive()
          {
               bool isSomeoneAlive = false;

               for (int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    BasePlayer player = r_Players[i];

                    if (player.Visible || player.IsAlive)
                    {
                         isSomeoneAlive = true;
                         break;
                    }
               }

               return isSomeoneAlive;
          }

          public void ShowResult(GameScreen i_GameScreen)
          {
               if (!isSomeoneAlive() && m_Result == null)
               {
                    int winnerIndex = 0;
                    StringBuilder result = new StringBuilder();
                    HashSet<int> ties    = new HashSet<int>();

                    addPlayerScoreResult(result, winnerIndex);

                    for (int i = 1; i < m_GameSettings.PlayersCount; i++)
                    {
                         BasePlayer player = r_Players[i];

                         if (r_Players[winnerIndex].Score < player.Score)
                         {
                              winnerIndex = i;

                              if(ties.Count > 0)
                              {
                                   ties.Clear();
                              }
                         }
                         else if (r_Players[winnerIndex].Score == player.Score)
                         {
                              ties.Add(i);
                              ties.Add(winnerIndex);
                         }

                         addPlayerScoreResult(result, i);
                    }

                    addResultString(result, ties, winnerIndex);
                    positionResult(result.ToString(), i_GameScreen);
               }
          }

          private void addPlayerScoreResult(StringBuilder i_Result, int i_PlayerIndex)
          {
               i_Result.Append(string.Format(k_ResultScoreString, i_PlayerIndex + 1, r_Players[i_PlayerIndex].Score));
               i_Result.Append(Environment.NewLine);
          }

          private void addResultString(StringBuilder i_Result, HashSet<int> i_PlayersIndicesWithSameScore, int i_WinnerIndex)
          {
               if (i_PlayersIndicesWithSameScore.Count > 0)
               {
                    i_Result.Append(Environment.NewLine);
                    i_Result.Append(k_TieMsg);
                    i_Result.Append(Environment.NewLine);

                    foreach (int playerIndex in i_PlayersIndicesWithSameScore)
                    {
                         i_Result.Append(string.Format(k_PlayerNum, playerIndex + 1));
                    }
               }
               else if (m_GameSettings.PlayersCount > 1)
               {
                    i_Result.Append(string.Format(k_WinnerString, i_WinnerIndex + 1));
               }
          }

          private void positionResult(string i_ResultText, GameScreen i_GameScreen)
          {
               m_Result = new StrokeSpriteFont(i_ResultText, i_GameScreen);
               m_Result.Initialize();
               m_Result.PositionOrigin = m_Result.SourceRectangleCenter;
               m_Result.RotationOrigin = m_Result.SourceRectangleCenter;
               m_Result.Position = ResultPosition;
               i_GameScreen.Add(m_Result);
          }
     }
}
