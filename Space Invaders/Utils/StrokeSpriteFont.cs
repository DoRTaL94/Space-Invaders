using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Models;
using Space_Invaders.Screens;

namespace Space_Invaders.Utils
{
     public class StrokeSpriteFont : Sprite
     {
          private const string k_DefaultFontAssetName = @"Fonts\ArialFont";
          private float m_Scale                       = -1;
          private string m_FontAssetName;
          private SpriteFont m_SpriteFont;
          private Color m_StrokeColor;

          public StrokeSpriteFont(string i_Text, GameScreen i_GameScreen)
               : this(k_DefaultFontAssetName, i_Text, i_GameScreen)
          {
          }

          public StrokeSpriteFont(string i_FontAssetName, string i_Text, GameScreen i_GameScreen)
               : base(string.Empty, i_GameScreen, int.MaxValue)
          {
               m_FontAssetName = i_FontAssetName;
               m_StrokeColor = new Color(Color.Black, this.TintColor.A);
               this.BlendState = BlendState.NonPremultiplied;
               this.Text = i_Text;
               this.BlendState = BlendState.NonPremultiplied;
          }

          public string Text { get; set; }

          public Color StrokeColor
          {
               get
               {
                    m_StrokeColor.A = TintColor.A;
                    return m_StrokeColor;
               }

               set
               {
                    m_StrokeColor = value;
               }
          }

          public float StrokeSize { get; set; } = 0;

          public float Scale
          {
               get
               {
                    m_Scale = Scales.X;
                    return m_Scale;
               }

               set
               {
                    m_Scale = value;
               }
          }

          public override void Initialize()
          {
               if (!IsInitialized)
               {
                    m_SpriteFont = this.Game.Content.Load<SpriteFont>(m_FontAssetName);
                    Vector2 dimension = m_SpriteFont.MeasureString(this.Text);
                    this.Width = dimension.X;
                    this.Height = dimension.Y;

                    base.Initialize();
               }
          }

          protected override void OnDraw()
          {
               if (StrokeSize > 0)
               {
                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(StrokeSize * this.Scale, -StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);
                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(-StrokeSize * this.Scale, -StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);

                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(0, -StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);
                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(0, StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);

                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(StrokeSize * this.Scale, StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);
                    SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw + new Vector2(-StrokeSize * this.Scale, StrokeSize * this.Scale), this.StrokeColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 1f);
               }

               SpriteBatch.DrawString(m_SpriteFont, Text, PositionForDraw, this.TintColor, 0, this.PositionOrigin, this.Scale, SpriteEffects.None, 0f);
          }
     }
}
