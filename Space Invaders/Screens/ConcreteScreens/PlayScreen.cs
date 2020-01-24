using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Interfaces;
using Space_Invaders.Models;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Managers;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public class PlayScreen : GameScreen
     {
          private const string k_GameTitle        = "Space Invaders";
          private const string k_Player1AssetName = @"Sprites\Ship01_32x32";
          private const string k_Player2AssetName = @"Sprites\Ship02_32x32";
          private readonly Background r_Background;
          private readonly LevelTransitionScreen r_LevelTransition;
          private readonly PauseScreen r_PauseScreen;
          private readonly IGameSettings r_GameSettings;
          private readonly IScoreManager r_ScoreManager;
          private readonly ISoundManager r_SoundManager;
          private EnemyManager m_EnemyManager;
          private BarrierManager m_BarrierManager;
          private IPlayersManager m_PlayersManager;
          private IInputManager m_InputManager;
          private int m_Level = 1;

          public PlayScreen(LevelTransitionScreen i_LevelTransitionScreen, Game i_Game)
               : base(i_Game)
          {
               r_LevelTransition = i_LevelTransitionScreen;
               r_PauseScreen     = new PauseScreen(this.Game);
               r_Background      = new Background(this);
               r_ScoreManager    = this.Game.Services.GetService(typeof(IScoreManager)) as IScoreManager;
               r_SoundManager    = this.Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
               r_GameSettings    = this.Game.Services.GetService(typeof(IGameSettings)) as IGameSettings;
               r_ScoreManager.AddScreen(this);
               r_GameSettings.GraphicsDeviceManager.PreferredBackBufferWidth  = (int)r_Background.Width;
               r_GameSettings.GraphicsDeviceManager.PreferredBackBufferHeight = (int)r_Background.Height;
               r_GameSettings.GraphicsDeviceManager.ApplyChanges();
          }

          public override void Initialize()
          {
               if (!IsInitialized)
               {
                    r_LevelTransition.StateChanged += levelTransition_StateChanged;
                    r_PauseScreen.StateChanged += pauseScreen_StateChanged;
                    this.Game.Window.Title = k_GameTitle;

                    initServices();
                    initPlayers();
                    initDrawableManagers();

                    base.Initialize();
                    IsInitialized = true;
               }
          }

          private void pauseScreen_StateChanged(object i_Sender, StateChangedEventArgs i_Args)
          {
               if (i_Args.CurrentState == eScreenState.Closed)
               {
                    this.ScreensManager.SetCurrentScreen(this);
               }
          }

          private void initDrawableManagers()
          {
               m_EnemyManager   = new EnemyManager(this);
               m_BarrierManager = new BarrierManager(this, m_PlayersManager[0].StartPosition.Y, m_PlayersManager[0].Height);

               m_EnemyManager.MatrixReachedBottomWindow += onGameOver;
               m_EnemyManager.AllEnemiesDied            += enemyManager_AllEnemiesDied;
               m_PlayersManager.AllPlayersDied          += onGameOver;
          }

          private void enemyManager_AllEnemiesDied()
          {
               m_Level++;
               m_EnemyManager.UpdateLevelDifficulty();
               m_BarrierManager.UpdateLevelDifficulty();
               m_PlayersManager.LevelReset();
               m_EnemyManager.LevelReset();
               m_BarrierManager.LevelReset();
               showLevelTransition();
          }

          private void showLevelTransition()
          {
               if (r_LevelTransition.CurrentLevel < m_Level)
               {
                    this.Deactivate();
                    r_LevelTransition.CurrentLevel++;
                    this.ScreensManager.SetCurrentScreen(r_LevelTransition);
               }
          }

          private void levelTransition_StateChanged(object i_Sender, StateChangedEventArgs i_Args)
          {
               if (i_Args.CurrentState == eScreenState.Closed)
               {
                    this.ScreensManager.SetCurrentScreen(this);
               }
          }

          private void initPlayers()
          {
               m_PlayersManager.AddPlayer(k_Player1AssetName);
               m_PlayersManager.AddPlayer(k_Player2AssetName);

               Player player2 = m_PlayersManager.GetLastAddedPlayer() as Player;
               player2.MoveLeftKey = Keys.A;
               player2.MoveRightKey = Keys.D;
               player2.ShootKey = Keys.W;
               player2.RepresentativeColor = Color.Green;
               player2.GroupRepresentative = m_PlayersManager[0];
          }

          private void initServices()
          {
               m_PlayersManager = new PlayersManager(this);
               m_PlayersManager.PlayerCollided += playersManager_PlayerCollided;
               m_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
          }

          private void playersManager_PlayerCollided(BasePlayer i_Player, ICollidable2D i_CollidedWith)
          {
               Enemy enemy = i_CollidedWith as Enemy;

               if (enemy != null)
               {
                    onGameOver();
               }
          }

          private void onGameOver()
          {
               r_SoundManager.EnableMuteKey = false;
               ExitScreen();
          }

          public void ResetAll()
          {
               m_PlayersManager.ResetAll();
               m_EnemyManager.ResetAll();
               m_BarrierManager.ResetAll();
               r_LevelTransition.ResetAll();
               m_Level = 1;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (m_InputManager.KeyPressed(Keys.Escape))
               {
                    this.Game.Exit();
               }

               if (m_InputManager.KeyPressed(Keys.P))
               {
                    this.ScreensManager.SetCurrentScreen(r_PauseScreen);
               }

               if (r_SoundManager.EnableMuteKey == false)
               {
                    r_SoundManager.EnableMuteKey = true;
               }

               showLevelTransition();

               base.Update(i_GameTime);
          }

          public override void Draw(GameTime i_GameTime)
          {
               base.Draw(i_GameTime);
               r_ScoreManager.DrawScores(this);
          }
     }
}
