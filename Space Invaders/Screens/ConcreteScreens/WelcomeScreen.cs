using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Animators.ConcreteAnimators;
using Space_Invaders.Models;

namespace Space_Invaders.Screens.ConcreteScreens
{
     public class WelcomeScreen : ControlScreen
     {
          private const string k_WelcomeAssetName      = @"Sprites\WelcomeMessage";
          private const string k_InstructionsAssetName = @"Sprites\Instructions";
          private const float k_WelcomePulsePerSec     = 0.7f;
          private const float k_WelcomeTargetScale     = 1.05f;
          private const int k_Space                    = 50;
          private const float k_ActivationLength       = 0.5f;
          private const float k_DeactivationLength     = 0.5f;
          private Sprite m_WelcomeMessage;
          private Background m_Background;
          private Sprite m_Instructions;

          public WelcomeScreen(Game i_Game)
              : base(i_Game)
          {
               m_WelcomeMessage = new Sprite(k_WelcomeAssetName, this);
               m_Instructions = new Sprite(k_InstructionsAssetName, this);
               m_Background = new Background(this);

               this.UseFadeTransition = true;
               this.ActivationLength = TimeSpan.FromSeconds(k_ActivationLength);
               this.DeactivationLength = TimeSpan.FromSeconds(k_DeactivationLength);
               this.Add(m_WelcomeMessage);
               this.Add(m_Instructions);
               this.BlendState = BlendState.NonPremultiplied;

               m_WelcomeMessage.BlendState = BlendState.NonPremultiplied;
               m_Instructions.BlendState   = BlendState.NonPremultiplied;
          }

          public override void Initialize()
          {
               base.Initialize();

               m_WelcomeMessage.Animations.Add(new PulseAnimator(TimeSpan.Zero, k_WelcomeTargetScale, k_WelcomePulsePerSec));
               m_WelcomeMessage.Animations.Enabled = true;
               m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SourceRectangleCenter;
               m_WelcomeMessage.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
               m_WelcomeMessage.Position = CenterOfViewPort;

               m_Instructions.PositionOrigin = m_WelcomeMessage.SourceRectangleCenter;
               m_Instructions.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
               m_Instructions.Position = CenterOfViewPort + new Vector2(0, m_WelcomeMessage.Height + k_Space);
          }

          protected override void DoTransition()
          {
               if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
               {
                    m_Background.Opacity = this.TransitionPosition;
                    m_WelcomeMessage.Opacity = this.TransitionPosition;
                    m_Instructions.Opacity = this.TransitionPosition;
               }
          }
     }
}
