
using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherShoot : BasicEnemyBaseState
	{
		private Transform _playerTargetTransform;
		private Transform _arrowTargetTransform;
		private Vector3 _targetVector3;
		private int _randomStateDir, _faceDir;
		public BanditArcherShoot(BasicEnemyStateMachine s, Vector3 v, ref Transform t, ref Transform arrowAimTransform, Transform playerTargetTransform, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_arrowTargetTransform = arrowAimTransform;
			if(arrowAimTransform == null)
				_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));
			_targetVector3 = new Vector3();
			_playerTargetTransform = playerTargetTransform;
			//_targetVector3 = arrowAimTransform.position;
			_idleWaitTime = 0.75f;//shoot anim length
			_randomStateDir = 0;
			_behaviourTimer = new Timer();
			_faceDir = -1;
			//Get Target position.
			//_targetVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

			//print("\nIdle State.");
		}

		public override void IsShootTarget(ref Animator animator, ref IBanditSprite banditSprite, ref ArcherCollision banditCollider, ref Transform arrowAimTransform)
		{
			if(arrowAimTransform == null) _stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID)); 

			animator.Play("Shoot");//, 0, 0.0f);
			_velocity.x = 0f;

			DetectFaceDirection(ref banditSprite, ref banditCollider);

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				if (banditCollider.IsDisableShootFlag())
					_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));

				CalculateArrowRotation(banditSprite.GetSpriteDirection(), ref arrowAimTransform);

				//Obj Pool Arrow here.
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_ArrowShotFX, arrowAimTransform, _enemyID, _faceDir);// - banditSprite.GetSpriteDirection());
				AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyQuickAttackSFX);


				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 40)
				{
					banditCollider.EnableTargetCollisionDetection();
					_stateMachine.SetState(new BanditArcherShoot(_stateMachine, _velocity, ref _transform, ref _arrowTargetTransform, AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.BanditArcher), _enemyID));
				}
				else
				{
					banditCollider.IsDisableShootFlag(true);
					_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));
				}

			}
		}
		private void CalculateArrowRotation(int faceDir, ref Transform arrowAimTransform)
		{
			//Get Target position.
			//_targetVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position; //AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.BanditArcher);

			_cachedVector3 = (arrowAimTransform.position - _playerTargetTransform.position);//(arrowAimTransform.position - _targetVector3);

			//Offset Arrow Spawn transform to a more convincing point (closer to the bow in sprite)
			//_cachedVector3.y += 0.33f;//-= 0.05f;
			if (faceDir == 1) _cachedVector3.x += 0.75f;
			if (faceDir == -1) _cachedVector3.x -= 0.75f;

			float angle = Mathf.Atan2(-_cachedVector3.y, -_cachedVector3.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			arrowAimTransform.rotation = Quaternion.Slerp(arrowAimTransform.rotation, rotation, Time.deltaTime * 10000);
		}

		private void DetectFaceDirection(ref IBanditSprite banditSprite, ref ArcherCollision banditCollider)
		{
			if (_playerTargetTransform.position.x > _transform.position.x)
			{
				if (banditSprite.GetSpriteDirection() == -1) banditSprite.FlipSprite();
				_faceDir = 1;
				return;
			}
			if (_playerTargetTransform.position.x < _transform.position.x)
			{
				if (banditSprite.GetSpriteDirection() == 1) banditSprite.FlipSprite();
				_faceDir = -1;
				return;
			}
		}
	}
}

