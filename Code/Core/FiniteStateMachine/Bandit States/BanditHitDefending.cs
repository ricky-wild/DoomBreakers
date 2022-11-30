
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitDefending : BanditBaseState
	{

		public BanditHitDefending(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 1.0f;
			_behaviourTimer = new Timer();
			//print("\nFall State.");
		}

		public override void IsDefending(ref Animator animator, ref Controller2D controller2D, ref IBanditSprite banditSprite)
		{
			animator.Play("DefQuickHit");
			_velocity.x = 0f;

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _banditID));
			}
			//base.UpdateBehaviour();
		}
	}
}

