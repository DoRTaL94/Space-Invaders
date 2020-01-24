using Space_Invaders.Managers;
using Space_Invaders.Screens.ConcreteScreens;

namespace Space_Invaders
{
     public class SpaceInvadersGame : BaseGame
     {
          private const string k_BackgroundMusic = "BGMusic";

          public SpaceInvadersGame()
          {
               Content.RootDirectory = "Content";
               LevelTransitionScreen levelTransitionScreen = new LevelTransitionScreen(this);
               PlayScreen playScreen = new PlayScreen(levelTransitionScreen, this);
               ScreensManager screensManager = new ScreensManager(this);
               screensManager.Push(new GameOverScreen(playScreen, this));
               screensManager.Push(playScreen);
               screensManager.Push(levelTransitionScreen);
               screensManager.SetCurrentScreen(new WelcomeScreen(this));
               this.IsMouseVisible = true;
          }

          protected override void LoadContent()
          {
               SoundManager.PlayMusic(k_BackgroundMusic);

               base.LoadContent();
          }
     }
}