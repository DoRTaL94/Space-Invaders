using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Components;
using Space_Invaders.Interfaces;
using Space_Invaders.Utils;
using Space_Invaders.Screens;
using Space_Invaders.Animators;

namespace Space_Invaders.Models
{
     public partial class Sprite : LoadableDrawableComponent
     {
          private readonly DeviceStates r_SavedDeviceStates;
          protected readonly GameScreen r_GameScreen;
          protected Vector2 m_Position  = Vector2.Zero;
          protected Vector2 m_Scales    = Vector2.One;
          private bool m_UseSharedBatch = false;
          private SpriteBatch m_SpriteBatch;
          private Texture2DPixels m_TexturePixels;
          private Vector2 m_StartPosition;
          private Vector2 m_StartVelocity;

          public Sprite(string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder)
              : base(i_AssetName, i_GameScreen.Game, i_UpdateOrder, i_DrawOrder)
          {
               r_SavedDeviceStates = new DeviceStates();
               r_GameScreen = i_GameScreen;
          }

          public Sprite(string i_AssetName, GameScreen i_GameScreen, int i_CallsOrder)
              : this(i_AssetName, i_GameScreen, i_CallsOrder, i_CallsOrder)
          {
          }

          public Sprite(string i_AssetName, GameScreen i_GameScreen)
              : this(i_AssetName, i_GameScreen, int.MaxValue)
          {
          }

          public override void Initialize()
          {
               base.Initialize();

               Animations = new CompositeAnimator(this);
               IsInitialized = true;
          }

          protected override void LoadContent()
          {
               if (Texture == null && m_AssetName != null && m_AssetName != string.Empty)
               {
                    Texture = Game.Content.Load<Texture2D>(m_AssetName);
               }

               if (m_SpriteBatch == null)
               {
                    m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                    if (m_SpriteBatch == null || this.BlendState == BlendState.NonPremultiplied)
                    {
                         m_SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
                         m_UseSharedBatch = false;
                    }
               }

               base.LoadContent();
          }

          public override void Update(GameTime i_GameTime)
          {
               float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

               OnUpdate(totalSeconds);

               if (this.Animations != null)
               {
                    this.Animations.Update(i_GameTime);
               }

               base.Update(i_GameTime);
          }

          protected virtual void OnUpdate(float i_TotalSeconds)
          {
               this.Position += this.MoveDirection * this.Velocity * i_TotalSeconds;
               this.Rotation += this.AngularVelocity * i_TotalSeconds;
          }

          public override void Draw(GameTime i_GameTime)
          {
               if (!m_UseSharedBatch)
               {
                    if (SaveAndRestoreDeviceState)
                    {
                         saveDeviceStates();
                    }

                    m_SpriteBatch.Begin(
                        SortMode,
                        BlendState, 
                        SamplerState,
                        DepthStencilState, 
                        RasterizerState, 
                        Shader, 
                        TransformMatrix);
               }

               OnDraw();

               if (!m_UseSharedBatch)
               {
                    m_SpriteBatch.End();

                    if (SaveAndRestoreDeviceState)
                    {
                         restoreDeviceStates();
                    }
               }

               base.Draw(i_GameTime);
          }

          protected virtual void OnDraw()
          {
               if (Texture != null)
               {
                    m_SpriteBatch.Draw(
                        Texture, 
                        this.PositionForDraw,
                        this.SourceRectangle, 
                        this.TintColor,
                        this.Rotation, 
                        this.RotationOrigin, 
                        this.Scales,
                        SpriteEffects.None, 
                        this.LayerDepth);
               }
          }

          public GameScreen GameScreen
          {
               get
               {
                    return r_GameScreen;
               }
          }

          public CompositeAnimator Animations { get; set; }

          public Texture2D Texture { get; set; }

          public float Width
          {
               get { return WidthBeforeScale * m_Scales.X; }
               set { WidthBeforeScale = value / m_Scales.X; }
          }

          public float Height
          {
               get { return HeightBeforeScale * m_Scales.Y; }
               set { HeightBeforeScale = value / m_Scales.Y; }
          }

          public float WidthBeforeScale { get; set; }

          public float HeightBeforeScale { get; set; }

          public virtual Vector2 Position
          {
               get { return m_Position; }
               set
               {
                    if (m_Position != value)
                    {
                         m_Position = value;
                         OnPositionChanged();
                    }
               }
          }

          public Vector2 PositionOrigin { get; set; }

          public Vector2 RotationOrigin { get; set; }

          protected Vector2 PositionForDraw
          {
               get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
          }

          public Vector2 TopLeftPosition
          {
               get { return this.Position - this.PositionOrigin; }
               set { this.Position = value + this.PositionOrigin; }
          }

          public Rectangle Bounds
          {
               get
               {
                    return new Rectangle(
                        (int)TopLeftPosition.X,
                        (int)TopLeftPosition.Y,
                        (int)this.Width,
                        (int)this.Height);
               }
          }
          
          public Rectangle BoundsBeforeScale
          {
               get
               {
                    return new Rectangle(
                        (int)TopLeftPosition.X,
                        (int)TopLeftPosition.Y,
                        (int)this.WidthBeforeScale,
                        (int)this.HeightBeforeScale);
               }
          }

          public Rectangle SourceRectangle { get; set; }

          public Vector2 TextureCenter
          {
               get
               {
                    return new Vector2((float)(Texture.Width / 2), (float)(Texture.Height / 2));
               }
          }

          public Vector2 SourceRectangleCenter
          {
               get { return new Vector2((float)(SourceRectangle.Width / 2), (float)(SourceRectangle.Height / 2)); }
          }

          public float Rotation { get; set; }

          public Vector2 Scales
          {
               get { return m_Scales; }
               set
               {
                    if (m_Scales != value)
                    {
                         m_Scales = value;
                         OnPositionChanged();
                    }
               }
          }

          public Color TintColor { get; set; } = Color.White;

          public virtual float Opacity
          {
               get { return (float)TintColor.A / (float)byte.MaxValue; }
               set { TintColor = new Color(TintColor, (byte)(value * (float)byte.MaxValue)); }
          }

          public float LayerDepth { get; set; }

          public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

          public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;

          public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

          public SamplerState SamplerState { get; set; }

          public DepthStencilState DepthStencilState { get; set; }

          public RasterizerState RasterizerState { get; set; }

          public Effect Shader { get; set; }

          public Matrix TransformMatrix { get; set; } = Matrix.Identity;

          public Vector2 Velocity { get; set; }

          public Vector2 StartVelocity 
          {
               get
               {
                    return m_StartVelocity;
               }

               set 
               {
                    m_StartVelocity = value;
                    Velocity = value;
               }
          }

          public float AngularVelocity { get; set; }

          public virtual void ResetProperties()
          {
               this.Animations.Reset();
               this.Animations.Pause();
               this.Animations.Enabled = true;
               this.Velocity = this.StartVelocity;
               this.Position = this.StartPosition;
               this.Visible = true;
               this.Enabled = true;
          }

          protected override void InitBounds()
          {
               if (Texture != null)
               {
                    if (this.Width == 0)
                    {
                         this.Width = this.Texture.Width;
                    }

                    if (this.Height == 0)
                    {
                         this.Height = this.Texture.Height;
                    }
               }

               InitSourceRectangle();
               InitOrigins();
          }

          private void InitOrigins()
          {
          }

          protected virtual void InitSourceRectangle()
          {
               SourceRectangle = new Rectangle(0, 0, (int)WidthBeforeScale, (int)HeightBeforeScale);
          }

          public SpriteBatch SpriteBatch
          {
               protected get
               {
                    return m_SpriteBatch;
               }

               set
               {
                    m_SpriteBatch = value;
                    m_UseSharedBatch = true;
               }
          }

          protected void saveDeviceStates()
          {
               r_SavedDeviceStates.BlendState = GraphicsDevice.BlendState;
               r_SavedDeviceStates.SamplerState = GraphicsDevice.SamplerStates[0];
               r_SavedDeviceStates.DepthStencilState = GraphicsDevice.DepthStencilState;
               r_SavedDeviceStates.RasterizerState = GraphicsDevice.RasterizerState;
          }

          private void restoreDeviceStates()
          {
               GraphicsDevice.BlendState = r_SavedDeviceStates.BlendState;
               GraphicsDevice.SamplerStates[0] = r_SavedDeviceStates.SamplerState;
               GraphicsDevice.DepthStencilState = r_SavedDeviceStates.DepthStencilState;
               GraphicsDevice.RasterizerState = r_SavedDeviceStates.RasterizerState;
          }

          public bool SaveAndRestoreDeviceState { get; set; }

          public virtual bool CheckCollision(ICollidable i_Source)
          {
               bool collided = false;
               ICollidable2D target = this as ICollidable2D;
               ICollidable2D source = i_Source as ICollidable2D;

               if (source != null && source.Visible && this.Visible && target.GroupRepresentative != i_Source.GroupRepresentative)
               {
                    collided = source.Bounds.Intersects(this.Bounds);
               }

               return collided;
          }

          public virtual void Collided(ICollidable i_Collidable)
          {
               this.Velocity *= -1;
          }

          public Sprite ShallowClone()
          {
               return this.MemberwiseClone() as Sprite;
          }

          public Texture2DPixels TexturePixels
          {
               get
               {
                    if (m_TexturePixels == null)
                    {
                         m_TexturePixels = Texture.GetPixels(SourceRectangle);
                    }

                    return m_TexturePixels;
               }
          }

          public Vector2 StartPosition
          {
               get
               {
                    return m_StartPosition;
               }

               set
               {
                    m_StartPosition = value;
                    m_Position = value;
               }
          }

          public Vector2 ViewDirection { get; set; } = Sprite.Down;

          public Vector2 MoveDirection { get; set; } = Sprite.Right;

          public static Vector2 Right { get; } = new Vector2(1, 0);

          public static Vector2 Left { get; } = new Vector2(-1, 0);

          public static Vector2 Up { get; } = new Vector2(0, -1);

          public static Vector2 Down { get; } = new Vector2(0, 1);
     }
}