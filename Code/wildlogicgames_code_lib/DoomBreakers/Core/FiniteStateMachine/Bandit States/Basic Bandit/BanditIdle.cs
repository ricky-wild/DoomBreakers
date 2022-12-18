
using UnityEngine;

namespace DoomBreakers
{
	public class BanditIdle : BasicEnemyBaseState
	{

		public BanditIdle(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 0.44f;
			_behaviourTimer = new Timer();
			//print("\nIdle State.");
		}

		public override void IsIdle(ref Animator animator, ref IBanditCollision banditCollider)//Bandit.cs use.
		{
			IsBase(ref animator);
			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
				banditCollider.EnableTargetCollisionDetection(); //Begin player detection & trigger PersueTarget() Method here.
		}
		//public override void IsIdle(ref Animator animator) //=> IsBase(ref animator); //BanditArcher.cs use.
		//{
		//	IsBase(ref animator);

		//	_behaviourTimer.StartTimer(1.0f);
		//	if (_behaviourTimer.HasTimerFinished()) _stateMachine.SetState(new BanditArcherAim(_stateMachine, _velocity, _enemyID));
		//}

		private void IsBase(ref Animator animator)
		{
			animator.Play("Idle");//, 0, 0.0f);
			_velocity.x = 0f;


			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _enemyID, false));

			//base.UpdateBehaviour();
		}
	}
}

