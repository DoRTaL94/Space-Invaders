using Microsoft.Xna.Framework;
using Space_Invaders.Interfaces;
using Space_Invaders.Screens;

namespace Space_Invaders.Models
{
     public abstract class Enemy : Entity, ICollidable2D
     {
          private const int k_CallOrder = 5;
          private int m_Score;

          public Enemy(string i_AssetName, GameScreen i_GameScreen) 
               : base(i_AssetName, i_GameScreen, k_CallOrder)
          {
               this.StartVelocity       = new Vector2(32, 0);
               this.Lives               = 1;
               this.ViewDirection       = Sprite.Down;
               this.GroupRepresentative = this;
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

          public object GroupRepresentative { get; set; }
     }
}