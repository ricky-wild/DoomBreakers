
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerKnockAttack : BaseState
	{
		private Transform _transform;
		public PlayerKnockAttack(StateMachine s, Vector3 v, Transform t) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_behaviourTimer = new Timer();
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, 1);
			//print("\nKnock Attack State.");
		}

		public override void IsKnockAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerKnockAttackSFX);
			animator.Play("Knock Attack");//, 0, 0.0f);
									//_velocity.x = 0f;
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed)) / 2;
			playerSprite.SetBehaviourTextureFlash(0.1f, Color.white);


			_behaviourTimer.StartTimer(0.355f);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, -playerSprite.GetSpriteDirection());
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}

			if (Mathf.Abs(_velocity.y) >= 3.0f)
				_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity, null, ref playerSprite));
			//base.UpdateBehaviour();
		}
	}
}

