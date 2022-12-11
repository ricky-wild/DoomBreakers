
using UnityEngine;

namespace DoomBreakers
{
	public class BanditIdle : BanditBaseState
	{

		public BanditIdle(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 0.44f;
			_behaviourTimer = new Timer();
			//print("\nIdle State.");
		}

		public override void IsIdle(ref Animator animator, ref IBanditCollision banditCollider)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditCollider.EnableTargetCollisionDetection(); //Begin player detection & trigger PersueTarget() Method here.
			}
			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID, false));

			//base.UpdateBehaviour();
		}
	}
}

