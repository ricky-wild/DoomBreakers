using UnityEngine;

namespace DoomBreakers
{
	public class PlayerMove : BaseState
	{
		public PlayerMove(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
		}

		public override void IsMoving(ref Animator animator, ref Vector2 input)
		{
			animator.Play("Run");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			//base.UpdateBehaviour();
		}
	}
}