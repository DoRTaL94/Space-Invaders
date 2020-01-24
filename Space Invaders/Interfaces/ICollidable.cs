using System;
using Space_Invaders.Utils;

namespace Space_Invaders.Interfaces
{
     public interface ICollidable
     {
          event EventHandler<EventArgs> PositionChanged;

          event EventHandler<EventArgs> SizeChanged;

          event EventHandler<EventArgs> VisibleChanged;

          event EventHandler<EventArgs> Disposed;

          bool Visible { get; }

          bool CheckCollision(ICollidable i_Source);

          void Collided(ICollidable i_Collidable);

          object GroupRepresentative { get; set; }

          Texture2DPixels TexturePixels { get; }
     }
}
