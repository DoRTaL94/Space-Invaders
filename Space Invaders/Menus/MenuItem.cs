using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Interfaces;
using Space_Invaders.Managers;
using Space_Invaders.Utils;
using Space_Invaders.Models;
using Space_Invaders.Screens;
using Space_Invaders.Animators.ConcreteAnimators;

namespace Space_Invaders.Menus
{
     public class MenuItem : Sprite, ISoundEmitter
     {
          public event Action<MenuItem> Clicked;

          public event Action<string> SoundActionOccurred;

          private const string k_MenuItemFocusSound = "MenuMove";
          private const float k_PulsePerSec         = 1.5f;
          private const float k_TargetScale         = 1.03f;
          private readonly List<KeyValuePair<eInputButtons, Action<MenuItem>>> r_ButtonsToActions;
          private readonly List<KeyValuePair<Keys, Action<MenuItem>>> r_KeysToActions;
          private readonly Action<MenuItem> r_CheckMouseOrKBState;
          protected readonly IInputManager r_InputManager;
          private StrokeSpriteFont m_StrokeSpriteFont;
          private Action<MenuItem> m_WheelUpAction;
          private Action<MenuItem> m_WheelDownAction;
          private Action<MenuItem> m_WheelAction;

          public MenuItem(StrokeSpriteFont i_StrokeSpriteFont, GameScreen i_GameScreen, Menu i_LinkedMenu = null, Action<MenuItem> i_CheckMosueOrKBState = null) 
               : base(string.Empty, i_GameScreen)
          {
               r_CheckMouseOrKBState = i_CheckMosueOrKBState;
               r_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
               r_KeysToActions = new List<KeyValuePair<Keys, Action<MenuItem>>>();
               r_ButtonsToActions = new List<KeyValuePair<eInputButtons, Action<MenuItem>>>();

               this.StrokeSpriteFont = i_StrokeSpriteFont;
               this.VisibleChanged += menuItem_VisibleChanged;
               this.LinkedMenu = i_LinkedMenu;
               this.BlendState = BlendState.NonPremultiplied;
          }

          public MenuItem(string i_Text, GameScreen i_GameScreen, Menu i_LinkedMenu = null, Action<MenuItem> i_CheckMosueOrKBState = null)
               : this(new StrokeSpriteFont(i_Text, i_GameScreen), i_GameScreen, i_LinkedMenu, i_CheckMosueOrKBState)
          {
          }

          public void BindActionToKeys(Action<MenuItem> i_Action, params Keys[] i_Keys)
          {
               foreach(Keys key in i_Keys)
               {
                    r_KeysToActions.Add(new KeyValuePair<Keys, Action<MenuItem>>(key, i_Action));
               }
          }

          public void BindActionToMouseButtons(Action<MenuItem> i_Action, params eInputButtons[] i_Buttons)
          {
               foreach (eInputButtons button in i_Buttons)
               {
                    r_ButtonsToActions.Add(new KeyValuePair<eInputButtons, Action<MenuItem>>(button, i_Action));
               }
          }

          public void BindActionToMouseWheelUp(Action<MenuItem> i_Action)
          {
               m_WheelUpAction = i_Action;
          }

          public void BindActionToMouseWheelDown(Action<MenuItem> i_Action)
          {
               m_WheelDownAction = i_Action;
          }

          public void BindActionToMouseWheel(Action<MenuItem> i_Action)
          {
               m_WheelAction = i_Action;
          }

          private void menuItem_VisibleChanged(object i_Sender, EventArgs i_Args)
          {
               m_StrokeSpriteFont.Enabled = this.Visible;
               m_StrokeSpriteFont.Visible = this.Visible;
               this.Enabled = this.Visible;
          }

          public Menu LinkedMenu { get; private set; }

          public bool IsFocused { get; set; }

          public override float Opacity
          {
               get
               {
                    return (float)TintColor.A / (float)byte.MaxValue;
               }

               set
               {
                    TintColor = new Color(TintColor, (byte)(value * (float)byte.MaxValue));
                    m_StrokeSpriteFont.Opacity = value;
               }
          }

          public StrokeSpriteFont StrokeSpriteFont
          {
               get
               {
                    return m_StrokeSpriteFont;
               }

               set
               {
                    if (m_StrokeSpriteFont == null || (m_StrokeSpriteFont != null && value.Text != string.Empty))
                    {
                         m_StrokeSpriteFont = value;
                         m_StrokeSpriteFont.Position = this.Position;
                         this.GameScreen.Add(m_StrokeSpriteFont);
                    }
               }
          }

          public override Vector2 Position
          {
               get
               {
                    return m_Position;
               }

               set
               {
                    m_Position = value;

                    if (StrokeSpriteFont != null && StrokeSpriteFont.Position == Vector2.Zero)
                    {
                         StrokeSpriteFont.Position = value;
                    }
               }
          }

          public override void Initialize()
          {
               if (!IsInitialized)
               {
                    base.Initialize();

                    IsInitialized = true;

                    if (!StrokeSpriteFont.IsInitialized)
                    {
                         StrokeSpriteFont.Initialize();
                    }

                    StrokeSpriteFont.Animations.Add(new PulseAnimator(TimeSpan.Zero, k_TargetScale, k_PulsePerSec));
                    StrokeSpriteFont.Animations.Add(new WaypointsAnimator(100, TimeSpan.FromSeconds(0.2f), false, this.Position + new Vector2(-10, 0)));
                    StrokeSpriteFont.Animations.Enabled = false;
               }
          }

          public void Focus()
          {
               IsFocused = true;

               if (IsInitialized)
               {
                    SoundActionOccurred.Invoke(k_MenuItemFocusSound);
                    this.StrokeSpriteFont.TintColor = Color.DeepSkyBlue;
                    this.StrokeSpriteFont.Animations.Restart();
               }
          }

          public void UnFocus()
          {
               IsFocused = false;

               if (IsInitialized)
               {
                    this.StrokeSpriteFont.TintColor = Color.White;
                    this.StrokeSpriteFont.Animations.Restart();
                    this.StrokeSpriteFont.Animations.Pause();
               }
          }

          private void checkMouseOrKBStateCheckClick()
          {
               if (r_CheckMouseOrKBState != null && IsFocused)
               {
                    r_CheckMouseOrKBState.Invoke(this);
               }
          }

          private bool invokeKeyBindActions()
          {
               bool isClicked = false;

               foreach (KeyValuePair<Keys, Action<MenuItem>> bind in r_KeysToActions)
               {
                    if (r_InputManager.KeyPressed(bind.Key))
                    {
                         bind.Value.Invoke(this);
                         isClicked = true;
                    }
               }

               return isClicked;
          }

          private bool invokeButtonBindActions()
          {
               bool isClicked = false;

               foreach (KeyValuePair<eInputButtons, Action<MenuItem>> bind in r_ButtonsToActions)
               {
                    if (r_InputManager.ButtonPressed(bind.Key))
                    {
                         bind.Value.Invoke(this);
                         isClicked = true;
                    }
               }

               return isClicked;
          }

          private bool invokeMouseWheelBindAction()
          {
               bool isClicked = false;

               if (m_WheelAction != null)
               {
                    if (r_InputManager.ScrollWheelDelta != 0)
                    {
                         m_WheelAction.Invoke(this);
                         isClicked = true;
                    }
               }
               else
               {
                    if (m_WheelUpAction != null)
                    {
                         if (r_InputManager.ScrollWheelDelta > 0)
                         {
                              m_WheelUpAction.Invoke(this);
                              isClicked = true;
                         }
                    }

                    if (m_WheelDownAction != null)
                    {
                         if (r_InputManager.ScrollWheelDelta < 0)
                         {
                              m_WheelDownAction.Invoke(this);
                              isClicked = true;
                         }
                    }
               }

               return isClicked;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (IsFocused)
               {
                    checkMouseOrKBStateCheckClick();
                    bool isClicked = 
                         invokeButtonBindActions() || 
                         invokeKeyBindActions() || 
                         invokeMouseWheelBindAction();

                    if (isClicked)
                    {
                         Clicked.Invoke(this);
                    }
               }

               base.Update(i_GameTime);
          }

          public string Text
          {
               get
               {
                    return m_StrokeSpriteFont.Text;
               }
          }
     }
}
