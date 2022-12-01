
using UnityEngine;

namespace DoomBreakers
{
	public class BanditFall : BanditBaseState
	{
		private int _playerFaceDir;
		private bool _receivedFaceDirFlag;
		public BanditFall(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_playerFaceDir = 0; // -1 or 1
			_receivedFaceDirFlag = false; //Used to get the face dir of player and this enemy during the attack.
			_behaviourTimer = new Timer();
			//print("\nFall State.");
		}

		public override void IsFalling(ref Animator animator, ref Controller2D controller2D, ref IBanditSprite banditSprite)
		{
			if (_velocity.y <= _maxJumpVelocity/2)
				animator.Play("Fall");
			else
				animator.Play("Hit");//, 0, 0.0f);
			

			if(!_receivedFaceDirFlag)
			{
				int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
				_playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);
				_receivedFaceDirFlag = true;
			}

			if(_playerFaceDir == 1)
				_velocity.x = (_moveSpeed * _sprintSpeed);
			if(_playerFaceDir == -1)
				_velocity.x = -(_moveSpeed * _sprintSpeed);


			if (controller2D.collisions.below) //Means we're finished jumping/falling.
			{
				_velocity.x = 0f;
				_velocity.y = 0f;
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity,_banditID));
			}
			//base.UpdateBehaviour();
		}
	}
}

