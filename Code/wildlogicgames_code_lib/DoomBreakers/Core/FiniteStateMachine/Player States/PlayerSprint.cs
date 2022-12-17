using UnityEngine;

namespace DoomBreakers
{
	public class PlayerSprint : BaseState, IPlayerMove
	{
		private Transform _transform;
		public PlayerSprint(StateMachine s, Vector3 v, Transform t,ref IPlayerSprite playerSprite) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_sprintSpeed = 1.75f;
			_behaviourTimer = new Timer();
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, -playerSprite.GetSpriteDirection());
		}

		public override void IsSprinting(ref Animator animator, ref Vector2 input, ref IPlayerSprite playerSprite, ref PlayerCollision playerCollider)
		{
			animator.Play("Sprint");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			DetectFaceDirection(ref playerSprite, ref playerCollider);

			

			_behaviourTimer.StartTimer(0.65f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.SetBehaviourTextureFlash(0.2f, Color.white);
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, playerSprite.GetSpriteDirection());
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, _transform, ref playerSprite));
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