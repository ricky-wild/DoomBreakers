
using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherShoot : BasicEnemyBaseState
	{
		private Transform _arrowTransform;
		private Vector3 _targetVector3;
		private int _randomStateDir;
		public BanditArcherShoot(BasicEnemyStateMachine s, Vector3 v, ref Transform t, ref Transform arrowAimTransform, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_arrowTransform = arrowAimTransform;
			_idleWaitTime = 0.75f;//shoot anim length
			_randomStateDir = 0;
			_behaviourTimer = new Timer();

			//Get Target position.
			_targetVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

			//print("\nIdle State.");
		}

		public override void IsShootTarget(ref Animator animator, ref IBanditSprite banditSprite, ref IBanditCollision banditCollider)
		{
			animator.Play("Shoot");//, 0, 0.0f);
			_velocity.x = 0f;

			DetectFaceDirection(ref banditSprite, ref banditCollider);

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				CalculateArrowRotation(banditSprite.GetSpriteDirection());

				//Obj Pool Arrow here.
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_ArrowShotFX, _arrowTransform, _enemyID, banditSprite.GetSpriteDirection());
				AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyQuickAttackSFX);


				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 60)
				{
					banditCollider.EnableTargetCollisionDetection();
					_stateMachine.SetState(new BanditArcherShoot(_stateMachine, _velocity, ref _transform, ref _arrowTransform, _enemyID));
				}
				else
					_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));

			}
		}
		private void CalculateArrowRotation(int faceDir)
		{
			//Get Target position.
			_targetVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

			_cachedVector3 = (_arrowTransform.position - _targetVector3);

			//Offset Arrow Spawn transform to a more convincing point (closer to the bow in sprite)
			_cachedVector3.y -= 0.05f;
			if (faceDir == 1) _cachedVector3.x += 0.75f;
			if (faceDir == -1) _cachedVector3.x -= 0.75f;

			float angle = Mathf.Atan2(-_cachedVector3.y, -_cachedVector3.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			_arrowTransform.rotation = Quaternion.Slerp(_arrowTransform.rotation, rotation, Time.deltaTime * 10000);
		}

		private void DetectFaceDirection(ref IBanditSprite banditSprite, ref IBanditCollision banditCollider)
		{
			if (_targetVector3.x > _transform.position.x)
			{
				if (banditSprite.GetSpriteDirection() == -1) banditSprite.FlipSprite();
				return;
			}
			if (_targetVector3.x < _transform.position.x)
			{
				if (banditSprite.GetSpriteDirection() == 1) banditSprite.FlipSprite();
				return;
			}
		}
	}
}

