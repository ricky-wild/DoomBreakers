
using UnityEngine;

namespace DoomBreakers
{
	public class BanditDead : BasicEnemyBaseState
	{

		public BanditDead(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 2.0f;
			_detectPlatformEdge = false;
			_behaviourTimer = new Timer();
			_behaviourTimer.StartTimer(_idleWaitTime);
			//print("\nIdle State.");
		}

		public override void IsDead(ref Animator animator, ref IBanditSprite banditSprite)
		{
			animator.Play("Dead");//, 0, 0.0f);
			_velocity.x = 0f;
				
			if (_behaviourTimer.HasTimerFinished(true)) banditSprite.SetBehaviourTextureFlash(0.75f, Color.black);


			//base.UpdateBehaviour();
		}
	}
}

