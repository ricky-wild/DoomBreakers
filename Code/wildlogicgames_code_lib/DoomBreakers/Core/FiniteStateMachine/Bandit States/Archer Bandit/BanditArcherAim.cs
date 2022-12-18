

using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherAim : BasicEnemyBaseState
	{

		public BanditArcherAim(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 1.25f;
			_behaviourTimer = new Timer();


		}

		public override void IsAiming(ref Animator animator, ref IBanditCollision banditCollider, ref IBanditSprite banditSprite)
		{
			animator.Play("Aim");//, 0, 0.0f);
			_velocity.x = 0f;



			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				//_behaviourTimer.Reset();
				//_behaviourTimer.StartTimer(_idleWaitTime);
				banditCollider.EnableTargetCollisionDetection(); //BanditCollision.cs->Finds Player->BanditArcher.cs->DetectedPlayer->BanditArcherShoot.cs
				//AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyQuickAttackSFX);
				_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));
			}
		}

	}
}

