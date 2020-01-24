using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public abstract class ControlScreen : GameScreen
     {
          protected bool m_IsDeactivated;

          public ControlScreen(Game i_Game) 
               : base(i_Game)
          {
               MainMenuScreen = new MainMenuScreen(i_Game);
          }

          public MainMenuScreen MainMenuScreen { get; private set; }

          protected virtual void GetPlayerInput()
          {
               if (InputManager.KeyPressed(Keys.Enter) && !m_IsDeactivated)
               {
                    OnEnterClicked();
               }

               if (InputManager.KeyPressed(Keys.Escape))
               {
                    OnEscClicked();
               }

               if (InputManager.KeyPressed(Keys.M) && !m_IsDeactivated)
               {
                    OnKeysMClicked();
               }
          }

          protected virtual void OnEnterClicked()
          {
               ExitScreen();
          }

          protected virtual void OnEscClicked()
          {
               this.Game.Exit();
          }

          protected virtual void OnKeysMClicked()
          {
               OpenMainMenu();
          }

          protected virtual void OpenMainMenu()
          {
               m_IsDeactivated = true;
               this.ScreensManager.SetCurrentScreen(MainMenuScreen);
               MainMenuScreen.StateChanged += MainMenu_StateChanged;
          }

          protected virtual void MainMenu_StateChanged(object i_Sender, StateChangedEventArgs i_Args)
          {
               if (i_Args.CurrentState == eScreenState.Closed)
               {
                    m_IsDeactivated = false;
                    this.ExitScreen();
               }
          }

          public override void Update(GameTime gameTime)
          {
               base.Update(gameTime);

               GetPlayerInput();
               DoTransition();
          }

          protected virtual void DoTransition()
          {
          }
     }
}
