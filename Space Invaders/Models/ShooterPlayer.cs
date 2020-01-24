using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Interfaces;
using Space_Invaders.Models.BaseModels;
using Space_Invaders.Screens;
using Space_Invaders.Managers;

namespace Space_Invaders.Models
{
     public abstract class ShooterPlayer : BasePlayer
     {
          private const string k_ShootSoundName = "SSGunShot";
          private const int k_MaxShotInMidAir = 2;
          private ISoundManager m_SoundManager;
          private BaseGun m_Gun;

          public ShooterPlayer(string i_AssetName, GameScreen i_GameScreen) 
               : this(i_AssetName, i_GameScreen, int.MaxValue)
          {
          }

          public ShooterPlayer(string i_AssetName, GameScreen i_GameScreen, int i_CallsOrder) 
               : this(i_AssetName, i_GameScreen, int.MaxValue, int.MaxValue)
          {
          }

          public ShooterPlayer(string i_AssetName, GameScreen i_GameScreen, int i_UpdateOrder, int i_DrawOrder) 
               : base(i_AssetName, i_GameScreen, i_UpdateOrder, i_DrawOrder)
          {
               m_SoundManager = this.Game.Services.GetService(typeof(SoundManager)) as ISoundManager;
               this.Gun = new Gun(k_MaxShotInMidAir, this, Bullet_Collided);
          }

          public override void ResetProperties()
          {
               this.Gun.Reset();
               base.ResetProperties();
          }

          public BaseGun Gun
          {
               get
               {
                    return m_Gun;
               }

               set
               {
                    if (m_Gun != null)
                    {
                         m_SoundManager.RemoveSoundEmitter(m_Gun);
                    }

                    m_Gun = value;
                    m_Gun.ShootSoundName = k_ShootSoundName;
                    m_SoundManager.AddSoundEmitter(m_Gun);
               }
          }

          public Keys ShootKey { get; set; } = Keys.U;

          public eInputButtons MouseShootButton { get; set; } = eInputButtons.Left;

          public override void Update(GameTime i_GameTime)
          {
               if (IsAlive)
               {
                    if (r_InputManager.KeyPressed(ShootKey) ||
                              (IsMouseControllable && r_InputManager.ButtonPressed(MouseShootButton)))
                    {
                         Gun.Shoot();
                    }
               }

               base.Update(i_GameTime);
          }

          protected virtual void Bullet_Collided(ICollidable i_Collidable)
          {
               Enemy enemy = i_Collidable as Enemy;

               if (enemy != null)
               {
                    this.Score += enemy.Score;
               }
          }
     }
}
