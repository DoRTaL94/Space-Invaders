using System;
using Space_Invaders.Models.BaseModels;

namespace Space_Invaders.Interfaces
{
     public interface ILivesManager
     {
          event Action AllPlayersDied;

          void AddPlayer(BasePlayer i_Player);

          bool IsPlayerAlreadyAdded(BasePlayer i_Player);
     }
}