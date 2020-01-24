using Microsoft.Xna.Framework;
using Space_Invaders.Models;

namespace Space_Invaders.Interfaces
{
     public interface ICollisionsManager
     {
          void AddObjectToMonitor(ICollidable i_Collidable);

          bool IsCollideWithWindowEdge(Sprite i_Sprite);

          bool IsCollideWithWindowBottomEdge(Sprite i_Sprite);

          bool IsCollideWithWindowTopEdge(Sprite i_Sprite);

          bool IsCollideWithWindowRightEdge(Sprite i_Sprite);

          bool IsCollideWithWindowLeftEdge(Sprite i_Sprite);

          void GetIntersectedRect(Sprite i_First, Sprite i_Second, out Rectangle o_IntersectedRect);
     }
}
