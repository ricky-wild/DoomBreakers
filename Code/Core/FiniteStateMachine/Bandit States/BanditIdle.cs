
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
			_behaviourTimer = new Timer();
			print("\nIdle State.");
		}

		public override void IsIdle(ref Animator animator)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;
			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));

			if (_behaviourTimer.HasTimerFinished())
			{
				//if (_cooldownTimer.HasTimerFinished())
				//	_attackCooldownCounter = 0;
				//banditCollider.EnableTargetCollisionDetection(); //Begin player detection & trigger PersueTarget() Method here.
			}

			//base.UpdateBehaviour();
		}
	}
}

