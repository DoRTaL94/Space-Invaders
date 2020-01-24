using System;
using Space_Invaders.Interfaces;
using Space_Invaders.Managers;
using Space_Invaders.Models;
using Microsoft.Xna.Framework;
using Space_Invaders.Animators.ConcreteAnimators;
using Space_Invaders.Menus;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public class GameOverScreen : ControlScreen
     {
          private const string k_GameOverAssetName     = @"Sprites\GameOverMessage";
          private const string k_InstructionsAssetName = @"Sprites\Instructions";
          private const string k_GameOverSoundName     = "GameOver";
          private const float k_PulseTargetScale       = 1.05f;
          private const float k_PulsePerSec            = 0.7f;
          private const float k_SpaceFromGameOverMsg   = 150;
          private const int k_ActivationLength         = 1;
          private const int k_Space                    = 150;
          private readonly IScoreManager r_ScoreManager;
          private readonly ISoundManager r_SoundManager;
          private readonly PlayScreen r_PlayScreen;
          private Sprite m_GameOverMessage;
          private Sprite m_Instructions;
          private Background m_Background;

          public GameOverScreen(PlayScreen i_PlayScreen, Game i_Game)
               : base(i_Game)
          {
               m_Background                        = new Background(this);
               m_Background.TintColor              = Color.Red;
               m_GameOverMessage                   = new Sprite(k_GameOverAssetName, this);
               m_Instructions                      = new Sprite(k_InstructionsAssetName, this);
               r_ScoreManager                      = Game.Services.GetService(typeof(IScoreManager)) as IScoreManager;
               r_SoundManager                      = Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
               r_PlayScreen                        = i_PlayScreen;
               MainMenuScreen.MainMenu.PlayClicked = mainMenu_PlayClicked;
               r_ScoreManager.AddScreen(this);

               this.Add(m_GameOverMessage);
               this.Add(m_Instructions);
               this.ActivationLength = TimeSpan.FromSeconds(k_ActivationLength);
               this.StateChanged += gameOverScreen_StateChanged;
          }

          private void gameOverScreen_StateChanged(object i_Sender, StateChangedEventArgs i_Args)
          {
               if (i_Args.CurrentState == eScreenState.Closed)
               {
                    r_SoundManager.StopMusic(k_GameOverSoundName);
               }
               else if (i_Args.CurrentState == eScreenState.Active)
               {
                    r_SoundManager.PlayMusic(k_GameOverSoundName);
               }
          }

          public override void Initialize()
          {
               if (!IsInitialized)
               {
                    base.Initialize();

                    m_GameOverMessage.Animations.Add(new PulseAnimator(TimeSpan.Zero, k_PulseTargetScale, k_PulsePerSec));
                    m_GameOverMessage.Animations.Enabled = true;
                    m_GameOverMessage.PositionOrigin = m_GameOverMessage.SourceRectangleCenter;
                    m_GameOverMessage.RotationOrigin = m_GameOverMessage.SourceRectangleCenter;
                    m_GameOverMessage.Position = CenterOfViewPort;

                    m_Instructions.PositionOrigin = m_Instructions.SourceRectangleCenter;
                    m_Instructions.RotationOrigin = m_Instructions.SourceRectangleCenter;
                    m_Instructions.Position = CenterOfViewPort + new Vector2(0, m_GameOverMessage.Height + k_Space);

                    r_ScoreManager.ResultPosition = new Vector2(CenterOfViewPort.X, m_GameOverMessage.Position.Y - k_SpaceFromGameOverMsg);

                    IsInitialized = true;
               }
          }

          private void mainMenu_PlayClicked(MenuItem i_MenuItem)
          {
               m_IsDeactivated = false;
               this.ScreensManager.SetCurrentScreen(this);
               OnEnterClicked();
          }

          public override void Draw(GameTime i_GameTime)
          {
               base.Draw(i_GameTime);

               r_ScoreManager.ShowResult(this);
          }

          protected override void DoTransition()
          {
               if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
               {
                    m_GameOverMessage.Scales = new Vector2(this.TransitionPosition);
               }
          }

          protected override void OnEnterClicked()
          {
               r_PlayScreen.ResetAll();
               this.ScreensManager.SetCurrentScreen(r_PlayScreen);
          }
     }
}
