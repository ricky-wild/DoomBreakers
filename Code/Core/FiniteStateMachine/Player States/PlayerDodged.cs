using UnityEngine;

namespace DoomBreakers
{
	public class PlayerDodged : BaseState//, IPlayerDodge
	{
		public PlayerDodged(StateMachine s, Vector3 v, bool dodgedLeft) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_dodgedLeftFlag = dodgedLeft;
			//print("\nDodged State.");
		}

		public override void IsDodged(ref Animator animator, ref Controller2D controller2D, ref Vector2 input)
		{
			animator.Play("Dodge");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));

			if (_dodgedLeftFlag)
				_velocity.x -= 66.0f;
			if (!_dodgedLeftFlag)
				_velocity.x += 66.0f;

			_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}