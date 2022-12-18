
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class BanditHitDefending : BasicEnemyBaseState
	{

		public BanditHitDefending(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 0.44f;
			_behaviourTimer = new Timer();

			AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyDefenseHitSFX);
			//print("\nFall State.");
		}

		public override void IsHitWhileDefending(ref Animator animator, ref CharacterController2D controller2D, ref IBanditSprite banditSprite)
		{
			animator.Play("DefHit");
			_velocity.x = 0f;

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_DustHitFX, _transform, _enemyID, banditSprite.GetSpriteDirection());
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _enemyID));
			}
			//base.UpdateBehaviour();
		}
	}
}

