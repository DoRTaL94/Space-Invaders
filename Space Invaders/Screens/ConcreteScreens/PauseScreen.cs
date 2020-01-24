using System;
using Space_Invaders.Interfaces;
using Space_Invaders.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public class PauseScreen : GameScreen
     {
          private const float k_ActivationLength   = 0.5f;
          private const float k_DeactivationLength = 0.5f;
          private const float k_BlackTintAlpha     = 0.55f;
          private const string k_PauseMsg          = "Press R to resume";
          private readonly IInputManager r_InputManager;
          private readonly StrokeSpriteFont r_ResumeText;

          public PauseScreen(Game i_Game) 
               : base(i_Game)
          {
               r_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
               r_ResumeText = new StrokeSpriteFont(k_PauseMsg, this);
               this.IsOverlayed = true;
               this.BlackTintAlpha = k_BlackTintAlpha;
               this.UseFadeTransition = true;
               this.ActivationLength = TimeSpan.FromSeconds(k_ActivationLength);
               this.DeactivationLength = TimeSpan.FromSeconds(k_DeactivationLength);
               this.BlendState = BlendState.NonPremultiplied;
               this.Add(r_ResumeText);
          }

          public override void Initialize()
          {
               base.Initialize();

               r_ResumeText.PositionOrigin = r_ResumeText.SourceRectangleCenter;
               r_ResumeText.RotationOrigin = r_ResumeText.SourceRectangleCenter;
               r_ResumeText.Position = CenterOfViewPort;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (r_InputManager.KeyPressed(Keys.R))
               {
                    ExitScreen();
               }

               doTransition();

               base.Update(i_GameTime);
          }

          private void doTransition()
          {
               if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
               {
                    r_ResumeText.Opacity = this.TransitionPosition;
               }
          }
     }
}
