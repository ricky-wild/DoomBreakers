
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerIdle : BaseState//, IPlayerIdle
	{

		public PlayerIdle(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			//print("\nIdle State.");
		}

		public override void IsIdle(ref Animator animator,ref IPlayerSprite playerSprite)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;
			if (Mathf.Abs(_velocity.y) >= 3.0f)
			{
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, null, ref playerSprite));
			}
			//base.UpdateBehaviour();
		}
	}
}

