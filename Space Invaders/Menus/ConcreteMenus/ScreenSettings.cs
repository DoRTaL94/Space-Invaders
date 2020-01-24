using Space_Invaders.Managers;
using Space_Invaders.Screens;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.Menus.ConcreteMenus
{
     public class ScreenSettings : SubMenu
     {
          private const string k_Title              = "Screen Settings";
          private const string k_FullScreenModeMsg  = "Full Screen Mode: {0}";
          private const string k_MouseVisibilityMsg = "Mouse Visibilty: {0}";
          private const string k_WindowResizingMsg  = "Allow Window Resizing: {0}";

          public ScreenSettings(Menu i_PrevMenu, GameScreen i_GameScreen) 
               : base(k_Title, i_PrevMenu, i_GameScreen)
          {
          }

          protected override void AddItems()
          {
               MenuItem fullScreenMode 
                    = new MenuItem(string.Format(k_FullScreenModeMsg, GameSettings.IsFullScreen ? "On" : "Off"), this.GameScreen);
               fullScreenMode.BindActionToKeys(fullScreenMode_Clicked, Keys.PageUp, Keys.PageDown);
               fullScreenMode.BindActionToMouseButtons(fullScreenMode_Clicked, eInputButtons.Right);
               fullScreenMode.BindActionToMouseWheel(fullScreenMode_Clicked);

               MenuItem mouseVisibility 
                    = new MenuItem(string.Format(k_MouseVisibilityMsg, GameSettings.IsMouseVisible ? "Visible" : "Invisible"), this.GameScreen);
               mouseVisibility.BindActionToKeys(mouseVisability_Clicked, Keys.PageUp, Keys.PageDown);
               mouseVisibility.BindActionToMouseButtons(mouseVisability_Clicked, eInputButtons.Right);
               mouseVisibility.BindActionToMouseWheel(mouseVisability_Clicked);

               MenuItem windowResizing 
                    = new MenuItem(string.Format(k_WindowResizingMsg, GameSettings.IsWindowResizeAllow ? "On" : "Off"), this.GameScreen);
               windowResizing.BindActionToKeys(windowResizing_Clicked, Keys.PageUp, Keys.PageDown);
               windowResizing.BindActionToMouseButtons(windowResizing_Clicked, eInputButtons.Right);
               windowResizing.BindActionToMouseWheel(windowResizing_Clicked);

               this.AddMenuItem(fullScreenMode);
               this.AddMenuItem(mouseVisibility);
               this.AddMenuItem(windowResizing);

               base.AddItems();
          }

          private void windowResizing_Clicked(MenuItem i_MenuItem)
          {
               GameSettings.IsWindowResizeAllow = !GameSettings.IsWindowResizeAllow;
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_WindowResizingMsg, GameSettings.IsWindowResizeAllow ? "On" : "Off");
          }

          private void mouseVisability_Clicked(MenuItem i_MenuItem)
          {
               GameSettings.IsMouseVisible = !GameSettings.IsMouseVisible;
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_MouseVisibilityMsg, GameSettings.IsMouseVisible ? "Visible" : "Invisible");
          }

          private void fullScreenMode_Clicked(MenuItem i_MenuItem)
          {
               GameSettings.IsFullScreen = !GameSettings.IsFullScreen;
               i_MenuItem.StrokeSpriteFont.Text = string.Format(k_FullScreenModeMsg, GameSettings.IsFullScreen ? "On" : "Off");
          }
     }
}
