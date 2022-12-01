
using UnityEngine;

namespace DoomBreakers
{
	public class BanditReleaseAttack : BanditBaseState
	{

		public BanditReleaseAttack(EnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
		}

		public override void IsReleaseAttack(ref Animator animator, ref IBanditCollision banditCollider, ref IBanditSprite banditSprite)
		{

			animator.Play("AtkRelease");
			_velocity.x = 0f;
			_velocity.y = 0f;


			_cooldownTimer.StartTimer(_quickAtkWaitTime / 2);
			if (_cooldownTimer.HasTimerFinished())//This needed to execute after collision detection with player is made, as "this" state is required. 
				banditCollider.EnableTargetCollisionDetection(); //Must be set before changing state here.

			banditSprite.SetBehaviourTextureFlash(0.15f, Color.white);


			_behaviourTimer.StartTimer(1.0f); 
			if (_behaviourTimer.HasTimerFinished()) _stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _banditID));
			
			
			if (Mathf.Abs(_velocity.y) >= 3.0f) _stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID));

			//base.UpdateBehaviour();
		}
	}
}

