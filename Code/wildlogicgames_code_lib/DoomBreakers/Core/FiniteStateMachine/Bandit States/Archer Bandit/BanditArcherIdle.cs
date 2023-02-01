
using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherIdle : BasicEnemyBaseState
	{

		public BanditArcherIdle(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 1.5f;
			_behaviourTimer = new Timer();

		}

		public override void IsIdleBowman(ref Animator animator, ref ArcherCollision banditCollider)//Bandit.cs use.
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;


			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				_stateMachine.SetState(new BanditArcherAim(_stateMachine, _velocity, _enemyID));
			}

			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));
		}

	}
}

