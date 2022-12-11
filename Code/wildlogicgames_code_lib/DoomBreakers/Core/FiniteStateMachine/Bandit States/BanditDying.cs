
using UnityEngine;

namespace DoomBreakers
{
	public class BanditDying : BanditBaseState
	{

		public BanditDying(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 1.0f; //1.520f dying anim length
			_behaviourTimer = new Timer();
			//print("\nIdle State.");
		}

		public override void IsDying(ref Animator animator, ref IBanditSprite banditSprite)
		{
			animator.Play("Dying");//, 0, 0.0f);
			_velocity.x = 0f;
			banditSprite.SetBehaviourTextureFlash(0.25f, Color.red);

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				_stateMachine.SetState(new BanditDead(_stateMachine, _velocity, _banditID));
			}


			//base.UpdateBehaviour();
		}
	}
}

