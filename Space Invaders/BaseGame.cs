using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Managers;
using Space_Invaders.Utils;
using Space_Invaders.Models;

namespace Space_Invaders
{
     public abstract class BaseGame : Game
     {
          public BaseGame()
          {
               InitServices();
          }

          protected override void Update(GameTime i_GameTime)
          {
               this.GameTime = i_GameTime;

               base.Update(i_GameTime);
          }

          protected virtual void InitServices()
          {
               GameSettings = new GameSettings(this);
               InputManager = new InputManager(this);
               CollisionsManager = new CollisionsManager(this);
               RandomBehavior = new RandomBehavior(this);
               ScoreManager = new ScoreManager(this);
               SoundManager = new SoundManager(this);
          }

          public GameTime GameTime { get; set; }

          protected IInputManager InputManager { get; set; }

          protected IScoreManager ScoreManager { get; set; }

          protected IPlayersManager PlayersManager { get; set; }

          protected ICollisionsManager CollisionsManager { get; set; }

          protected IRandomBehavior RandomBehavior { get; set; }

          protected IGameSettings GameSettings { get; set; }

          protected ISoundManager SoundManager { get; set; }
     }
}
