using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders.Utils
{
     public static class Texture2DHelper
     {
          public static Color GetPixel(this Color[] i_Colors, int i_Row, int i_Col, int i_Width)
          {
               return i_Colors[i_Col + (i_Row * i_Width)];
          }

          public static void SetPixel(this Color[] i_Colors, int i_Row, int i_Col, int i_Width, Color i_Color)
          {
               i_Colors[i_Col + (i_Row * i_Width)] = i_Color;
          }

          public static Texture2DPixels GetPixels(this Texture2D i_Texture, Rectangle i_SourceRectangle)
          {
               int textureWidth  = i_Texture.Width;
               int textureHeight = i_Texture.Height;
               int pixelsSize    = i_SourceRectangle.Width * i_SourceRectangle.Height;
               Color[] colorData = new Color[textureWidth * textureHeight];
               Color[] pixels    = new Color[pixelsSize];

               i_Texture.GetData(colorData);

               for (int y = 0; y < i_SourceRectangle.Height; y++)
               {
                    for(int x = 0; x < i_SourceRectangle.Width; x++)
                    {
                         pixels[x + (y * i_SourceRectangle.Width)] = colorData[x + i_SourceRectangle.X + ((y + i_SourceRectangle.Y) * textureWidth)];
                    }
               }

               return new Texture2DPixels(pixels, i_SourceRectangle.Height, i_SourceRectangle.Width, i_SourceRectangle);
          }

          public static Texture2D Clone(this Texture2D i_Texture, GraphicsDevice i_GraphicsDevice)
          {
               Texture2D texture = new Texture2D(i_GraphicsDevice, i_Texture.Width, i_Texture.Height);
               Color[] pixels = new Color[(int)(i_Texture.Width * i_Texture.Height)];
               i_Texture.GetData(pixels);
               texture.SetData(pixels);

               return texture;
          }
     }
}
