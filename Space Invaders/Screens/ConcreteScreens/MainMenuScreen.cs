using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Menus.ConcreteMenus;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public class MainMenuScreen : GameScreen
     {
          private const float k_ActivationLength   = 0.5f;
          private const float k_DeactivationLength = 0.5f;
          private const float k_BlackTintAlpha     = 0.65f;

          public MainMenuScreen(Game i_Game) 
               : base(i_Game)
          {
               MainMenu = new MainMenu(this);

               this.IsOverlayed           = true;
               this.UseGradientBackground = true;
               this.BlackTintAlpha        = k_BlackTintAlpha;
               this.UseFadeTransition     = true;
               this.ActivationLength      = TimeSpan.FromSeconds(k_ActivationLength);
               this.DeactivationLength    = TimeSpan.FromSeconds(k_DeactivationLength);
               this.BlendState            = BlendState.NonPremultiplied;
          }

          public MainMenu MainMenu { get; private set; }

          public override void Update(GameTime i_GameTime)
          {
               base.Update(i_GameTime);

               if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
               {
                    MainMenu.Opacity = this.TransitionPosition;
               }
          }
     }
}
