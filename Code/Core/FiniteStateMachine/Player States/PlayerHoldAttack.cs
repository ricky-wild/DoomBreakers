using UnityEngine;

namespace DoomBreakers
{
	public class PlayerHoldAttack : BaseState, IPlayerHoldAttack
	{
		public PlayerHoldAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
						   //_quickAttackIncrement = quickAttackIncrement;
			_behaviourTimer = new Timer();
			//print("\nHold Attack State.");
		}


		public override void IsHoldAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Attack");
			_velocity.x = 0f;
			_velocity.y = 0f;
			
			playerSprite.SetWeaponChargeTextureFXFlag(true);

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));

			_behaviourTimer.StartTimer(0.5f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.SetBehaviourTextureFlash(0.25f, Color.white);
				_behaviourTimer.StartTimer(0.5f);
			}
			//	playerSprite.SetWeaponChargeTextureFXFlag(true);
			//base.UpdateBehaviour();
		}
	}
}