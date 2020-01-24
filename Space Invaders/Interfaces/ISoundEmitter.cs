using System;

namespace Space_Invaders.Interfaces
{
     public interface ISoundEmitter
     {
          event Action<string> SoundActionOccurred;
     }
}
