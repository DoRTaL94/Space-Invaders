using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;

namespace Space_Invaders.Models
{
     public class GameSettings : IGameSettings
     {
          public event EventHandler PlayersCountChanged;

          public event EventHandler FullScreenChanged;

          public event EventHandler MouseVisibleChanged;

          public event EventHandler WindowResizeAllowChanged;

          public event EventHandler BackgroundMusicVolumeChanged;

          public event EventHandler SoundEffectsVolumeChanged;

          public event EventHandler SoundStateChanged;

          private readonly Game r_Game;
          private int m_PlayersCount          = 1;
          private int m_BackgroundMusicVolume = 10;
          private int m_SoundEffectsVolume    = 10;
          private bool m_IsSound              = true;
          private bool m_IsFullScreen;

          public GameSettings(Game i_Game)
          {
               GraphicsDeviceManager = new GraphicsDeviceManager(i_Game);
               i_Game.Services.AddService(typeof(IGameSettings), this);
               this.r_Game = i_Game;
          }

          public int PlayersCount
          {
               get
               {
                    return m_PlayersCount;
               }

               set
               {
                    m_PlayersCount = value;

                    if (PlayersCountChanged != null)
                    {
                         PlayersCountChanged.Invoke(this, null);
                    }
               }
          }

          public bool IsFullScreen
          {
               get
               {
                    return m_IsFullScreen;
               }

               set
               {
                    if (m_IsFullScreen != value)
                    {
                         m_IsFullScreen = value;
                         GraphicsDeviceManager.ToggleFullScreen();

                         if (FullScreenChanged != null)
                         {
                              FullScreenChanged.Invoke(this, null);
                         }
                    }
               }
          }

          public bool IsMouseVisible
          {
               get
               {
                    return r_Game.IsMouseVisible;
               }

               set
               {
                    if (r_Game.IsMouseVisible != value)
                    {
                         r_Game.IsMouseVisible = value;

                         if (MouseVisibleChanged != null)
                         {
                              MouseVisibleChanged.Invoke(this, null);
                         }
                    }
               }
          }

          public bool IsWindowResizeAllow
          {
               get
               {
                    return r_Game.Window.AllowUserResizing;
               }

               set
               {
                    if (r_Game.Window.AllowUserResizing != value)
                    {
                         r_Game.Window.AllowUserResizing = value;

                         if (WindowResizeAllowChanged != null)
                         {
                              WindowResizeAllowChanged.Invoke(this, null);
                         }
                    }
               }
          }

          public int BackgroundMusicVolume
          {
               get
               {
                    return m_BackgroundMusicVolume;
               }

               set
               {
                    m_BackgroundMusicVolume = value;

                    if (BackgroundMusicVolumeChanged != null)
                    {
                         BackgroundMusicVolumeChanged.Invoke(this, null);
                    }
               }
          }

          public int SoundEffectsVolume
          {
               get
               {
                    return m_SoundEffectsVolume;
               }

               set
               {
                    m_SoundEffectsVolume = value;

                    if (SoundEffectsVolumeChanged != null)
                    {
                         SoundEffectsVolumeChanged.Invoke(this, null);
                    }
               }
          }

          public bool IsSound
          {
               get
               {
                    return m_IsSound;
               }

               set
               {
                    m_IsSound = value;

                    if (SoundStateChanged != null)
                    {
                         SoundStateChanged.Invoke(this, null);
                    }
               }
          }

          public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
     }
}
