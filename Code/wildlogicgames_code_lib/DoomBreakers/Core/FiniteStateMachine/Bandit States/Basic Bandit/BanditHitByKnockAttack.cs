﻿
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByKnockAttack : BasicEnemyBaseState
	{

		public BanditHitByKnockAttack(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_maxPowerStruckVelocityX = 10.0f;
			_maxPowerStruckVelocityY = 10.0f;
			_detectPlatformEdge = false;
		}

		public override void IsHitByKnockAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Hit");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.5f, Color.red);

			int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
			int banditFaceDir = banditSprite.GetSpriteDirection();
			int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);

			float multiplier = 0.75f;// 1.66f;


			
			_targetVelocityX = multiplier * multiplier;
			_velocity.y += _targetVelocityX * 1.5f;

			if (banditFaceDir == 1 && playerFaceDir == -1) _velocity.x -= _targetVelocityX;
			if (banditFaceDir == -1 && playerFaceDir == 1) _velocity.x += _targetVelocityX;
			if (banditFaceDir == 1 && playerFaceDir == 1) _velocity.x += _targetVelocityX;
			if (banditFaceDir == -1 && playerFaceDir == -1) _velocity.x -= _targetVelocityX;

			if (Mathf.Abs(_velocity.x) >= _maxPowerStruckVelocityX)
			{
				banditSprite.SetBehaviourTextureFlash(0.1f, Color.red);
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));
				return;
			}
			//else
			//	_velocity.y += _targetVelocityX * multiplier; 



			//base.UpdateBehaviour();
		}
	}
}

