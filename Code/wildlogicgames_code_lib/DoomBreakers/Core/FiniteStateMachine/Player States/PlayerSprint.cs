using UnityEngine;

namespace DoomBreakers
{
	public class PlayerSprint : BaseState, IPlayerMove
	{
		public PlayerSprint(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_sprintSpeed = 1.75f;
			_behaviourTimer = new Timer();
		}

		public override void IsSprinting(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider)
		{
			animator.Play("Sprint");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			DetectFaceDirection(ref playerSprite, ref playerCollider);

			

			_behaviourTimer.StartTimer(0.25f);
			if (_behaviourTimer.HasTimerFinished())
				playerSprite.SetBehaviourTextureFlash(0.2f, Color.white);

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}

		private void DetectFaceDirection(ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider)
		{
			if (_velocity.x < 0f)
			{
				if (playerSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
				{
					playerSprite.FlipSprite();
					playerCollider.FlipAttackPoints(-1);
				}
				return;
			}
			if (_velocity.x > 0f)
			{
				if (playerSprite.GetSpriteDirection() == -1)
				{
					playerSprite.FlipSprite();
					playerCollider.FlipAttackPoints(1);
				}
				return;
			}
		}
	}
}