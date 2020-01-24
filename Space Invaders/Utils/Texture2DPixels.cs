using Microsoft.Xna.Framework;

namespace Space_Invaders.Utils
{
     public class Texture2DPixels
     {
          private readonly Rectangle r_SourceRectangle;

          public Texture2DPixels(Color[] i_Colors, int i_Rows, int i_Cols, Rectangle i_SourceRectangle)
          {
               Pixels = i_Colors;
               r_SourceRectangle = i_SourceRectangle;
               Size = i_Rows * i_Cols;
               Rows = i_Rows;
               Cols = i_Cols;
          }

          public Color this[int i_Row, int i_Col]
          {
               get
               {
                    return Pixels.GetPixel(i_Row, i_Col, r_SourceRectangle.Width);
               }

               set
               {
                    Pixels.SetPixel(i_Row, i_Col, r_SourceRectangle.Width, value);
               }
          }

          public Color[] Pixels { get; private set; }

          public int Size { get; private set; }

          public int Rows { get; private set; }

          public int Cols { get; private set; }
     }
}
