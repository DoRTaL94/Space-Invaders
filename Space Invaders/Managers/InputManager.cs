using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Components;
using Space_Invaders.Interfaces;

namespace Space_Invaders.Managers
{
     public class InputManager : GameService, IInputManager
     {
          public InputManager(Game i_Game)
              : base(i_Game, int.MinValue)
          { 
          }

          public KeyboardState PrevKeyboardState { get; private set; }

          public KeyboardState KeyboardState { get; private set; }

          public MouseState PrevMouseState { get; private set; }

          public MouseState MouseState { get; private set; }

          public GamePadState PrevGamePadState { get; private set; }

          public GamePadState GamePadState { get; private set; }

          public override void Initialize()
          {
               PrevKeyboardState = Keyboard.GetState();
               KeyboardState = PrevKeyboardState;

               PrevMouseState = Mouse.GetState();
               MouseState = PrevMouseState;

               PrevGamePadState = GamePad.GetState(PlayerIndex.One);
               GamePadState = PrevGamePadState;
          }

          protected override void RegisterAsService()
          {
               Game.Services.AddService(typeof(IInputManager), this);
          }

          public override void Update(GameTime gameTime)
          {
               PrevKeyboardState = KeyboardState;
               KeyboardState = Keyboard.GetState();

               PrevMouseState = MouseState;
               MouseState = Mouse.GetState();

               PrevGamePadState = GamePadState;
               GamePadState = GamePad.GetState(PlayerIndex.One);
          }

          public bool KeyHeld(Keys i_Key)
          {
               return KeyboardState.IsKeyDown(i_Key) && PrevKeyboardState.IsKeyDown(i_Key);
          }

          public bool KeyReleased(Keys i_Key)
          {
               return PrevKeyboardState.IsKeyDown(i_Key) && KeyboardState.IsKeyUp(i_Key);
          }

          public bool KeyPressed(Keys i_Key)
          {
               return PrevKeyboardState.IsKeyUp(i_Key) && KeyboardState.IsKeyDown(i_Key);
          }

          public bool ButtonPressed(eInputButtons i_Buttons)
          {
               const bool v_OneIsEnough = true;

               return buttonStateChanged(i_Buttons, ButtonState.Pressed, v_OneIsEnough);
          }

          public bool ButtonReleased(eInputButtons i_Buttons)
          {
               const bool v_OneIsEnough = true;

               return buttonStateChanged(i_Buttons, ButtonState.Released, v_OneIsEnough);
          }

          public bool ButtonsPressed(eInputButtons i_Buttons)
          {
               const bool v_OneIsEnough = true;

               return buttonStateChanged(i_Buttons, ButtonState.Pressed, !v_OneIsEnough);
          }

          public bool ButtonsReleased(eInputButtons i_Buttons)
          {
               const bool v_OneIsEnough = true;

               return buttonStateChanged(i_Buttons, ButtonState.Released, !v_OneIsEnough);
          }

          public bool ButtonStateChanged(eInputButtons i_Buttons)
          {
               const bool v_OneIsEnough = true;

               return buttonStateChanged(i_Buttons, ButtonState.Released, v_OneIsEnough)
                   ||
                   buttonStateChanged(i_Buttons, ButtonState.Pressed, v_OneIsEnough);
          }

          private bool buttonStateChanged(eInputButtons i_Buttons, ButtonState i_ButtonState, bool i_IsOneEnough)
          {
               const bool v_CheckChanged = true;

               return checkButtonsState(i_Buttons, i_ButtonState, i_IsOneEnough, v_CheckChanged);
          }

          private bool checkButtonsState(eInputButtons i_Buttons, ButtonState i_ButtonState, bool i_IsOneEnough)
          {
               const bool v_CheckChanged = true;
               return checkButtonsState(i_Buttons, i_ButtonState, i_IsOneEnough, !v_CheckChanged);
          }

          private bool checkButtonsState(eInputButtons i_Buttons, ButtonState i_ButtonState, bool i_IsOneEnough, bool i_CheckChanged)
          {
               bool checkRelease = i_ButtonState == ButtonState.Released;

               bool atLeastOneIsTrue = false;
               bool allTrue = false;
               bool currCheck = false;

               ButtonState currState = i_ButtonState;
               ButtonState prevState = checkRelease ? ButtonState.Pressed : ButtonState.Released;

               if ((i_Buttons & eInputButtons.A) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.A)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.A));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.B) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.B)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.B));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.X) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.X)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.X));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.Y) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.Y)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.Y));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.DPadDown) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.DPadDown)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.DPadDown));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.DPadUp) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.DPadUp)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.DPadUp));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.DPadLeft) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.DPadLeft)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.DPadLeft));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.DPadRight) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.DPadRight)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.DPadRight));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.Back) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.Back)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.Back));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.Start) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.Start)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.Start));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftShoulder) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftShoulder)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftShoulder));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightShoulder) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightShoulder)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightShoulder));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftStick) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftStick)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftStick));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightStick) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightStick)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightStick));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftThumbstickDown) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftThumbstickDown)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftThumbstickDown));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftThumbstickUp) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftThumbstickUp)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftThumbstickUp));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftThumbstickLeft) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftThumbstickLeft)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftThumbstickLeft));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftThumbstickRight) != 0)
               {
                    currCheck = checkRelease == GamePadState.IsButtonUp(Buttons.LeftThumbstickRight)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftThumbstickRight));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightThumbstickDown) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightThumbstickDown)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightThumbstickDown));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightThumbstickUp) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightThumbstickUp)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightThumbstickUp));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightThumbstickLeft) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightThumbstickLeft)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightThumbstickLeft));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightThumbstickRight) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.RightThumbstickRight)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightThumbstickRight));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.LeftTrigger) != 0)
               {
                    currCheck =
                        checkRelease == GamePadState.IsButtonUp(Buttons.LeftTrigger)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.LeftTrigger));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.RightTrigger) != 0)
               {
                    currCheck =
                         checkRelease == GamePadState.IsButtonUp(Buttons.RightTrigger)
                        && (!i_CheckChanged || checkRelease != PrevGamePadState.IsButtonUp(Buttons.RightTrigger));

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               if ((i_Buttons & eInputButtons.Left) != 0)
               {
                    currCheck =
                        MouseState.LeftButton == currState
                        && ((PrevMouseState.LeftButton == prevState) || !i_CheckChanged);

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }
               else if ((i_Buttons & eInputButtons.Middle) != 0)
               {
                    currCheck =
                        MouseState.MiddleButton == currState
                        && ((PrevMouseState.MiddleButton == prevState) || !i_CheckChanged);

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }
               else if ((i_Buttons & eInputButtons.Right) != 0)
               {
                    currCheck =
                        MouseState.RightButton == currState
                        && ((PrevMouseState.RightButton == prevState) || !i_CheckChanged);

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }
               else if ((i_Buttons & eInputButtons.XButton1) != 0)
               {
                    currCheck =
                        MouseState.XButton1 == currState
                        && ((PrevMouseState.XButton1 == prevState) || !i_CheckChanged);

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }
               else if ((i_Buttons & eInputButtons.XButton2) != 0)
               {
                    currCheck =
                        MouseState.XButton2 == currState
                        && ((PrevMouseState.XButton2 == prevState) || !i_CheckChanged);

                    atLeastOneIsTrue |= currCheck;
                    allTrue &= currCheck;
               }

               return i_IsOneEnough ? atLeastOneIsTrue : allTrue;
          }

          public bool ButtonIsDown(eInputButtons i_MouseButtons)
          {
               const bool v_OneIsEnough = true;
               return checkButtonsState(i_MouseButtons, ButtonState.Pressed, v_OneIsEnough);
          }

          public bool ButtonsAreDown(eInputButtons i_MouseButtons)
          {
               const bool v_OneIsEnough = true;
               return checkButtonsState(i_MouseButtons, ButtonState.Pressed, !v_OneIsEnough);
          }

          public bool ButtonIsUp(eInputButtons i_MouseButtons)
          {
               const bool v_OneIsEnough = true;
               return checkButtonsState(i_MouseButtons, ButtonState.Released, v_OneIsEnough);
          }

          public bool ButtonsAreUp(eInputButtons i_MouseButtons)
          {
               const bool v_OneIsEnough = true;
               return checkButtonsState(i_MouseButtons, ButtonState.Released, !v_OneIsEnough);
          }

          public Vector2 MousePositionDelta
          {
               get
               {
                    return new Vector2(
                        (float)(MouseState.X - PrevMouseState.X),
                        (float)(MouseState.Y - PrevMouseState.Y));
               }
          }

          public int ScrollWheelDelta
          {
               get { return MouseState.ScrollWheelValue - PrevMouseState.ScrollWheelValue; }
          }

          public Vector2 LeftThumbDelta
          {
               get
               {
                    return new Vector2(
                        GamePadState.ThumbSticks.Left.X - PrevGamePadState.ThumbSticks.Left.X,
                        GamePadState.ThumbSticks.Left.Y - PrevGamePadState.ThumbSticks.Left.Y);
               }
          }

          public Vector2 RightThumbDelta
          {
               get
               {
                    return new Vector2(
                        GamePadState.ThumbSticks.Right.X - PrevGamePadState.ThumbSticks.Right.X,
                        GamePadState.ThumbSticks.Right.Y - PrevGamePadState.ThumbSticks.Right.Y);
               }
          }

          public float LeftTrigerDelta
          {
               get { return GamePadState.Triggers.Left - PrevGamePadState.Triggers.Left; }
          }

          public float RightTrigerDelta
          {
               get { return GamePadState.Triggers.Right - PrevGamePadState.Triggers.Right; }
          }

          public string PressedKeys
          {
               get
               {
                    Keys[] pressedKeys = KeyboardState.GetPressedKeys();
                    string keys = string.Empty;

                    if (pressedKeys.Length > 0)
                    {
                         StringBuilder keysMsgBuilder = new StringBuilder(pressedKeys.Length * 3);
                         int keysCount = 0;
                         foreach (Keys key in pressedKeys)
                         {
                              keysCount++;
                              keysMsgBuilder.Append(key.ToString());
                              if (keysCount < pressedKeys.Length)
                              {
                                   keysMsgBuilder.Append(", ");
                              }
                         }

                         keys = keysMsgBuilder.ToString();
                    }

                    return keys;
               }
          }

          public override string ToString()
          {
               string status = string.Format(
@"
Keyboard.PressedKeys:       {18}

GamePad.IsConnected:        {0}
GamePad.ThumbSticks.Left    {1}
GamePad.ThumbSticks.Right:  {2}
GamePad.Triggers.Left:      {3}
GamePad.Triggers.Right:     {4}
GamePad.DPad:               {5}
GamePad.Buttons:            {6}
GamePad.PacketNumber:       {7}

Mouse.X:            {8}
Mouse.Y:            {9}
Mouse.DeltaXY:      {10}
Mouse.Left:         {11}
Mouse.Middle:       {12}
Mouse.Right:        {13}
Mouse.XButton1:     {14}
Mouse.XButton2:     {15}
ScrollWheelValue:   {16}
ScrollWheelDelta:   {17}
",
    GamePadState.IsConnected,
    GamePadState.ThumbSticks.Left,
    GamePadState.ThumbSticks.Right,
    GamePadState.Triggers.Left,
    GamePadState.Triggers.Right,
    GamePadState.DPad,
    GamePadState.Buttons,
    GamePadState.PacketNumber,
    MouseState.X,
    MouseState.Y,
    MousePositionDelta,
    MouseState.LeftButton,
    MouseState.MiddleButton,
    MouseState.RightButton,
    MouseState.XButton1,
    MouseState.XButton2,
    MouseState.ScrollWheelValue,
    ScrollWheelDelta,
    PressedKeys);
               return status;
          }
     }
}
