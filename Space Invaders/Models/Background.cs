using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Screens;

namespace Space_Invaders.Models
{
     public class Background : Sprite
     {
          private const string k_AssetName = @"Sprites\BG_Space01_1024x768";
          private const int k_CallsOrder   = 0;

          public Background(GameScreen i_GameScreen) : base(k_AssetName, i_GameScreen, k_CallsOrder)
          {
               this.BlendState = BlendState.NonPremultiplied;
               this.Width = 1024;
               this.Height = 768;
               this.GameScreen.Add(this);
          }
     }
}
