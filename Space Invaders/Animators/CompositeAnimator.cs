using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Space_Invaders.Models;

namespace Space_Invaders.Animators
{
     public class CompositeAnimator : SpriteAnimator
     {
          private const string k_Name                                              = "AnimationsManager";
          private readonly Dictionary<string, SpriteAnimator> AnimationsDictionary = new Dictionary<string, SpriteAnimator>();
          protected readonly List<SpriteAnimator> AnimationsList                   = new List<SpriteAnimator>();

          public CompositeAnimator(Sprite i_BoundSprite)
               : this(k_Name, TimeSpan.Zero, i_BoundSprite, new SpriteAnimator[] { })
          {
               this.Enabled = false;
          }
        
          public CompositeAnimator(string i_Name, TimeSpan i_AnimationLength, Sprite i_BoundSprite, params SpriteAnimator[] i_Animations)
               : base(i_Name, i_AnimationLength)
          {
               this.BoundSprite = i_BoundSprite;

               foreach (SpriteAnimator animation in i_Animations)
               {
                    this.Add(animation);
               }
          }

          public void Add(SpriteAnimator i_Animation)
          {
               i_Animation.BoundSprite = this.BoundSprite;
               i_Animation.Enabled = true;
               AnimationsDictionary.Add(i_Animation.Name, i_Animation);
               AnimationsList.Add(i_Animation);
          }

          public void Remove(string i_AnimationName)
          {
               SpriteAnimator animationToRemove;
               AnimationsDictionary.TryGetValue(i_AnimationName, out animationToRemove);

               if (animationToRemove != null)
               {
                    AnimationsDictionary.Remove(i_AnimationName);
                    AnimationsList.Remove(animationToRemove);
               }
          }

          public SpriteAnimator this[string i_Name]
          {
               get
               {
                    SpriteAnimator retVal = null;
                    AnimationsDictionary.TryGetValue(i_Name, out retVal);
                    return retVal;
               }            
          }

          public override void Restart()
          {
               base.Restart();

               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Restart();
               }
          }

          public override void Restart(TimeSpan i_AnimationLength)
          {
               base.Restart(i_AnimationLength);

               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Restart();
               }
          }

          public override void Pause()
          {
               base.Pause();

               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Pause();
               }
          }

          public override void Resume()
          {
               base.Resume();

               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Resume();
               }
          }

          protected override void RevertToOriginal()
          {
               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Reset();
               }
          }

          protected override void CloneSpriteInfo()
          {
               base.CloneSpriteInfo();

               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.m_OriginalSpriteInfo = m_OriginalSpriteInfo;
               }
          }

          protected override void DoFrame(GameTime i_GameTime)
          {
               foreach (SpriteAnimator animation in AnimationsList)
               {
                    animation.Update(i_GameTime);
               }
          }
     }
}
