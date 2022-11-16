
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerIdle : BaseState
	{

		public PlayerIdle(StateMachine s) => _stateMachine = s; 

		public override void IsIdle(ref Animator animator)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;
			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine));
			//base.UpdateBehaviour();
		}
	}
}

