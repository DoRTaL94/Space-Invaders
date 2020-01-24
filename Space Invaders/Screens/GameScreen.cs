using System;
using Space_Invaders.Components;
using Space_Invaders.Interfaces;
using Space_Invaders.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders.Screens
{
     public abstract class GameScreen : CompositeDrawableComponent<IGameComponent>
     {
          public event EventHandler<StateChangedEventArgs> StateChanged;

          protected eScreenState m_State   = eScreenState.Inactive;
          protected float m_BlackTintAlpha = 0;
          private IInputManager m_InputManager;
          private IInputManager m_DummyInputManager = new DummyInputManager();
          private Texture2D m_GradientTexture;
          private Texture2D m_BlankTexture;

          public GameScreen(Game i_Game)
              : base(i_Game)
          {
               this.Enabled = false;
               this.Visible = false;
          }

          public eScreenState State
          {
               get 
               { 
                    return m_State; 
               }

               set
               {
                    if (m_State != value)
                    {
                         StateChangedEventArgs args = new StateChangedEventArgs(m_State, value);
                         m_State = value;
                         OnStateChanged(args);
                    }
               }
          }

          private void OnStateChanged(StateChangedEventArgs i_Args)
          {
               switch (i_Args.CurrentState)
               {
                    case eScreenState.Activating:
                         OnActivating();
                         break;
                    case eScreenState.Active:
                         OnActivated();
                         break;
                    case eScreenState.Deactivating:
                         break;
                    case eScreenState.Closing:
                         break;
                    case eScreenState.Inactive:
                    case eScreenState.Closed:
                         OnDeactivated();
                         break;
                    default:
                         break;
               }

               if (StateChanged != null)
               {
                    StateChanged(this, i_Args);
               }
          }

          public IScreensMananger ScreensManager { get; set; }

          public bool IsModal { get; set; } = true;

          public bool IsOverlayed { get; set; }

          public GameScreen PreviousScreen { get; set; }

          public bool HasFocus { get; set; }

          public float BlackTintAlpha
          {
               get 
               { 
                    return m_BlackTintAlpha; 
               }

               set
               {
                    if (m_BlackTintAlpha < 0 || m_BlackTintAlpha > 1)
                    {
                         throw new ArgumentException("value must be between 0 and 1", "BackgroundDarkness");
                    }

                    m_BlackTintAlpha = value;
               }
          }

          public IInputManager InputManager
          {
               get 
               { 
                    return this.HasFocus ? m_InputManager : m_DummyInputManager; 
               }
          }

          public override void Initialize()
          {
               m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;

               if (m_InputManager == null)
               {
                    m_InputManager = m_DummyInputManager;
               }

               base.Initialize();
          }

          internal virtual void Activate()
          {
               if (this.State == eScreenState.Inactive
                   || this.State == eScreenState.Deactivating
                   || this.State == eScreenState.Closed
                   || this.State == eScreenState.Closing)
               {
                    this.State = eScreenState.Activating;

                    if (ActivationLength == TimeSpan.Zero)
                    {
                         this.State = eScreenState.Active;
                    }
               }
          }

          protected virtual void OnActivating()
          {
               this.Enabled = true;
               this.Visible = true;
               this.HasFocus = true;
          }

          protected virtual void OnActivated()
          {
               if (PreviousScreen != null)
               {
                    PreviousScreen.HasFocus = !this.HasFocus;
               }

               TransitionPosition = 1;
          }

          protected internal virtual void Deactivate()
          {
               if (this.State == eScreenState.Active
                   || this.State == eScreenState.Activating)
               {
                    this.State = eScreenState.Deactivating;

                    if (DeactivationLength == TimeSpan.Zero)
                    {
                         this.State = eScreenState.Inactive;
                    }
               }
          }

          public void ExitScreen()
          {
               this.State = eScreenState.Closing;

               if (DeactivationLength == TimeSpan.Zero)
               {
                    this.State = eScreenState.Closed;
               }
          }

          protected virtual void OnDeactivated()
          {
               this.Enabled = false;
               this.Visible = false;
               this.HasFocus = false;

               TransitionPosition = 0;
          }

          protected override void LoadContent()
          {
               base.LoadContent();

               m_GradientTexture = this.ContentManager.Load<Texture2D>(@"Screens\gradient");
               m_BlankTexture = this.ContentManager.Load<Texture2D>(@"Screens\blank");
          }

          public override void Draw(GameTime i_GameTime)
          {
               bool fading = UseFadeTransition
                   && TransitionPosition > 0
                   && TransitionPosition < 1;

               if (PreviousScreen != null
                   && IsOverlayed)
               {
                    PreviousScreen.Draw(i_GameTime);

                    if (!fading && (BlackTintAlpha > 0 || UseGradientBackground))
                    {
                         FadeBackBufferToBlack((byte)(m_BlackTintAlpha * byte.MaxValue));
                    }
               }

               base.Draw(i_GameTime);

               if (fading)
               {
                    FadeBackBufferToBlack(TransitionAlpha);
               }
          }

          public bool UseGradientBackground { get; set; }

          public void FadeBackBufferToBlack(byte i_Alpha)
          {
               Viewport viewport = this.GraphicsDevice.Viewport;

               Texture2D background = UseGradientBackground ? m_GradientTexture : m_BlankTexture;

               SpriteBatch.Begin();
               SpriteBatch.Draw(
                    background,
                    new Rectangle(0, 0, viewport.Width, viewport.Height),
                    new Color((byte)0, (byte)0, (byte)0, i_Alpha));
                    SpriteBatch.End();
          }

          public TimeSpan ActivationLength { get; protected set; } = TimeSpan.Zero;

          public TimeSpan DeactivationLength { get; protected set; } = TimeSpan.Zero;

          public float TransitionPosition { get; protected set; }

          public bool IsClosing { get; protected internal set; }

          public override void Update(GameTime i_GameTime)
          {
               bool doUpdate = true;

               switch (this.State)
               {
                    case eScreenState.Activating:
                    case eScreenState.Deactivating:
                    case eScreenState.Closing:
                         updateTransition(i_GameTime);
                         break;
                    case eScreenState.Active:
                         break;
                    case eScreenState.Inactive:
                    case eScreenState.Closed:
                         doUpdate = false;
                         break;
                    default:
                         break;
               }

               if (doUpdate)
               {
                    base.Update(i_GameTime);

                    if (PreviousScreen != null && !this.IsModal)
                    {
                         PreviousScreen.Update(i_GameTime);
                    }
               }
          }

          private void updateTransition(GameTime i_GameTime)
          {
               bool transionEnded = false;
               int direction = this.State == eScreenState.Activating ? 1 : -1;
               TimeSpan transitionLength = this.State == eScreenState.Activating ? ActivationLength : DeactivationLength;
               float transitionDelta;

               if (transitionLength == TimeSpan.Zero)
               {
                    transitionDelta = 1;
               }
               else
               {
                    transitionDelta = (float)(
                        i_GameTime.ElapsedGameTime.TotalMilliseconds
                        / transitionLength.TotalMilliseconds);
               }

               TransitionPosition += transitionDelta * direction;

               if (((direction < 0) && (TransitionPosition <= 0)) ||
                   ((direction > 0) && (TransitionPosition >= 1)))
               {
                    TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                    transionEnded = true;
               }

               if (transionEnded)
               {
                    onTransitionEnded();
               }
          }

          private void onTransitionEnded()
          {
               switch (this.State)
               {
                    case eScreenState.Inactive:
                    case eScreenState.Activating:
                         this.State = eScreenState.Active;
                         break;
                    case eScreenState.Active:
                    case eScreenState.Deactivating:
                         this.State = eScreenState.Inactive;
                         break;
                    case eScreenState.Closing:
                         this.State = eScreenState.Closed;
                         break;
               }
          }

          protected byte TransitionAlpha
          {
               get 
               { 
                    return (byte)(byte.MaxValue * TransitionPosition * m_BlackTintAlpha); 
               }
          }

          public bool UseFadeTransition { get; set; } = true;
     }
}
