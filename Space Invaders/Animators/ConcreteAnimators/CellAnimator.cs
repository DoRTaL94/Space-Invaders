using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class CellAnimator : SpriteAnimator
     {
          private const string k_Name = "Cell";
          private readonly bool r_Loop;
          private readonly int r_NumOfCells;
          private readonly bool r_IsStartFromEnd;
          private readonly TimeSpan r_CellTime;
          private int m_CurrCellIdx;
          private TimeSpan m_TimeLeftForCell;

          public CellAnimator(bool i_IsStartFromEnd, TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength)
               : base(k_Name, i_AnimationLength)
          {
               r_CellTime = i_CellTime;
               m_TimeLeftForCell = i_CellTime;
               r_NumOfCells = i_NumOfCells;
               r_Loop = i_AnimationLength == TimeSpan.Zero;
               r_IsStartFromEnd = i_IsStartFromEnd;

               if (r_IsStartFromEnd)
               {
                    m_CurrCellIdx = i_NumOfCells - 1;
               }
          }

          private void goToNextFrameFromStart()
          {
               m_CurrCellIdx++;

               if (m_CurrCellIdx >= r_NumOfCells)
               {
                    if (r_Loop)
                    {
                         m_CurrCellIdx = 0;
                    }
                    else
                    {
                         m_CurrCellIdx = r_NumOfCells - 1;
                         this.IsFinished = true;
                    }
               }
          }

          private void goToNextFrameFromEnd()
          {
               m_CurrCellIdx--;

               if (m_CurrCellIdx < 0)
               {
                    if (r_Loop)
                    {
                         m_CurrCellIdx = r_NumOfCells - 1;
                    }
                    else
                    {
                         m_CurrCellIdx = 0;
                         this.IsFinished = true;
                    }
               }
          }

          protected override void RevertToOriginal()
          {
               if (r_IsStartFromEnd)
               {
                    m_CurrCellIdx = r_NumOfCells - 1;
               }
               else
               {
                    m_CurrCellIdx = 0;
               }

               m_TimeLeftForCell = r_CellTime;
               this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               if (r_CellTime != TimeSpan.Zero)
               {
                    m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;

                    if (m_TimeLeftForCell.TotalSeconds <= 0)
                    {
                         if (r_IsStartFromEnd)
                         {
                              goToNextFrameFromEnd();
                         }
                         else
                         {
                              goToNextFrameFromStart();
                         }

                         m_TimeLeftForCell = r_CellTime;
                    }
               }

               this.BoundSprite.SourceRectangle = new Rectangle(
               m_OriginalSpriteInfo.SourceRectangle.Left + (m_CurrCellIdx * this.BoundSprite.SourceRectangle.Width),
               this.BoundSprite.SourceRectangle.Top,
               this.BoundSprite.SourceRectangle.Width,
               this.BoundSprite.SourceRectangle.Height);
          }
     }
}
