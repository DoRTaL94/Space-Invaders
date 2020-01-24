using System;

namespace Space_Invaders.Interfaces
{
     public interface IRandomBehavior
     {
          bool Roll();

          bool Roll(int i_RandomFactor, int i_RandomMin, int i_RandomMax);

          Action DelayedAction { get; set; }

          void TryInvokeDelayedAction();

          int GetRandomNumber(int i_Min, int i_Max);

          TimeSpan GetRandomIntervalMilliseconds(int i_MillisecondsMaxVal);

          TimeSpan GetRandomIntervalSeconds(int i_SecondsMaxVal);
     }
}