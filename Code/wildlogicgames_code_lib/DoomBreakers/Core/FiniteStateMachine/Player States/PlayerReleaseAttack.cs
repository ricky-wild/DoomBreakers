using UnityEngine;

namespace DoomBreakers
{
	public class PlayerReleaseAttack : BaseState, IPlayerReleaseAttack
	{
		private Transform _transform;
		public PlayerReleaseAttack(StateMachine s, Vector3 v, Transform t) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_behaviourTimer = new Timer();
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, 1);
			//print("\nRelease Attack State.");
		}


		public override void IsReleaseAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("AtkRelease");
			_velocity.x = 0f;
			_velocity.y = 0f;
			playerSprite.SetBehaviourTextureFlash(0.15f, Color.white);
			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
			{
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, -playerSprite.GetSpriteDirection());
				//AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, null, ref playerSprite));

			//_behaviourTimer.StartTimer(0.01f);
			//if (_behaviourTimer.HasTimerFinished())
			//	playerSprite.SetWeaponChargeTextureFXFlag(true);
			//base.UpdateBehaviour();
		}
	}
}

