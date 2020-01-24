using Microsoft.Xna.Framework;

namespace Space_Invaders.Interfaces
{
     public interface ICollidable3D : ICollidable
     {
          BoundingBox Bounds { get; }

          Vector3 Velocity { get; }
     }
}
