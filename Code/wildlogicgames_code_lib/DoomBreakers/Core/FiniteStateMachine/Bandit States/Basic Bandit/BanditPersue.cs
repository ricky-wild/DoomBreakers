
using UnityEngine;

namespace DoomBreakers
{
	public class BanditPersue : BasicEnemyBaseState
	{

		public BanditPersue(BasicEnemyStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.

			_detectPlatformEdge = true;

			_randSpeedModifier = wildlogicgames.Utilities.GetRandomNumberInt(1, 3);
			if (_randSpeedModifier == 1) _attackDist = 1.6f;
			if (_randSpeedModifier == 2) _attackDist = 1.8f;
			if (_randSpeedModifier == 3) _attackDist = 2.1f;

			_randSpeedModifier = wildlogicgames.Utilities.GetRandomNumberInt(1, 3);
			_randSpeedModifier = (_randSpeedModifier / 2.15f); 
			_cachedVector3 = new Vector3();
			_behaviourTimer = new Timer();
			//print("\nPersue State.");
		}

		public override void IsPersueTarget(ref Animator animator, ref IBanditSprite banditSprite, ref IBanditCollision banditCollider, ref BanditStats banditStats)
		{
			animator.Play("Run");//, 0, 0.0f);

			_behaviourTimer.StartTimer(1.0f);
			if(_behaviourTimer.HasTimerFinished()) 
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_RunningDustFX, _transform, _enemyID, banditSprite.GetSpriteDirection());

			DetectFaceDirection(ref banditSprite, ref banditCollider);

			int trackingDir = 0; //Face direction either -1 left, or 1 right.
			_cachedVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

			if (_cachedVector3.x > _transform.position.x)
				trackingDir = 1;
			if (_cachedVector3.x < _transform.position.x)
				trackingDir = -1;


			if (trackingDir == -1)
			{
				if (_transform.position.x > _cachedVector3.x + _attackDist)
					_targetVelocityX = -(_randSpeedModifier * (_moveSpeed * _sprintSpeed));
				else
					CheckSetForJumpOrAttackState(ref banditStats);
			}
			if (trackingDir == 1)
			{
				if (_transform.position.x < _cachedVector3.x - _attackDist)
					_targetVelocityX = _randSpeedModifier * (_moveSpeed * _sprintSpeed);
				else
					CheckSetForJumpOrAttackState(ref banditStats);
			}

			if (!banditStats.IsBludgeoning()) _velocity.x = _targetVelocityX;
			if (banditStats.IsBludgeoning()) _velocity.x = _targetVelocityX/4;

			CheckSetForFallState();

			//base.UpdateBehaviour();
		}
		private void CheckSetForJumpOrAttackState(ref BanditStats banditStats)
		{
			CheckSetForFallState();
			if (_transform.position.y < _cachedVector3.y - (_attackDist*1.25f))
			{
				if(!banditStats.IsBludgeoning())
					_stateMachine.SetState(new BanditJump(_stateMachine, _velocity, _transform, _enemyID));
			}
			else
				_stateMachine.SetState(new BanditQuickAttack(_stateMachine, _velocity, _enemyID));
		}
		private void CheckSetForFallState()
		{
			if (Mathf.Abs(_velocity.y) >= 3.0f)
			{
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));
				return;
			}
		}

		private void DetectFaceDirection(ref IBanditSprite banditSprite, ref IBanditCollision banditCollider)
		{
			if (_velocity.x < 0f)
			{
				if (banditSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
				{
					banditSprite.FlipSprite();
					banditCollider.FlipAttackPoints(-1);
				}
				return;
			}
			if (_velocity.x > 0f)
			{
				if (banditSprite.GetSpriteDirection() == -1)
				{
					banditSprite.FlipSprite();
					banditCollider.FlipAttackPoints(1);
				}
				return;
			}
		}
	}
}

