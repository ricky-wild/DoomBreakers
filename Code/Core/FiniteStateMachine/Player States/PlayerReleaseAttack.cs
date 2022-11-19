using UnityEngine;

namespace DoomBreakers
{
	public class PlayerReleaseAttack : BaseState, IPlayerReleaseAttack
	{
		public PlayerReleaseAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
						   //_quickAttackIncrement = quickAttackIncrement;
			_behaviourTimer = new Timer();
			print("\nRelease Attack State.");
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
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));

			//_behaviourTimer.StartTimer(0.01f);
			//if (_behaviourTimer.HasTimerFinished())
			//	playerSprite.SetWeaponChargeTextureFXFlag(true);
			//base.UpdateBehaviour();
		}
	}
}

