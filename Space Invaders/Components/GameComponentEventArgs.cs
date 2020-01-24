using System;
using Microsoft.Xna.Framework;

namespace Space_Invaders.Components
{
     public class GameComponentEventArgs<ComponentType> : EventArgs
         where ComponentType : IGameComponent
     {
          public GameComponentEventArgs(ComponentType i_GameComponent)
          {
               GameComponent = i_GameComponent;
          }

          public ComponentType GameComponent { get; private set; }
     }
}
