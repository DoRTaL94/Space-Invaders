using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Space_Invaders.Components;
using Space_Invaders.Interfaces;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Screens;
using Space_Invaders.Models;

namespace Space_Invaders.Managers
{
     public class PlayersManager : GameService, IPlayersManager
     {
          public event Action AllPlayersDied;

          public event Action<BasePlayer, ICollidable2D> PlayerCollided;

          private const int k_RightSpace = 64;
          private const int k_DownSpace  = 64;
          private readonly List<BasePlayer> r_Players;
          private readonly GameScreen r_GameScreen;
          private readonly ISoundManager r_SoundManager;
          private IGameSettings m_GameSettings;
          private IScoreManager m_ScoreManager;
          private LivesManager m_LivesManager;
          private bool m_IsMouseControlTaken;

          public PlayersManager(GameScreen i_GameScreen)
               : base(i_GameScreen.Game)
          {
               r_GameScreen = i_GameScreen;
               r_Players = new List<BasePlayer>();
               r_SoundManager = this.Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
               NextPosition = new Vector2(r_GameScreen.Game.GraphicsDevice.Viewport.Width - k_RightSpace, r_GameScreen.Game.GraphicsDevice.Viewport.Height - k_DownSpace);
               initServices();

               m_GameSettings.PlayersCountChanged += gameSettings_PlayersCountChanged;
               this.Game.Services.AddService(typeof(IPlayersManager), this);
          }

          private Vector2 NextPosition { get; set; }

          public Type PlayerType { get; set; } = typeof(Player);

          public int PlayersCount { get; set; }

          public BasePlayer this[int i_Index]
          {
               get
               {
                    BasePlayer player = null;

                    if (i_Index >= 0 && i_Index < r_Players.Count)
                    {
                         player = r_Players[i_Index];
                    }

                    return player;
               }
          }

          public void AddPlayer(string i_AssetName)
          {
               ConstructorInfo[] constructorsInfo = PlayerType.GetConstructors();
               BasePlayer player = null;

               foreach (ConstructorInfo constructor in constructorsInfo)
               {
                    ParameterInfo[] parameterInfo = constructor.GetParameters();

                    if (parameterInfo.Length == 2)
                    {
                         if (parameterInfo[0].ParameterType == typeof(string) && parameterInfo[1].ParameterType == typeof(GameScreen))
                         {
                              player = constructor.Invoke(new object[] { i_AssetName, r_GameScreen }) as BasePlayer;
                         }
                    }
               }

               if (player != null)
               {
                    AddPlayer(player);
               }
          }

          public void AddPlayer(BasePlayer i_Player)
          {
               i_Player.PlayerCollided += player_Collided;
               i_Player.StartPosition = NextPosition;
               NextPosition -= new Vector2(i_Player.Width, 0);
               m_LivesManager.AddPlayer(i_Player);
               m_ScoreManager.AddPlayer(i_Player);
               r_GameScreen.Add(i_Player);
               r_Players.Add(i_Player);
               PlayersCount++;
               r_SoundManager.AddSoundEmitter(i_Player);

               if (PlayersCount > m_GameSettings.PlayersCount)
               {
                    i_Player.Visible = false;
                    i_Player.Enabled = false;
               }

               setMouseControl(i_Player);
          }

          private void player_Collided(BasePlayer i_Player, ICollidable2D i_CollidedWith)
          {
               if (PlayerCollided != null)
               {
                    PlayerCollided.Invoke(i_Player, i_CollidedWith);
               }
          }

          public BasePlayer GetLastAddedPlayer()
          {
               BasePlayer player = null;

               if (r_Players.Count > 0)
               {
                    player = r_Players[r_Players.Count - 1];
               }

               return player;
          }

          private void setMouseControl(BasePlayer i_Player)
          {
               if (!m_IsMouseControlTaken)
               {
                    i_Player.IsMouseControllable = !m_IsMouseControlTaken;
                    m_IsMouseControlTaken = !m_IsMouseControlTaken;
               }
          }

          private void initServices()
          {
               m_LivesManager = new LivesManager(r_GameScreen);
               m_ScoreManager = this.Game.Services.GetService(typeof(IScoreManager)) as IScoreManager;
               m_GameSettings = this.Game.Services.GetService(typeof(IGameSettings)) as IGameSettings;
               m_LivesManager.AllPlayersDied += livesManager_AllPlayersDied;
          }

          private void livesManager_AllPlayersDied()
          {
               AllPlayersDied.Invoke();
          }

          private void gameSettings_PlayersCountChanged(object i_Sender, EventArgs i_Args)
          {
               for (int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    if (!r_Players[i].Visible)
                    {
                         r_Players[i].Visible = true;
                         r_Players[i].Enabled = true;
                    }
               }
          }

          public void LevelReset()
          {
               for(int i = 0; i < m_GameSettings.PlayersCount; i++)
               {
                    r_Players[i].ResetProperties();
               }
          }

          public void ResetAll()
          {
               LevelReset();

               foreach(BasePlayer player in r_Players)
               {
                    player.Score = player.StartingScore;
               }
          }
     }
}
