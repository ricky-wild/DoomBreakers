using UnityEngine;

namespace DoomBreakers
{
	public class PlayerMove : BaseState, IPlayerMove
	{
		public PlayerMove(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			print("\nMove State.");
		}

		public override void IsMoving(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref IPlayerCollision playerCollider)
		{
			animator.Play("Run");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			DetectFaceDirection(ref playerSprite, ref playerCollider);
			print("\nplayerSprite.GetSpriteDirection()=" + playerSprite.GetSpriteDirection());
			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
			//base.UpdateBehaviour();
		}

		private void DetectFaceDirection(ref IPlayerSprite playerSprite, ref IPlayerCollision playerCollider)
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