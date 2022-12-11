using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class PlayerDodged : BaseState//, IPlayerDodge
	{

		private float _dodgedXValue;
		public PlayerDodged(StateMachine s, Vector3 v, bool dodgedLeft) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_dodgedLeftFlag = dodgedLeft;
			_dodgedXValue = 100.0f;
			//print("\nDodged State.");
		}

		public override void IsDodged(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input)
		{
			animator.Play("Dodge");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));

			if (_dodgedLeftFlag)
				_velocity.x -= _dodgedXValue;
			if (!_dodgedLeftFlag)
				_velocity.x += _dodgedXValue;

			_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}