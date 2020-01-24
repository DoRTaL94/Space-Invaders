using Microsoft.Xna.Framework;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Screens;

namespace Space_Invaders.Interfaces
{
     public interface IScoreManager
     {
          void AddPlayer(BasePlayer i_Player);

          void AddScreen(GameScreen i_GameScreen);

          bool IsPlayerAlreadyAdded(BasePlayer i_Player);

          void ShowResult(GameScreen i_GameScreen);

          void DrawScores(GameScreen i_GameScreen);

          Vector2 ResultPosition { get; set; }
     }
}
