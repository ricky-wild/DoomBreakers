
using UnityEngine;

namespace DoomBreakers
{
	public class BanditHoldAttack : BasicEnemyBaseState
	{
		private bool _releaseFlag;
		public BanditHoldAttack(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_quickAtkWaitTime = 0.2f;//0.133f;
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			_releaseFlag = false;
			ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_RunningDustFX, _transform, _banditID, 1);
			ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_RunningDustFX, _transform, _banditID, -1);
			//print("\nQuickAttack State.");
		}

		public override void IsHoldAttack(ref Animator animator, ref IBanditSprite banditSprite)
		{

			animator.Play("Attack");
			_velocity.x = 0f;
			_velocity.y = 0f;

			banditSprite.SetWeaponChargeTextureFXFlag(true);

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new BanditFall(_stateMachine, _velocity, _banditID, false));

			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
			{
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_RunningDustFX, _transform, _banditID, 1);
				ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_RunningDustFX, _transform, _banditID, -1);
				banditSprite.SetBehaviourTextureFlash(0.25f, Color.white);
				_behaviourTimer.StartTimer(0.5f);
			}


			if(!_releaseFlag)
			{
				_cooldownTimer.StartTimer(wildlogicgames.Utilities.GetRandomNumberInt(1, 3)); //1 to 3 seconds hold before release.
				_releaseFlag = true;
			}
			if (_cooldownTimer.HasTimerFinished())
				_stateMachine.SetState(new BanditReleaseAttack(_stateMachine, _velocity, _banditID));
			//base.UpdateBehaviour();
		}
	}
}

