using Space_Invaders.Managers;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Screens;

namespace Space_Invaders.Menus.ConcreteMenus
{
     public class SoundSettingsMenu : SubMenu
     {
          private const string k_TitleName                 = "Sound Settings";
          private const string k_BackgroundMusicVolumeText = "Background Music Volume: {0}";
          private const string k_SoundEffectVolumeText     = "Sound Effect Volume: {0}";
          private const string k_ToggleSoundText           = "Toggle Sound: {0}";
          private const string k_OnText                    = "On";
          private const string k_OffText                   = "Off";
          private const int k_ChangeVolumeBy               = 10;
          private const int k_MaxVolume                    = 100;

          public SoundSettingsMenu(Menu i_PrevMenu, GameScreen i_GameScreen) 
               : base(k_TitleName, i_PrevMenu, i_GameScreen)
          {
          }

          protected override void AddItems()
          {
               MenuItem backgroundVolumeMeniItem 
                    = new MenuItem(string.Format(k_BackgroundMusicVolumeText, GameSettings.BackgroundMusicVolume), this.GameScreen);
               backgroundVolumeMeniItem.BindActionToKeys(backgroundMusicVolume_Clicked, Keys.PageDown, Keys.PageUp);
               backgroundVolumeMeniItem.BindActionToMouseWheel(backgroundMusicVolume_Clicked);

               MenuItem SoundEffectVolumeMenuItem
                    = new MenuItem(string.Format(k_SoundEffectVolumeText, GameSettings.SoundEffectsVolume), this.GameScreen);
               SoundEffectVolumeMenuItem.BindActionToKeys(soundEffectVolume_Clicked, Keys.PageDown, Keys.PageUp);
               SoundEffectVolumeMenuItem.BindActionToMouseWheel(soundEffectVolume_Clicked);

               MenuItem toggleSoundMenuItem
                    = new MenuItem(string.Format(k_ToggleSoundText, GameSettings.IsSound ? k_OnText : k_OffText), this.GameScreen);
               toggleSoundMenuItem.BindActionToKeys(toggleSound_Clicked, Keys.PageDown, Keys.PageUp);
               toggleSoundMenuItem.BindActionToMouseButtons(toggleSound_Clicked, eInputButtons.Right);
               toggleSoundMenuItem.BindActionToMouseWheel(toggleSound_Clicked);

               this.AddMenuItem(backgroundVolumeMeniItem);
               this.AddMenuItem(SoundEffectVolumeMenuItem);
               this.AddMenuItem(toggleSoundMenuItem);

               base.AddItems();
          }

          private void toggleSound_Clicked(MenuItem i_MenuItem)
          {
               GameSettings.IsSound = !GameSettings.IsSound;
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_ToggleSoundText, GameSettings.IsSound ? k_OnText : k_OffText);
          }

          private void soundEffectVolume_Clicked(MenuItem i_MenuItem)
          {
               updateSoundEffectVolumeInSettings();
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_SoundEffectVolumeText, GameSettings.SoundEffectsVolume);
          }

          private void backgroundMusicVolume_Clicked(MenuItem i_MenuItem)
          {
               updateBackgroundMusicVolumeInSettings();
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_BackgroundMusicVolumeText, GameSettings.BackgroundMusicVolume);
          }

          private void updateSoundEffectVolumeInSettings()
          {
               if (r_InputManager.KeyPressed(Keys.PageDown) 
                    || r_InputManager.ScrollWheelDelta < 0)
               {
                    if (GameSettings.SoundEffectsVolume >= k_ChangeVolumeBy)
                    {
                         GameSettings.SoundEffectsVolume -= k_ChangeVolumeBy;
                    }
               }
               else if (r_InputManager.KeyPressed(Keys.PageUp) 
                    || r_InputManager.ScrollWheelDelta > 0)
               {
                    if (GameSettings.SoundEffectsVolume <= k_MaxVolume - k_ChangeVolumeBy)
                    {
                         GameSettings.SoundEffectsVolume += k_ChangeVolumeBy;
                    }
               }
          }

          private void updateBackgroundMusicVolumeInSettings()
          {
               if (r_InputManager.KeyPressed(Keys.PageDown)
                    || r_InputManager.ScrollWheelDelta < 0)
               {
                    if (GameSettings.BackgroundMusicVolume >= k_ChangeVolumeBy)
                    {
                         GameSettings.BackgroundMusicVolume -= k_ChangeVolumeBy;
                    }
               }
               else if (r_InputManager.KeyPressed(Keys.PageUp)
                    || r_InputManager.ScrollWheelDelta > 0)
               {
                    if (GameSettings.BackgroundMusicVolume <= k_MaxVolume - k_ChangeVolumeBy)
                    {
                         GameSettings.BackgroundMusicVolume += k_ChangeVolumeBy;
                    }
               }
          }
     }
}
