using System;
using Space_Invaders.Models.BaseModels;

namespace Space_Invaders.Interfaces
{
     public interface IPlayersManager
     {
          event Action AllPlayersDied;

          event Action<BasePlayer, ICollidable2D> PlayerCollided;

          void AddPlayer(string i_AssetName);

          void AddPlayer(BasePlayer i_Player);

          BasePlayer this[int i_Index] { get; }

          BasePlayer GetLastAddedPlayer();

          int PlayersCount { get; set; }

          void LevelReset();

          void ResetAll();
     }
}