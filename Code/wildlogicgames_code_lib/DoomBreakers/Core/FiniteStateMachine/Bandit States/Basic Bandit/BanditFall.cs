
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class BanditFall : BasicEnemyBaseState
	{
		private int _playerFaceDir;
		private bool _receivedFaceDirFlag;
		private int _randomStateDir;
		private bool _fromJumpState;

		public BanditFall(BasicEnemyStateMachine s, Vector3 v, int id, bool fromJump) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_playerFaceDir = 0; // -1 or 1
			_receivedFaceDirFlag = false; //Used to get the face dir of player and this enemy during the attack.
			_randomStateDir = 0;
			_fromJumpState = fromJump;
			_behaviourTimer = new Timer();
			//print("\nFall State.");
		}

		public override void IsFalling(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite)
		{
			if (_velocity.y <= _maxJumpVelocity/2)
				animator.Play("Fall");
			else
				animator.Play("Jump");//, 0, 0.0f);
			
			if(!_fromJumpState)
			{
				if (!_receivedFaceDirFlag)
				{
					int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
					_playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);
					_receivedFaceDirFlag = true;
				}

				if (_playerFaceDir == 1)
					_velocity.x = (_moveSpeed * _sprintSpeed);
				if (_playerFaceDir == -1)
					_velocity.x = -(_moveSpeed * _sprintSpeed);
			}
			if(_fromJumpState)
			{
				//_cachedVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

				//if( _cachedVector3.y < _transform.position.y){}

			}


			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];//0 below , 2 left, 3 right
			//bool collisionLeft = controller2D._collisionDetail._collidedDirection[2];
			//bool collisionRight = controller2D._collisionDetail._collidedDirection[3];

			//if (collisionLeft) _stateMachine.SetState(new BanditPersue(_stateMachine, _velocity, _transform, _enemyID));//return;
			//if (collisionRight) _stateMachine.SetState(new BanditPersue(_stateMachine, _velocity, _transform, _enemyID));//return;

			if (collisionBelow) //Means we're finished jumping/falling.
			{
				//_behaviourTimer.StartTimer(1.0f);
				//if (!_behaviourTimer.HasTimerFinished()) return;

				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_JumpingDustFX, _transform, _enemyID, banditSprite.GetSpriteDirection());
				_velocity.x = 0f;
				_velocity.y = 0f;

				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 50)
					_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity,_enemyID));
				else if (_randomStateDir > 50 && _randomStateDir < 80)
					_stateMachine.SetState(new BanditHoldAttack(_stateMachine, _velocity, _enemyID));
				else
					_stateMachine.SetState(new BanditDefending(_stateMachine, _velocity, _enemyID));
			}
			//base.UpdateBehaviour();
		}
	}
}

