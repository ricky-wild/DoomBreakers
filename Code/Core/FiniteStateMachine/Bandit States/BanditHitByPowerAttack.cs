
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByPowerAttack : BanditBaseState
	{

		public BanditHitByPowerAttack(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_maxPowerStruckVelocityY = 10.0f; //10.0f for lowest impact. 14.0f for average. 16.0f for maximum impact.
			_maxPowerStruckVelocityX = 0.5f;
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			print("\nHitByPowerAttack State.");
		}

		public override void IsHitByPowerAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Hit");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);

			WeaponChargeHold weaponChargeHoldFlag = WeaponChargeHold.Minimal;
			float multiplier = 1.2f;// 1.66f;
			float heightCap = 0f;

			switch (weaponChargeHoldFlag)
			{
				case WeaponChargeHold.None:
					multiplier = 0.75f;
					heightCap = 1.25f;
					break;
				case WeaponChargeHold.Minimal:
					multiplier = 1.2f;
					heightCap = 1.0f;
					break;
				case WeaponChargeHold.Moderate:
					multiplier = 1.35f;
					heightCap = 0.5f;
					break;
				case WeaponChargeHold.Maximal:
					multiplier = 1.5f;
					heightCap = 0.25f;
					break;
			}

			int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
			int banditFaceDir = banditSprite.GetSpriteDirection();
			int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);

			if (_velocity.y >= _maxPowerStruckVelocityY - heightCap)//Near peak of jump velocity, set falling state.
			{
				banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);
				_velocity.x = 0f;
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID));
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY / 6;// (6-multiplier);// = 0f;
				_targetVelocityX = _maxPowerStruckVelocityX * multiplier; //Carries on over to FallProcess() where appropriate.

				if (banditFaceDir == 1 && playerFaceDir == -1) //Enemy facing right & player facing left, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
				if (banditFaceDir == -1 && playerFaceDir == 1) //Enemy facing left & player facing right, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (banditFaceDir == 1 && playerFaceDir == 1) //Enemy facing right & player facing right behind enemy, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (banditFaceDir == -1 && playerFaceDir == -1) //Enemy facing left & player facing left behind enemy, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
			}



			//base.UpdateBehaviour();
		}
	}
}

