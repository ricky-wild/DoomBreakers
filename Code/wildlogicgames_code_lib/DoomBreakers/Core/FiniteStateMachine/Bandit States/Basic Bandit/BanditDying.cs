
using UnityEngine;

namespace DoomBreakers
{
	public class BanditDying : BasicEnemyBaseState
	{

		public BanditDying(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 1.0f; //1.520f dying anim length
			_detectPlatformEdge = false;
			_behaviourTimer = new Timer();
			AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyDeathSFX);
			Time.timeScale = 0.66f;
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
				Time.timeScale = 1.0f;
				_stateMachine.SetState(new BanditDead(_stateMachine, _velocity, _enemyID));
			}


			//base.UpdateBehaviour();
		}
	}
}

