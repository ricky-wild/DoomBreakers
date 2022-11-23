
using UnityEngine;

namespace DoomBreakers
{
	public class BanditWaiting : BanditBaseState
	{

		public BanditWaiting(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			print("\nWaiting State.");
		}

		public override void IsWaiting(ref Animator animator)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;
			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));

			if (_behaviourTimer.HasTimerFinished())
			{

			}

			//base.UpdateBehaviour();
		}
	}
}

