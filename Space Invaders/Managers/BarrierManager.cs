using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Utils;
using Space_Invaders.Components;
using Space_Invaders.Models;
using Space_Invaders.Screens;
using Space_Invaders.Interfaces;

namespace Space_Invaders.Managers
{
     public class BarrierManager : CompositeDrawableComponent<Barrier>
     {
          private const float k_BarrierWidth                      = 44;
          private const float k_BarrierHeight                     = 32;
          private const int k_NumOfBarriers                       = 4;
          private const int k_CallOrder                           = 5;
          private const int k_MaxLevel                            = 5;
          private const float k_StartingXVelocity                 = 45f;
          private const float k_OffsetToChangeDirection           = k_BarrierWidth / 2;
          private const float k_VelocityIncreaseOnLevelTransition = 0.04f;
          private const string k_BarrierAsset                     = @"Sprites\Barrier_44x32";
          private readonly float r_PlayerStartingY;
          private readonly float r_PlayerHeight;
          private readonly List<Barrier> r_Barriers;
          private readonly GameScreen r_GameScreen;
          private readonly ISoundManager r_SoundManager;
          private Texture2D m_BarrierTexture;
          private Texture2DPixels m_SampleBarrierPixels;
          private int m_CurrentLevel = 1;

          public BarrierManager(GameScreen i_GameScreen, float i_PlayerStartingY, float i_PlayerHeight)
               : base(i_GameScreen.Game)
          {
               r_SoundManager    = this.Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
               r_Barriers        = new List<Barrier>(k_NumOfBarriers);
               r_PlayerStartingY = i_PlayerStartingY;
               r_PlayerHeight    = i_PlayerHeight;
               this.UpdateOrder  = k_CallOrder;
               r_GameScreen      = i_GameScreen;
               r_GameScreen.Add(this);
          }

          public override void Initialize()
          {
               createBarriers();

               base.Initialize();
          }

          private void createBarriers()
          {
               Vector2 startingObstaclesPoint = getStartingPosition();

               for (int i = 0; i < k_NumOfBarriers; i++)
               {
                    Barrier barrier = new Barrier(r_GameScreen);
                    setBarrierProperties(barrier, startingObstaclesPoint);
                    r_Barriers.Add(barrier);
                    r_SoundManager.AddSoundEmitter(barrier);
                    startingObstaclesPoint.X += barrier.Width * 2;
               }
          }

          protected override void LoadContent()
          {
               base.LoadContent();

               m_BarrierTexture = Game.Content.Load<Texture2D>(k_BarrierAsset);

               foreach (Barrier barrier in r_Barriers)
               {
                    barrier.Texture = m_BarrierTexture.Clone(this.Game.GraphicsDevice);
               }
          }

          private Vector2 getStartingPosition()
          {
               float posX = (this.Game.GraphicsDevice.Viewport.Width - getObstaclesLength()) / 2;
               float posY = r_PlayerStartingY - k_BarrierHeight - r_PlayerHeight;

               return new Vector2(posX, posY);
          }

          private float getObstaclesLength()
          {
               return (k_NumOfBarriers * k_BarrierWidth) + ((k_NumOfBarriers - 1) * k_BarrierWidth);
          }

          private void setBarrierProperties(Barrier i_Barrier, Vector2 i_Pos)
          {
               i_Barrier.GroupRepresentative = this;
               i_Barrier.StartPosition = i_Pos;
          }

          public override void Update(GameTime i_GameTime)
          {
               if (isBarriersChangeDirection())
               {
                    foreach (Barrier barrier in r_Barriers)
                    {
                         barrier.Velocity *= -1;
                    }
               }

               base.Update(i_GameTime);
          }

          public void ResetAll()
          {
               LevelReset();
               m_CurrentLevel = 1;
          }

          public void LevelReset()
          {
               if (m_SampleBarrierPixels == null)
               {
                    m_SampleBarrierPixels = m_BarrierTexture.GetPixels(m_BarrierTexture.Bounds);
               }

               foreach (Barrier barrier in r_Barriers)
               {
                    for (int row = 0; row < barrier.TexturePixels.Rows; row++)
                    {
                         for(int col = 0; col < barrier.TexturePixels.Cols; col++)
                         {
                              barrier.TexturePixels[row, col] = m_SampleBarrierPixels[row, col];
                         }
                    }

                    barrier.Texture.SetData(barrier.TexturePixels.Pixels);
               }
          }

          public float BarrierXVelocity { get; set; } = k_StartingXVelocity;

          public void UpdateLevelDifficulty()
          {
               if (m_CurrentLevel % k_MaxLevel == 0)
               {
                    BarrierXVelocity = 0;
               }
               else if (m_CurrentLevel % k_MaxLevel == 1)
               {
                    BarrierXVelocity = k_StartingXVelocity;
               }
               else
               {
                    BarrierXVelocity += BarrierXVelocity * k_VelocityIncreaseOnLevelTransition;
               }

               m_CurrentLevel++;

               foreach (Barrier barrier in r_Barriers)
               {
                    barrier.Velocity = new Vector2(BarrierXVelocity, 0);
               }
          }

          private bool isBarriersChangeDirection()
          {
               Barrier representetive = r_Barriers[0];
               Vector2 BarriersRepresentetiveCurrPosition = representetive.Position;
               bool isPassedRightOffsetToChangeBarrierDirection = BarriersRepresentetiveCurrPosition.X >= representetive.StartPosition.X + k_OffsetToChangeDirection;
               bool isPassedLeftOffsetToChangeBarrierDirection = BarriersRepresentetiveCurrPosition.X <= representetive.StartPosition.X - k_OffsetToChangeDirection;

               return isPassedRightOffsetToChangeBarrierDirection || isPassedLeftOffsetToChangeBarrierDirection;
          }
     }
}
