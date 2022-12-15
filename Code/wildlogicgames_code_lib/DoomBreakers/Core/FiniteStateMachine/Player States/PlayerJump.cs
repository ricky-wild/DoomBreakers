using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class PlayerJump : BaseState//, IPlayerJump
	{
		private Transform _transform;
		public PlayerJump(StateMachine s, Vector3 v, Transform t, ref IPlayerSprite playerSprite) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, -playerSprite.GetSpriteDirection());
		}
		public override void IsJumping(ref Animator animator, ref CharacterController2D controller2D, ref Vector2 input, ref IPlayerSprite playerSprite)
		{
			animator.Play("Jump");//, -1, 0.0f);
			_velocity.y += _jumpSpeed;
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			if (_velocity.y >= _maxJumpVelocity)//(_maxJumpVelocity / 1.15f)) //Near peak of jump velocity, set falling state.
					_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, _transform, ref playerSprite));
			
			//base.UpdateBehaviour();
		}
	}
}