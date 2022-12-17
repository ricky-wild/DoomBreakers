
using UnityEngine;

namespace DoomBreakers
{
	public class BanditWaiting : BasicEnemyBaseState
	{

		public BanditWaiting(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_cooldownWaitTime = 3.0f;
			_behaviourTimer = new Timer();
			//print("\nWaiting State.");
		}

		public override void IsWaiting(ref Animator animator)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;

			_cooldownTimer.StartTimer(_cooldownWaitTime);
			if (_cooldownTimer.HasTimerFinished())
			{
				_attackCooldownCounter = 0;
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _banditID));
				return;
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID, false));

			//base.UpdateBehaviour();
		}
	}
}

