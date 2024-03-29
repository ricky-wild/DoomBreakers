﻿
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerHitByQuickAttack : BaseState//, IPlayerJump
	{
		public PlayerHitByQuickAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			AudioEventManager.StopPlayerSFX(PlayerSFXID.PlayerChargeAttackSFX);
			//print("\nHitByQuickAttack State.");
		}
		public override void IsHitByQuickAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input)
		{
			animator.Play("Jabbed");//, -1, 0.0f);


			playerSprite.SetBehaviourTextureFlash(0.5f, Color.red);

			_behaviourTimer.StartTimer(_quickAtkWaitTime*2);
			if (_behaviourTimer.HasTimerFinished())
			{
				
				//AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX);
				playerSprite.ResetTexture2DColor();
				_targetVelocityX = 0f;
				_velocity.x = 0f;
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}


			//base.UpdateBehaviour();
		}
	}
}