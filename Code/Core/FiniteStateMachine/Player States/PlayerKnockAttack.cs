
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerKnockAttack : BaseState
	{

		public PlayerKnockAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			//print("\nKnock Attack State.");
		}

		public override void IsKnockAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Knock Attack");//, 0, 0.0f);
									//_velocity.x = 0f;
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed)) / 2;
			playerSprite.SetBehaviourTextureFlash(0.1f, Color.white);


			_behaviourTimer.StartTimer(0.355f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();

				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}

