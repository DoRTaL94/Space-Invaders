using System;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;

namespace Space_Invaders.Models
{
     public abstract class Entity : Sprite, ISoundEmitter
     {
          public event Action<string> SoundActionOccurred;

          private const string k_KillSoundName = @"EnemyKill";
          private int m_LivesForReset          = -1;
          protected int m_Lives;

          public Entity(string i_AssetName, GameScreen i_GameScreen)
               : this(i_AssetName, i_GameScreen, int.MaxValue)
          {
          }

          public Entity(string i_AssetName, GameScreen i_GameScreen, int i_CallsOrder)
               : this(i_AssetName, i_GameScreen, i_CallsOrder, i_CallsOrder)
          {
          }

          public Entity(string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder)
               : base(i_AssetName, i_GameScreen, i_UpdateOrder, i_DrawOrder)
          {
          }

          public string KilledSoundName { get; set; } = k_KillSoundName;

          public int Lives
          {
               get
               {
                    return m_Lives;
               }

               set
               {
                    if (value >= 0)
                    {
                         playLostLifeSound(m_Lives, value);
                         m_Lives = value;

                         if (m_LivesForReset < 0)
                         {
                              m_LivesForReset = value;
                         }
                    }

                    checkIfAlive(m_Lives);
               }
          }

          private void playLostLifeSound(int i_OldLives, int i_NewLives)
          {
               if (i_OldLives > i_NewLives && LifeLostSoundName != string.Empty && SoundActionOccurred != null)
               {
                    SoundActionOccurred.Invoke(LifeLostSoundName);
               }
          }

          private void checkIfAlive(int i_Lives)
          {
               if (m_Lives == 0)
               {
                    if (SoundActionOccurred != null)
                    {
                         SoundActionOccurred.Invoke(KilledSoundName);
                    }

                    IsAlive = false;
               }
               else if (m_Lives > 0)
               {
                    IsAlive = true;
               }
          }

          public string LifeLostSoundName { get; set; } = k_KillSoundName;

          public override void ResetProperties()
          {
               base.ResetProperties();
               this.Lives = m_LivesForReset;
          }

          public bool IsAlive { get; protected set; } = true;
     }
}
