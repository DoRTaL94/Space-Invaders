using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Animators.ConcreteAnimators
{
     public class WaypointsAnimator : SpriteAnimator
     {
          private const string k_Name   = "Waypoints";
          private readonly bool r_Loop  = false;
          private readonly float r_VelocityPerSecond;
          private readonly Vector2[] r_Waypoints;
          private int m_CurrentWaypoint = 0;

          public WaypointsAnimator(float i_VelocityPerSecond, TimeSpan i_AnimationLength, bool i_Loop, params Vector2[] i_Waypoints)
              : this(k_Name, i_VelocityPerSecond, i_AnimationLength, i_Loop, i_Waypoints)
          { 
          }

          public WaypointsAnimator(string i_Name, float i_VelocityPerSecond, TimeSpan i_AnimationLength, bool i_Loop, params Vector2[] i_Waypoints)
              : base(i_Name, i_AnimationLength)
          {
               r_VelocityPerSecond = i_VelocityPerSecond;
               r_Waypoints = i_Waypoints;
               r_Loop = i_Loop;
               m_ResetAfterFinish = false;
          }

          protected override void RevertToOriginal()
          {
               m_CurrentWaypoint = 0;
               this.BoundSprite.Position = m_OriginalSpriteInfo.Position;
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               float maxDistance = (float)i_GameTime.ElapsedGameTime.TotalSeconds * r_VelocityPerSecond;
               Vector2 remainingVector = r_Waypoints[m_CurrentWaypoint] - this.BoundSprite.Position;
               
               if (remainingVector.Length() > maxDistance)
               {
                    remainingVector.Normalize();
                    remainingVector *= maxDistance;
               }

               this.BoundSprite.Position += remainingVector;

               if (reachedCurrentWaypoint())
               {
                    lookAtNextWayPoint();
               }
          }

          private void lookAtNextWayPoint()
          {
               if (reachedLastWaypoint() && !r_Loop)
               {
                    IsFinished = true;
               }
               else
               {
                    m_CurrentWaypoint++;
                    m_CurrentWaypoint %= r_Waypoints.Length;
               }
          }

          private bool reachedLastWaypoint()
          {
               return m_CurrentWaypoint == r_Waypoints.Length - 1;
          }

          private bool reachedCurrentWaypoint()
          {
               return this.BoundSprite.Position == r_Waypoints[m_CurrentWaypoint];
          }
     }
}
