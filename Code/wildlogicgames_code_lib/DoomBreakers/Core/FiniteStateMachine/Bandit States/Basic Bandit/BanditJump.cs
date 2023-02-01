
using UnityEngine;

namespace DoomBreakers
{
	public class BanditJump : BasicEnemyBaseState
	{

		public BanditJump(BasicEnemyStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = transform;
			_maxPowerStruckVelocityY = 7.8f; //7.0f
			_randSpeedModifier = wildlogicgames.Utilities.GetRandomNumberInt(1, 3);
			_randSpeedModifier = (_randSpeedModifier / 2.15f);
			_cachedVector3 = new Vector3();

			AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyJumpSFX);
			ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_JumpingDustFX, _transform, _enemyID, 1);
		}

		public override void IsJumping(ref Animator animator, ref IBanditSprite banditSprite)
		{
			animator.Play("Jump");//, 0, 0.0f);


			float multiplier = 1.2f;// 1.66f;
			int trackingDir = banditSprite.GetSpriteDirection();
			if (AITargetTrackingManager._instance != null)
			{
				Transform t = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit);

				if(t != null) _cachedVector3 = t.position;

			}

			if(_cachedVector3 != null)
			{
				if (_cachedVector3.x > _transform.position.x)
					trackingDir = 1;
				if (_cachedVector3.x < _transform.position.x)
					trackingDir = -1;
			}


			if (_velocity.y >= (_maxPowerStruckVelocityY))//Near peak of jump velocity, set falling state.
			{
				banditSprite.SetBehaviourTextureFlash(0.25f, Color.white);
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, true));
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY - multiplier / 6;
				_targetVelocityX = (_randSpeedModifier * ((_moveSpeed/2) * _sprintSpeed));

				if (trackingDir == 1) _velocity.x += _targetVelocityX;
				if (trackingDir == -1) _velocity.x -= _targetVelocityX;
			}



			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID));

			//base.UpdateBehaviour();
		}
	}
}

