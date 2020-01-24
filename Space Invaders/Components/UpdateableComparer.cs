using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Components
{
     public sealed class UpdateableComparer : IComparer<IUpdateable>
     {
          public static readonly UpdateableComparer Default;

          static UpdateableComparer() 
          { 
               Default = new UpdateableComparer(); 
          }
          
          private UpdateableComparer() 
          { 
          }

          public int Compare(IUpdateable i_X, IUpdateable i_Y)
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
                    else if (i_X.UpdateOrder > i_Y.UpdateOrder)
                    {
                         return k_XBigger;
                    }
               }

               return retCompareResult;
          }
     }
}
