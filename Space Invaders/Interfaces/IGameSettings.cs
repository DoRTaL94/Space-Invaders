using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Interfaces
{
     public interface IGameSettings
     {
          event EventHandler PlayersCountChanged;

          event EventHandler FullScreenChanged;

          event EventHandler MouseVisibleChanged;

          event EventHandler WindowResizeAllowChanged;

          event EventHandler BackgroundMusicVolumeChanged;

          event EventHandler SoundEffectsVolumeChanged;

          event EventHandler SoundStateChanged;

          GraphicsDeviceManager GraphicsDeviceManager { get; }

          int PlayersCount { get; set; }

          bool IsFullScreen { get; set; }

          bool IsMouseVisible { get; set; }

          bool IsWindowResizeAllow { get; set; }

          int BackgroundMusicVolume { get; set; }

          int SoundEffectsVolume { get; set; }

          bool IsSound { get; set; }
     }
}
