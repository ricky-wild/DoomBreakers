
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class BanditFall : BanditBaseState
	{
		private int _playerFaceDir;
		private bool _receivedFaceDirFlag;
		private int _randomStateDir;
		private bool _fromJumpState;

		public BanditFall(EnemyStateMachine s, Vector3 v, int id, bool fromJump) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
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
				//_cachedVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_banditID, EnemyAI.Bandit).position;

				//if( _cachedVector3.y < _transform.position.y){}

			}


			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (collisionBelow) //Means we're finished jumping/falling.
			{
				_velocity.x = 0f;
				_velocity.y = 0f;

				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 50)
					_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity,_banditID));
				else if (_randomStateDir > 50 && _randomStateDir < 80)
					_stateMachine.SetState(new BanditHoldAttack(_stateMachine, _velocity, _banditID));
				else
					_stateMachine.SetState(new BanditDefending(_stateMachine, _velocity, _banditID));
			}
			//base.UpdateBehaviour();
		}
	}
}

