using Microsoft.Xna.Framework;

namespace Space_Invaders.Interfaces
{
     public interface ICollidable2D : ICollidable
     {
          Rectangle Bounds { get; }

          Vector2 Velocity { get; }
     }
}
