using UnityEngine;

namespace DoomBreakers
{
	public class NPCJump : BasicNPCBaseState
	{
		public NPCJump(NPCStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, npcId: id)//=> _stateMachine = s; 
		{
			_npcID = id;
			_stateMachine = s;
			_transform = transform;
			_velocity = v; //We want to carry this on between states.
			_maxPowerStruckVelocityY = 11.0f;
		}

		public override void IsJumping(ref NPCAnimator animator, ref NPCSprite npcSprite)
		{
			animator.PlayAnimation((int)NPCAnimID.Jump);

			float multiplier = 1.2f;
			if (_velocity.y >= (_maxPowerStruckVelocityY))//Near peak of jump velocity, set falling state.
			{
				npcSprite.SetBehaviourTextureFlash(0.25f, Color.white);
				_stateMachine.SetState(new NPCFall(_stateMachine, _velocity, _transform, _npcID, true));
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY - multiplier * 6;
				_targetVelocityX = (_randSpeedModifier * ((_moveSpeed / 4) * _sprintSpeed));

				if (_targetVelocityX > 0) _velocity.x += _targetVelocityX;
				if (_targetVelocityX < 0) _velocity.x -= _targetVelocityX;
			}

			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));


		}
	}
}
