using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.Interfaces
{
     public interface ISoundManager
     {
          void AddSoundEmitter(ISoundEmitter i_SoundEmitter);

          void RemoveSoundEmitter(ISoundEmitter i_SoundEmmiter);

          void PlayMusic(string i_SoundEffectName);

          void StopMusic(string i_SoundEffectName);

          bool EnableMuteKey { get; set; }

          Keys MuteKey { get; set; } 
     }
}
