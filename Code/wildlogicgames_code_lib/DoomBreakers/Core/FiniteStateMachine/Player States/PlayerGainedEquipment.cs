
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerGainedEquipment : BaseState
	{

		public PlayerGainedEquipment(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			//print("\nGained Equipment State.");
		}

		public override void IsGainedEquipment(ref Animator animator, ref IPlayerSprite playerSprite, ref IPlayerEquipment playerEquipment)
		{
			animator.Play("EquipAquired");//, 0, 0.0f);
			_velocity.x = 0f;

			//playerSprite.SetBehaviourTextureFlash(0.025f, Color.white);
			_behaviourTimer.StartTimer(_gainedEquipWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				//AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerEquippedSFX);
				playerSprite.SetNewEquipmemtTextureColorFlag(true, playerEquipment);
				if (Mathf.Abs(_velocity.y) >= 3.0f)
					_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));
				else
					_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}
			//base.UpdateBehaviour();
		}
	}
}

