using UnityEngine;

namespace DoomBreakers
{
	public class PlayerHoldAttack : BaseState, IPlayerHoldAttack
	{
		private Transform _transform;
		public PlayerHoldAttack(StateMachine s, Vector3 v, Transform t) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_behaviourTimer = new Timer();

			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, 1);
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, -1);
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, 1);
			//print("\nHold Attack State.");
		}


		public override void IsHoldAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Attack");
			_velocity.x = 0f;
			_velocity.y = 0f;
			
			playerSprite.SetWeaponChargeTextureFXFlag(true);

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, _transform, ref playerSprite));

			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
			{
				AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerChargeAttackSFX);
				playerSprite.SetBehaviourTextureFlash(0.25f, Color.white);
				_behaviourTimer.StartTimer(0.5f);
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, -1);
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, 1);
			}
			//	playerSprite.SetWeaponChargeTextureFXFlag(true);
			//base.UpdateBehaviour();
		}
	}
}