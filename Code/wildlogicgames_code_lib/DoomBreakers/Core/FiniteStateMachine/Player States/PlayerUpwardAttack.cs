using UnityEngine;

namespace DoomBreakers
{
	public class PlayerUpwardAttack : BaseState, IPlayerUpwardAttack
	{
		private Transform _transform;
		public PlayerUpwardAttack(StateMachine s, Vector3 v, Transform t) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_transform = t;
			_behaviourTimer = new Timer();
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, -1);
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, -1);
			ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_RunningDustFX, _transform, 0, 1);
			//print("\nUpward Attack State.");
		}


		public override void IsUpwardAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("SmallAttackUpward");
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			playerSprite.SetBehaviourTextureFlash(0.1f, Color.white);
			_behaviourTimer.StartTimer(0.66f);//anim length
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();
				AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerQuickAttackSFX);
				ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_JumpingDustFX, _transform, 0, -playerSprite.GetSpriteDirection());
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}
			//base.UpdateBehaviour();
		}
	}
}