using Space_Invaders.Screens;

namespace Space_Invaders.Interfaces
{
     public interface IScreensMananger
     {
          GameScreen ActiveScreen { get; }

          void SetCurrentScreen(GameScreen i_NewScreen);

          bool Remove(GameScreen i_Screen);

          void Add(GameScreen i_Screen);

          void Push(GameScreen i_GameScreen);
     }
}