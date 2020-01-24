using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Components
{
     public sealed class DrawableComparer<TDrawble> : IComparer<TDrawble>
         where TDrawble : class, IDrawable
     {
          public static readonly DrawableComparer<TDrawble> Default;

          static DrawableComparer() 
          { 
               Default = new DrawableComparer<TDrawble>(); 
          }

          private DrawableComparer() 
          { 
          }

          public int Compare(TDrawble i_X, TDrawble i_Y)
          {
               const int k_XBigger = 1;
               const int k_Equal = 0;
               const int k_YBigger = -1;

               int retCompareResult = k_YBigger;

               if (i_X == null && i_Y == null)
               {
                    retCompareResult = k_Equal;
               }
               else if (i_X != null)
               {
                    if (i_Y == null)
                    {
                         retCompareResult = k_XBigger;
                    }
                    else if (i_X.Equals(i_Y))
                    {
                         return k_Equal;
                    }
                    else if (i_X.DrawOrder > i_Y.DrawOrder)
                    {
                         return k_XBigger;
                    }
               }

               return retCompareResult;
          }
     }
}
