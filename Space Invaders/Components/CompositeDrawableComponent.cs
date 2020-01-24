using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Models;

namespace Space_Invaders.Components
{
     public abstract class CompositeDrawableComponent<ComponentType> :
     DrawableGameComponent, ICollection<ComponentType>
     where ComponentType : IGameComponent
     {
          public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentAdded;

          public event EventHandler<GameComponentEventArgs<ComponentType>> ComponentRemoved;

          private Collection<ComponentType> m_Components = new Collection<ComponentType>();
          private List<ComponentType> m_UninitializedComponents = new List<ComponentType>();
          protected List<IUpdateable> m_UpdateableComponents = new List<IUpdateable>();
          protected List<IDrawable> m_DrawableComponents = new List<IDrawable>();
          protected List<Sprite> m_Sprites = new List<Sprite>();

          public CompositeDrawableComponent(Game i_Game)
              : base(i_Game)
          {
          }

          protected virtual void OnComponentAdded(GameComponentEventArgs<ComponentType> i_Args)
          {
               if (IsInitialized)
               {
                    initializeComponent(i_Args.GameComponent);
               }
               else
               {
                    m_UninitializedComponents.Add(i_Args.GameComponent);
               }

               IUpdateable updatable = i_Args.GameComponent as IUpdateable;

               if (updatable != null)
               {
                    insertSorted(updatable);
                    updatable.UpdateOrderChanged += new EventHandler<EventArgs>(childUpdateOrderChanged);
               }

               IDrawable drawable = i_Args.GameComponent as IDrawable;

               if (drawable != null)
               {
                    insertSorted(drawable);
                    drawable.DrawOrderChanged += new EventHandler<EventArgs>(childDrawOrderChanged);
               }

               if (ComponentAdded != null)
               {
                    ComponentAdded(this, i_Args);
               }
          }

          public bool IsInitialized { get; set; }

          protected virtual void OnComponentRemoved(GameComponentEventArgs<ComponentType> i_Args)
          {
               if (!IsInitialized)
               {
                    m_UninitializedComponents.Remove(i_Args.GameComponent);
               }

               IUpdateable updatable = i_Args.GameComponent as IUpdateable;

               if (updatable != null)
               {
                    m_UpdateableComponents.Remove(updatable);
                    updatable.UpdateOrderChanged -= childUpdateOrderChanged;
               }

               Sprite sprite = i_Args.GameComponent as Sprite;

               if (sprite != null)
               {
                    m_Sprites.Remove(sprite);
                    sprite.DrawOrderChanged -= childDrawOrderChanged;
               }
               else
               {
                    IDrawable drawable = i_Args.GameComponent as IDrawable;
                    if (drawable != null)
                    {
                         m_DrawableComponents.Remove(drawable);
                         drawable.DrawOrderChanged -= childDrawOrderChanged;
                    }
               }

               if (ComponentRemoved != null)
               {
                    ComponentRemoved(this, i_Args);
               }
          }

          private void childUpdateOrderChanged(object i_Sender, EventArgs i_Args)
          {
               IUpdateable updatable = i_Sender as IUpdateable;
               m_UpdateableComponents.Remove(updatable);

               insertSorted(updatable);
          }

          private void childDrawOrderChanged(object i_Sender, EventArgs i_Args)
          {
               IDrawable drawable = i_Sender as IDrawable;

               Sprite sprite = i_Sender as Sprite;
               if (sprite != null)
               {
                    m_Sprites.Remove(sprite);
               }
               else
               {
                    m_DrawableComponents.Remove(drawable);
               }

               insertSorted(drawable);
          }

          private void insertSorted(IUpdateable i_Updateable)
          {
               int idx = m_UpdateableComponents.BinarySearch(i_Updateable, UpdateableComparer.Default);
               if (idx < 0)
               {
                    idx = ~idx;
               }

               m_UpdateableComponents.Insert(idx, i_Updateable);
          }

          private void insertSorted(IDrawable i_Drawable)
          {
               Sprite sprite = i_Drawable as Sprite;

               if (sprite != null)
               {
                    int idx = m_Sprites.BinarySearch(sprite, DrawableComparer<Sprite>.Default);
                    if (idx < 0)
                    {
                         idx = ~idx;
                    }

                    m_Sprites.Insert(idx, sprite);
               }
               else
               {
                    int idx = m_DrawableComponents.BinarySearch(i_Drawable, DrawableComparer<IDrawable>.Default);
                    if (idx < 0)
                    {
                         idx = ~idx;
                    }

                    m_DrawableComponents.Insert(idx, i_Drawable);
               }
          }

          public override void Initialize()
          {
               if (!IsInitialized)
               {
                    while (m_UninitializedComponents.Count > 0)
                    {
                         initializeComponent(m_UninitializedComponents[0]);
                    }

                    IsInitialized = true;
               }

               base.Initialize();
          }

          private void initializeComponent(ComponentType i_Component)
          {
               Sprite component = i_Component as Sprite;

               if (component != null && component.BlendState == this.BlendState)
               {
                    (i_Component as Sprite).SpriteBatch = SpriteBatch;
               }

               i_Component.Initialize();
               m_UninitializedComponents.Remove(i_Component);
          }

          protected override void LoadContent()
          {
               base.LoadContent();

               SpriteBatch = new SpriteBatch(this.GraphicsDevice);

               foreach (Sprite sprite in m_Sprites)
               {
                    if (sprite.BlendState == this.BlendState)
                    {
                         sprite.SpriteBatch = SpriteBatch;
                    }
               }
          }

          public override void Update(GameTime i_GameTime)
          {
               for (int i = 0; i < m_UpdateableComponents.Count; i++)
               {
                    IUpdateable updateable = m_UpdateableComponents[i];

                    if (updateable.Enabled)
                    {
                         updateable.Update(i_GameTime);
                    }
               }
          }

          public override void Draw(GameTime i_GameTime)
          {
               SpriteBatch.Begin(
                   this.SpritesSortMode, 
                   this.BlendState, 
                   this.SamplerState,
                   this.DepthStencilState, 
                   this.RasterizerState, 
                   this.Shader, 
                   this.TransformMatrix);

               foreach (Sprite sprite in m_Sprites)
               {
                    if (sprite.Visible)
                    {
                         sprite.Draw(i_GameTime);
                    }
               }

               SpriteBatch.End();

               foreach (IDrawable drawable in m_DrawableComponents)
               {
                    if (drawable.Visible)
                    {
                         drawable.Draw(i_GameTime);
                    }
               }
          }

          protected override void Dispose(bool i_IsDisposing)
          {
               if (i_IsDisposing)
               {
                    // Dispose of components in this manager
                    for (int i = 0; i < Count; i++)
                    {
                         IDisposable disposable = m_Components[i] as IDisposable;
                         if (disposable != null)
                         {
                              disposable.Dispose();
                         }
                    }
               }

               base.Dispose(i_IsDisposing);
          }

          public virtual void Add(ComponentType i_Component)
          {
               this.InsertItem(m_Components.Count, i_Component);
          }

          protected virtual void InsertItem(int i_Idx, ComponentType i_Component)
          {
               if (m_Components.IndexOf(i_Component) != -1)
               {
                    throw new ArgumentException("Duplicate components are not allowed in the same GameComponentManager.");
               }

               if (i_Component != null)
               {
                    m_Components.Insert(i_Idx, i_Component);

                    OnComponentAdded(new GameComponentEventArgs<ComponentType>(i_Component));
               }
          }

          public void Clear()
          {
               for (int i = 0; i < Count; i++)
               {
                    OnComponentRemoved(new GameComponentEventArgs<ComponentType>(m_Components[i]));
               }

               m_Components.Clear();
          }

          public bool Contains(ComponentType i_Component)
          {
               return m_Components.Contains(i_Component);
          }

          public void CopyTo(ComponentType[] io_ComponentsArray, int i_ArrayIndex)
          {
               m_Components.CopyTo(io_ComponentsArray, i_ArrayIndex);
          }

          public int Count
          {
               get { return m_Components.Count; }
          }

          public bool IsReadOnly
          {
               get { return false; }
          }

          public virtual bool Remove(ComponentType i_Component)
          {
               bool removed = m_Components.Remove(i_Component);

               if (i_Component != null && removed)
               {
                    OnComponentRemoved(new GameComponentEventArgs<ComponentType>(i_Component));
               }

               return removed;
          }

          public IEnumerator<ComponentType> GetEnumerator()
          {
               return m_Components.GetEnumerator();
          }

          IEnumerator IEnumerable.GetEnumerator()
          {
               return ((IEnumerable)m_Components).GetEnumerator();
          }

          public SpriteBatch SpriteBatch { get; set; }

          public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

          public SpriteSortMode SpritesSortMode { get; set; } = SpriteSortMode.Deferred;

          public SamplerState SamplerState { get; set; }

          public DepthStencilState DepthStencilState { get; set; }

          public RasterizerState RasterizerState { get; set; }

          public Effect Shader { get; set; }

          public Matrix TransformMatrix { get; set; } = Matrix.Identity;

          protected Vector2 CenterOfViewPort
          {
               get
               {
                    return new Vector2((float)Game.GraphicsDevice.Viewport.Width / 2, (float)Game.GraphicsDevice.Viewport.Height / 2);
               }
          }

          public ContentManager ContentManager
          {
               get { return this.Game.Content; }
          }
     }
}
