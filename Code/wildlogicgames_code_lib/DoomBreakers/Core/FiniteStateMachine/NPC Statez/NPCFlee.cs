using UnityEngine;

namespace DoomBreakers
{
	public class NPCFlee : BasicNPCBaseState
	{
		private bool _getOppositeTravelDir;
		private int _travelDir;
		public NPCFlee(NPCStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, npcId: id)//=> _stateMachine = s; 
		{
			_npcID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.
			_travelDir = 1;
			_getOppositeTravelDir = false;
			_moveSpeed = wildlogicgames.Utilities.GetRandomNumberInt(3, 5);//4.0f;//3.75f;//3.5f;
			_moveSpeed = _moveSpeed - 0.5f;
			_sprintSpeed = 1.44f;
			_idleWaitTime = 3.33f;
			_behaviourTimer = new Timer();
			_cachedVector3 = new Vector3();
			_cachedVector3 = _transform.position;
			_cachedVector3.x += 6.66f;
			//print("\nIdle State.");
		}

		public override void IsFleeing(ref NPCAnimator animator, ref NPCSprite npcSprite)
		{

			animator.PlayAnimation((int)NPCAnimID.Flee);
			_velocity.x = _targetVelocityX;

			if(!_getOppositeTravelDir)
			{
				_getOppositeTravelDir = true;
				npcSprite.SetBehaviourTextureFlash(0.25f, Color.white);

				if (npcSprite.GetSpriteDirection() == 1)
				{		
					_cachedVector3 = _transform.position;
					_cachedVector3.x -= 6.66f;
					_travelDir = -1;
					return;
				}
				if (npcSprite.GetSpriteDirection() == -1)
				{
					_cachedVector3 = _transform.position;
					_cachedVector3.x += 6.66f;
					_travelDir = 1;
					return;
				}
				
			}
			DetectFaceDirection(ref npcSprite);

			if (_travelDir == 1)
			{
				if (_transform.position.x < _cachedVector3.x)// + _attackDist) 
					_targetVelocityX = (_moveSpeed * _sprintSpeed);
				else
					CheckSetForJumpState();// ref npcSprite);
			}
			if (_travelDir == -1)
			{
				if (_transform.position.x > _cachedVector3.x)// + _attackDist) 
					_targetVelocityX = -(_moveSpeed * _sprintSpeed);
				else
					CheckSetForJumpState();// ref npcSprite);
			}


			CheckSetForFallState();
		}
		private void CheckSetForJumpState()//ref CharacterStat npcStats)
		{
			//CheckSetForFallState();
			if (_transform.position.y < _cachedVector3.y - (_attackDist * 1.25f))
			{
				//_stateMachine.SetState(new NPCJump(_stateMachine, _velocity, _transform, _npcID));
			}
			else
				_stateMachine.SetState(new NPCIdle(_stateMachine, _velocity, _transform, _npcID));
		}
		private void CheckSetForFallState()
		{
			if (Mathf.Abs(_velocity.y) >= 3.0f)
			{
				_stateMachine.SetState(new NPCFall(_stateMachine, _velocity, _transform, _npcID, true));
				//_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _npcID, false));
				return;
			}
		}
		private void DetectFaceDirection(ref NPCSprite npcSprite)//, ref IBanditCollision banditCollider)
		{
			if (_velocity.x < 0f)
			{
				if (npcSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
				{
					npcSprite.FlipSprite();
					//banditCollider.FlipAttackPoints(-1);
				}
				return;
			}
			if (_velocity.x > 0f)
			{
				if (npcSprite.GetSpriteDirection() == -1)
				{
					npcSprite.FlipSprite();
					//banditCollider.FlipAttackPoints(1);
				}
				return;
			}
		}
	}
}
