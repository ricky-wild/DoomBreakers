

using UnityEngine;

namespace DoomBreakers
{
	public class BanditHitByQuickAttack : BanditBaseState
	{

		public BanditHitByQuickAttack(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.

			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			print("\nHitByQuickAttack State.");
		}

		public override void IsHitByQuickAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Jabbed");//, 0, 0.0f);

			banditSprite.SetBehaviourTextureFlash(0.1f, Color.red);

			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();
				_targetVelocityX = 0f;
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _banditID));
			}

			//base.UpdateBehaviour();
		}
	}
}

