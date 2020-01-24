using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;
using Space_Invaders.Managers;

namespace Space_Invaders.Models.BaseModels
{
     public abstract class BasePlayer : Entity
     {
          public event Action<BasePlayer, ICollidable2D> PlayerCollided;

          protected readonly IInputManager r_InputManager;
          private readonly Vector2 r_Velocity;
          private int m_Score;

          public BasePlayer(string i_AssetName, GameScreen i_GameScreen)
               : this(i_AssetName, i_GameScreen, int.MaxValue)
          {
          }

          public BasePlayer(string i_AssetName, GameScreen i_GameScreen, int i_CallsOrder)
               : this(i_AssetName, i_GameScreen, int.MaxValue, int.MaxValue)
          {
          }

          public BasePlayer(string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder)
               : base(i_AssetName, i_GameScreen, i_UpdateOrder, i_DrawOrder)
          {
               r_InputManager = this.Game.Services.GetService(typeof(IInputManager)) as IInputManager;
               r_Velocity = new Vector2(145, 0);
          }

          public int Score
          {
               get
               {
                    return m_Score;
               }

               set
               {
                    m_Score = value;

                    if (StartingScore < 0)
                    {
                         StartingScore = value;
                    }
               }
          }

          public int StartingScore { get; private set; } = -1;

          public bool IsMouseControllable { get; set; }

          public Keys MoveLeftKey { get; set; } = Keys.H;

          public Keys MoveRightKey { get; set; } = Keys.K;

          public Color RepresentativeColor { get; set; } = Color.Blue;

          public override void ResetProperties()
          {
               base.ResetProperties();
          }

          public override Vector2 Position
          {
               get
               {
                    return base.Position;
               }

               set
               {
                    if (m_Position != value)
                    {
                         m_Position.X = MathHelper.Clamp(value.X, 0, (float)this.GraphicsDevice.Viewport.Width - Width);
                         OnPositionChanged();
                    }
               }
          }

          public override void Initialize()
          {
               IScoreManager scoreManager = this.Game.Services.GetService(typeof(IScoreManager)) as IScoreManager;
               ILivesManager livesManager = this.Game.Services.GetService(typeof(ILivesManager)) as ILivesManager;

               if (scoreManager == null)
               {
                    scoreManager = new ScoreManager(this.Game);
                    scoreManager.AddPlayer(this);
               }
               else
               {
                    if (!scoreManager.IsPlayerAlreadyAdded(this))
                    {
                         scoreManager.AddPlayer(this);
                    }
               }

               if (livesManager == null)
               {
                    livesManager = new LivesManager(this.GameScreen);
                    livesManager.AddPlayer(this);
               }
               else
               {
                    if (!livesManager.IsPlayerAlreadyAdded(this))
                    {
                         livesManager.AddPlayer(this);
                    }
               }

               base.Initialize();
          }

          public override void Update(GameTime i_GameTime)
          {
               if (IsAlive)
               {
                    if (r_InputManager.KeyboardState.IsKeyDown(MoveLeftKey))
                    {
                         Velocity = r_Velocity * Sprite.Left;
                    }
                    else if (r_InputManager.KeyboardState.IsKeyDown(MoveRightKey))
                    {
                         Velocity = r_Velocity * Sprite.Right;
                    }
                    else
                    {
                         Velocity = Vector2.Zero;
                    }

                    if (IsMouseControllable)
                    {
                         Vector2 mouseDelta = r_InputManager.MousePositionDelta;

                         if (mouseDelta != Vector2.Zero)
                         {
                              Position += mouseDelta;
                         }
                    }
               }
               else
               {
                    Velocity = Vector2.Zero;
               }

               base.Update(i_GameTime);
          }

          public override void Collided(ICollidable i_Collidable)
          {
               BaseBullet bullet = i_Collidable as BaseBullet;
               Enemy enemy = i_Collidable as Enemy;

               if (bullet != null)
               {
                    OnCollidedWithBullet(bullet);
               }
               else if (enemy != null)
               {
                    OnCollidedWithEnemy(enemy);
               }
          }

          protected virtual void OnCollidedWithBullet(BaseBullet i_Bullet)
          {
               if (PlayerCollided != null)
               {
                    PlayerCollided.Invoke(this, i_Bullet);
               }
          }

          protected virtual void OnCollidedWithEnemy(Enemy i_Enemy)
          {
               if (PlayerCollided != null)
               {
                    PlayerCollided.Invoke(this, i_Enemy);
               }
          }
     }
}