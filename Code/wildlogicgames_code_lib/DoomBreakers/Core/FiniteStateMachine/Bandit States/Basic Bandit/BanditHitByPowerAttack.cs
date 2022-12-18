
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByPowerAttack : BasicEnemyBaseState
	{

		public BanditHitByPowerAttack(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_maxPowerStruckVelocityY = 10.0f; //10.0f for lowest impact. 14.0f for average. 16.0f for maximum impact.
			_maxPowerStruckVelocityX = 0.75f;
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			_detectPlatformEdge = false;
			AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
			//print("\nHitByPowerAttack State.");
		}

		public override void IsHitByPowerAttack(ref Animator animator, ref IBanditSprite banditSprite, float playerAttackChargeTime)
		{

			animator.Play("Hit");//, 0, 0.0f);

			

			_behaviourTimer.StartTimer(0.6f);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.SetBehaviourTextureFlash(0.5f, Color.red);
				_behaviourTimer.StartTimer(0.6f);
			}

			WeaponChargeHold weaponChargeHoldFlag = WeaponChargeHold.None;
			if (playerAttackChargeTime < 0.5f) weaponChargeHoldFlag = WeaponChargeHold.None;
			if (playerAttackChargeTime > 0.5f && playerAttackChargeTime < 1.5f) weaponChargeHoldFlag = WeaponChargeHold.Minimal;
			if (playerAttackChargeTime > 1.5f && playerAttackChargeTime < 3.0f) weaponChargeHoldFlag = WeaponChargeHold.Moderate;
			if (playerAttackChargeTime > 3.0f) weaponChargeHoldFlag = WeaponChargeHold.Maximal;

			float multiplier = 1.2f;// 1.66f;
			float heightCap = 0f;

			switch (weaponChargeHoldFlag)
			{
				case WeaponChargeHold.None:
					multiplier = 0.5f;
					heightCap = 0.65f;
					_maxPowerStruckVelocityY = 10.0f;
					_maxPowerStruckVelocityX = 0.75f;
					//print("\nWeaponChargeHold.None");
					break;
				case WeaponChargeHold.Minimal:
					multiplier = 0.85f;
					heightCap = 0.485f;
					_maxPowerStruckVelocityY = 10.65f;
					_maxPowerStruckVelocityX = 0.925f;
					//print("\nWeaponChargeHold.Minimal");
					break;
				case WeaponChargeHold.Moderate:
					multiplier = 1.1f;
					heightCap = 0.365f;
					_maxPowerStruckVelocityY = 11.25f;
					_maxPowerStruckVelocityX = 1.15f;
					//print("\nWeaponChargeHold.Moderate");
					break;
				case WeaponChargeHold.Maximal:
					multiplier = 2.0f;
					heightCap = 0.1f;
					_maxPowerStruckVelocityY = 12.0f;
					_maxPowerStruckVelocityX = 1.25f;
					//print("\nWeaponChargeHold.Maximal");
					break;
			}

			int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
			int banditFaceDir = banditSprite.GetSpriteDirection();
			int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);

			if (_velocity.y >= (_maxPowerStruckVelocityY - heightCap))//Near peak of jump velocity, set falling state.//(_maxPowerStruckVelocityY + (multiplier)) - (heightCap))
			{
				banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);
				//_velocity.x = 0f;
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY + multiplier /6;
				_targetVelocityX = _maxPowerStruckVelocityX * (1.25f+ multiplier); //Carries on over to FallProcess() where appropriate.

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

