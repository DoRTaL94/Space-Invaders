using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders.Managers
{
     public class MusicCatagoryManager
     {
          private const float k_MaxVolume = 100;
          private readonly AudioEngine r_AudioEngine;
          private int m_Volume;
          private int m_PrevVolume;
          private bool m_IsMute;

          public MusicCatagoryManager(string i_CatagoryName, int i_StartVolume, AudioEngine i_AudioEngine)
          {
               r_AudioEngine = i_AudioEngine;
               Name = i_CatagoryName;
               m_Volume = i_StartVolume;
               Volume = m_Volume;
          }

          public string Name { get; private set; }

          public int Volume
          {
               get
               {
                    return m_Volume;
               }

               set
               {
                    if (value >= 0)
                    {
                         m_PrevVolume = m_Volume;
                         m_Volume = value;
                         r_AudioEngine.GetCategory(Name).SetVolume(m_Volume / k_MaxVolume);
                    }
               }
          }

          public void Mute()
          {
               if(Volume > 0 && !m_IsMute)
               {
                    m_IsMute = true;
                    Volume = 0;
               }
          }

          public void UnMute()
          {
               if (m_IsMute)
               {
                    m_IsMute = false;
                    Volume = m_PrevVolume;
               }
          }

          public void Resume()
          {
               r_AudioEngine.GetCategory(Name).Resume();
          }

          public void Stop()
          {
               r_AudioEngine.GetCategory(Name).Stop(AudioStopOptions.Immediate);
          }
     }
}
