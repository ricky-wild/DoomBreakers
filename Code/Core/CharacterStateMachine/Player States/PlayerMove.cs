using UnityEngine;

namespace DoomBreakers
{
	public class PlayerMove : BaseState
	{
		public PlayerMove(StateMachine s) => _stateMachine = s; //{_stateMachine = s;}

		public override void IsMoving(ref Animator animator, ref Vector2 input)
		{
			animator.Play("Run");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			//base.UpdateBehaviour();
		}
	}
}