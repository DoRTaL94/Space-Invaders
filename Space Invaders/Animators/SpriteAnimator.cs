using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Models;

namespace Space_Invaders.Animators
{
     public abstract class SpriteAnimator
     {
          public event EventHandler Finished;

          private bool m_IsFinished         = false;
          private bool m_Initialized        = false;
          protected bool m_ResetAfterFinish = true;
          private TimeSpan m_TimeLeft;
          protected internal Sprite m_OriginalSpriteInfo;

          protected SpriteAnimator(string i_Name, TimeSpan i_AnimationLength)
          {
               Name = i_Name;
               AnimationLength = i_AnimationLength;
          }

          protected virtual void OnFinished()
          {
               if (m_ResetAfterFinish)
               {
                    Reset();
                    this.m_IsFinished = true;
               }

               if (Finished != null)
               {
                    Finished(this, EventArgs.Empty);
               }
          }

          protected internal Sprite BoundSprite { get; set; }

          public string Name { get; private set; }

          public bool Enabled { get; set; }

          public bool IsFinite
          {
               get { return this.AnimationLength != TimeSpan.Zero; }
          }

          public bool ResetAfterFinish
          {
               get { return m_ResetAfterFinish; }
               set { m_ResetAfterFinish = value; }
          }

          public virtual void Initialize()
          {
               if (!m_Initialized)
               {
                    m_Initialized = true;

                    CloneSpriteInfo();

                    Reset();
               }
          }

          protected virtual void CloneSpriteInfo()
          {
               if (m_OriginalSpriteInfo == null)
               {
                    m_OriginalSpriteInfo = BoundSprite.ShallowClone();
               }
          }

          public void Reset()
          {
               Reset(AnimationLength);
          }

          public void Reset(TimeSpan i_AnimationLength)
          {
               if (!m_Initialized)
               {
                    Initialize();
               }
               else
               {
                    AnimationLength = i_AnimationLength;
                    m_TimeLeft = AnimationLength;
                    this.IsFinished = false;
               }

               RevertToOriginal();
          }

          protected abstract void RevertToOriginal();

          public virtual void Pause()
          {
               this.Enabled = false;
          }

          public virtual void Resume()
          {
               Enabled = true;
          }

          public virtual void Restart()
          {
               Restart(AnimationLength);
          }

          public virtual void Restart(TimeSpan i_AnimationLength)
          {
               Reset(i_AnimationLength);
               Resume();
          }

          protected TimeSpan AnimationLength { get; private set; }
      
          public bool IsFinished
          {
               get { return this.m_IsFinished; }

               protected set
               {
                    if (value != m_IsFinished)
                    {
                         m_IsFinished = value;

                         if (m_IsFinished == true)
                         {
                              OnFinished();
                         }
                    }
               }
          }

          public void Update(GameTime i_GameTime)
          {
               if (!m_Initialized)
               {
                    Initialize();
               }

               if (this.Enabled && !this.IsFinished)
               {
                    if (this.IsFinite)
                    {
                         m_TimeLeft -= i_GameTime.ElapsedGameTime;

                         if (m_TimeLeft.TotalSeconds < 0)
                         {
                              this.IsFinished = true;
                         }
                    }

                    if (!this.IsFinished)
                    {
                         DoFrame(i_GameTime);
                    }
               }
          }

          protected abstract void DoFrame(GameTime i_GameTime);
     }
}
