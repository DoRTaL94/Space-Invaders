using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Interfaces;
using Space_Invaders.Components;

namespace Space_Invaders.Managers
{
     public class SoundManager : GameService, ISoundManager
     {
          private const string k_BGMusicCatagory      = "Background";
          private const string k_SoundEffectsCatagory = "SoundEffects";
          private const string k_AudioEnginePath      = @"Content\Win\Sounds.xgs";
          private const string k_WaveBankPath         = @"Content\Win\Wave Bank.xwb";
          private const string k_SoundBankPath        = @"Content\Win\Sound Bank.xsb";
          private readonly IInputManager r_InputManager;
          private readonly IGameSettings r_GameSettings;
          private readonly SoundBank r_SoundBank;
          private readonly WaveBank r_WaveBank;
          private readonly AudioEngine r_AudioEngine;
          private readonly List<MusicCatagoryManager> r_CatagoryManagers;
          private readonly Dictionary<string, MusicCatagoryManager> r_CatagoryManagersLookup;
          private bool m_IsMusicMutted;

          public SoundManager(Game i_Game)
               : base(i_Game)
          {
               r_InputManager           = i_Game.Services.GetService(typeof(IInputManager)) as IInputManager;
               r_GameSettings           = i_Game.Services.GetService(typeof(IGameSettings)) as IGameSettings;
               r_CatagoryManagers       = new List<MusicCatagoryManager>();
               r_CatagoryManagersLookup = new Dictionary<string, MusicCatagoryManager>();
               r_AudioEngine            = new AudioEngine(k_AudioEnginePath);
               r_WaveBank               = new WaveBank(r_AudioEngine, k_WaveBankPath);
               r_SoundBank              = new SoundBank(r_AudioEngine, k_SoundBankPath);

               InitCatagories();
          }

          protected virtual void InitCatagories()
          {
               AddCatagoryManager(k_BGMusicCatagory, r_GameSettings.BackgroundMusicVolume, r_AudioEngine);
               AddCatagoryManager(k_SoundEffectsCatagory, r_GameSettings.BackgroundMusicVolume, r_AudioEngine);

               r_GameSettings.BackgroundMusicVolumeChanged += gameSettings_BackgroundMusicVolumeChanged;
               r_GameSettings.SoundEffectsVolumeChanged += gameSettings_SoundEffectsVolumeChanged;
               r_GameSettings.SoundStateChanged += gameSettings_SoundStateChanged;
          }

          private void gameSettings_SoundStateChanged(object i_Sender, EventArgs i_Args)
          {
               m_IsMusicMutted = !r_GameSettings.IsSound;

               if (m_IsMusicMutted)
               {
                    MuteAll();
               }
               else
               {
                    UnMuteAll();
               }
          }

          private void gameSettings_SoundEffectsVolumeChanged(object i_Sender, EventArgs i_Args)
          {
               r_CatagoryManagersLookup[k_SoundEffectsCatagory].Volume = r_GameSettings.SoundEffectsVolume;
          }

          private void gameSettings_BackgroundMusicVolumeChanged(object i_Sender, EventArgs i_Args)
          {
               r_CatagoryManagersLookup[k_BGMusicCatagory].Volume = r_GameSettings.BackgroundMusicVolume;
          }

          public void AddCatagoryManager(string i_CatagoryName, int i_StartVolume, AudioEngine i_AudioEngine)
          {
               AddCatagoryManager(new MusicCatagoryManager(i_CatagoryName, i_StartVolume, i_AudioEngine));
          }

          public void AddCatagoryManager(MusicCatagoryManager i_MusicCatagoryManager)
          {
               r_CatagoryManagers.Add(i_MusicCatagoryManager);
               r_CatagoryManagersLookup.Add(i_MusicCatagoryManager.Name, i_MusicCatagoryManager);
          }

          public void AddSoundEmitter(ISoundEmitter i_SoundEmmiter)
          {
               i_SoundEmmiter.SoundActionOccurred += soundEmitter_SoundActionOccurred;
          }

          public void RemoveSoundEmitter(ISoundEmitter i_SoundEmmiter)
          {
               i_SoundEmmiter.SoundActionOccurred -= soundEmitter_SoundActionOccurred;
          }

          private void soundEmitter_SoundActionOccurred(string i_SoundEffectPath)
          {
               PlayMusic(i_SoundEffectPath);
          }
          
          public void PlayMusic(string i_SoundEffectName)
          {
               if (!m_IsMusicMutted)
               {
                    Cue soundCue = r_SoundBank.GetCue(i_SoundEffectName);
                    soundCue.Play();
               }
          }

          public void StopMusic(string i_SoundEffectName)
          {
               if (!m_IsMusicMutted)
               {
                    Cue soundCue = r_SoundBank.GetCue(i_SoundEffectName);
                    soundCue.Stop(AudioStopOptions.Immediate);
               }
          }

          protected virtual void MuteAll()
          {
               foreach (MusicCatagoryManager catagoryManager in r_CatagoryManagers)
               {
                    catagoryManager.Mute();
               }

               m_IsMusicMutted = true;
          }

          protected virtual void UnMuteAll()
          {
               foreach (MusicCatagoryManager catagoryManager in r_CatagoryManagers)
               {
                    catagoryManager.UnMute();
               }

               m_IsMusicMutted = false;
          }

          protected virtual void StopAll()
          {
               foreach (MusicCatagoryManager catagoryManager in r_CatagoryManagers)
               {
                    catagoryManager.Stop();
               }
          }

          protected virtual void ResumeAll()
          {
               foreach (MusicCatagoryManager catagoryManager in r_CatagoryManagers)
               {
                    catagoryManager.Resume();
               }
          }

          public bool EnableMuteKey { get; set; }

          public Keys MuteKey { get; set; } = Keys.M;

          public override void Update(GameTime i_GameTime)
          {
               if(EnableMuteKey && r_InputManager.KeyPressed(MuteKey))
               {
                    if (!m_IsMusicMutted)
                    {
                         r_GameSettings.IsSound = false;
                    }
                    else
                    {
                         r_GameSettings.IsSound = true;
                    }
               }

               base.Update(i_GameTime);
          }
     }
}
