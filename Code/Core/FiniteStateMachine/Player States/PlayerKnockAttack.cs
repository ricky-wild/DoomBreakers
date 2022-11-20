
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerKnockAttack : BaseState
	{

		public PlayerKnockAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			print("\nKnock Attack State.");
		}

		public override void IsKnockAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Knock Attack");//, 0, 0.0f);
									//_velocity.x = 0f;
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed)) / 2;
			playerSprite.SetBehaviourTextureFlash(0.66f, Color.red);

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}
	}
}

