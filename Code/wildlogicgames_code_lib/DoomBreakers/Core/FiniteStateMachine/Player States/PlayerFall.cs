using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class PlayerFall : BaseState, IPlayerFall
	{
		public PlayerFall(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			//print("\nFall State.");
		}
		public override void IsFalling(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input)
		{
			if (_velocity.y <= _maxJumpVelocity)
				animator.Play("Fall");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));

			bool collisionBelow = controller2D._collisionDetail._collidedDirection[0];

			if (collisionBelow) //Means we're finished jumping/falling.
			{			
				_velocity.x = 0f;
				_velocity.y = 0f;
				AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerLandImpactSFX);
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}
			//base.UpdateBehaviour();
		}
	}
}

