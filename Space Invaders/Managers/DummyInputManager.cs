using Space_Invaders.Managers;
using Space_Invaders.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.Managers
{
     public class DummyInputManager : IInputManager
     {
          public GamePadState GamePadState { get; } = new GamePadState();

          public KeyboardState KeyboardState { get; } = new KeyboardState();

          public MouseState MouseState { get; } = new MouseState();

          public bool ButtonIsDown(eInputButtons i_MouseButtons)
          {
               return false;
          }

          public bool ButtonIsUp(eInputButtons i_MouseButtons)
          {
               return true;
          }

          public bool ButtonsAreDown(eInputButtons i_MouseButtons)
          {
               return false;
          }

          public bool ButtonsAreUp(eInputButtons i_MouseButtons)
          {
               return true;
          }

          public bool ButtonPressed(eInputButtons i_Buttons)
          {
               return false;
          }

          public bool ButtonReleased(eInputButtons i_Buttons)
          {
               return false;
          }

          public bool ButtonsPressed(eInputButtons i_Buttons)
          {
               return false;
          }

          public bool ButtonsReleased(eInputButtons i_Buttons)
          {
               return false;
          }

          public bool KeyPressed(Keys i_Key)
          {
               return false;
          }

          public bool KeyReleased(Keys i_Key)
          {
               return false;
          }

          public bool KeyHeld(Keys i_Key)
          {
               return false;
          }

          public Vector2 MousePositionDelta
          {
               get
               {
                    return Vector2.Zero;
               }
          }

          public int ScrollWheelDelta
          {
               get
               {
                    return 0;
               }
          }

          public Vector2 LeftThumbDelta
          {
               get
               {
                    return Vector2.Zero;
               }
          }

          public Vector2 RightThumbDelta
          {
               get
               {
                    return Vector2.Zero;
               }
          }

          public float LeftTrigerDelta
          {
               get
               {
                    return 0;
               }
          }

          public float RightTrigerDelta
          {
               get
               {
                    return 0;
               }
          }
     }
}
