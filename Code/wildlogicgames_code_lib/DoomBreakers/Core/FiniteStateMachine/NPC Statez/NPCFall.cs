
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class NPCFall : BasicNPCBaseState
	{
		private int _faceDir;
		private int _randomStateDir;
		private bool _fromJumpState, _receivedFaceDirFlag;

		public NPCFall(NPCStateMachine s, Vector3 v, Transform transform, int id, bool fromJump) : base(velocity: v, npcId: id)//=> _stateMachine = s; 
		{
			_npcID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = transform;
			_faceDir = 1;
			_randomStateDir = 0;
			_fromJumpState = fromJump;
			_receivedFaceDirFlag = false;
			_behaviourTimer = new Timer();
			//print("\nFall State.");
		}

		public override void IsFalling(ref NPCAnimator animator, ref CharacterController2D controller2D, ref NPCSprite npcSprite)
		{
			if (_velocity.y <= _maxJumpVelocity / 2)
				animator.PlayAnimation((int)NPCAnimID.Fall);
			else
				animator.PlayAnimation((int)NPCAnimID.Jump);

			if (!_fromJumpState)
			{
				if (!_receivedFaceDirFlag)
				{
					_faceDir = npcSprite.GetSpriteDirection();
					_receivedFaceDirFlag = true;
				}

				if (_faceDir == 1)
					_velocity.x = (_moveSpeed * _sprintSpeed);
				if (_faceDir == -1)
					_velocity.x = -(_moveSpeed * _sprintSpeed);
			}
			if (_fromJumpState)
			{
				//_cachedVector3 = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.Bandit).position;

				//if( _cachedVector3.y < _transform.position.y){}

			}


			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (collisionBelow) //Means we're finished jumping/falling.
			{

				//ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_JumpingDustFX, _transform, _enemyID, banditSprite.GetSpriteDirection());
				_velocity.x = 0f;
				_velocity.y = 0f;

				_randomStateDir = wildlogicgames.Utilities.GetRandomNumberInt(0, 100);

				if (_randomStateDir < 50)
					_stateMachine.SetState(new NPCIdle(_stateMachine, _velocity, _transform, _npcID));
				else //(_randomStateDir > 50 && _randomStateDir < 80)
					_stateMachine.SetState(new NPCTravelling(_stateMachine, _velocity, _transform, _npcID));
				//else
				//	_stateMachine.SetState(new BanditDefending(_stateMachine, _velocity, _enemyID));
			}
			//base.UpdateBehaviour();
		}
	}
}
