

using UnityEngine;

namespace DoomBreakers
{
	public class PlayerHitDefending : BaseState
	{

		public PlayerHitDefending(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			//print("\nHitDefending State.");
		}

		public override void IsHitWhileDefending(ref Animator animator, ref Vector2 input)
		{
			//playerSprite.SetWeaponChargeTextureFXFlag(true);
			animator.Play("DefQuickHit");

			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			if (_velocity.x != 0.0f)
			{
				animator.Play("RunDefend");
			}
			else
			{
				animator.Play("DefQuickHit");//, -1, 0.0f);
			}
			_behaviourTimer.StartTimer(0.25f);
			if (_behaviourTimer.HasTimerFinished())
				_stateMachine.SetState(new PlayerDefend(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}

