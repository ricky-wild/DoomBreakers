
using UnityEngine;

namespace DoomBreakers
{
	public class PlayerDefend : BaseState, IPlayerDefend
	{

		public PlayerDefend(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			//print("\nDefence State.");
		}

		public override void IsDefending(ref Animator animator, ref Vector2 input)
		{
			//playerSprite.SetWeaponChargeTextureFXFlag(true);
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			if (_velocity.x != 0.0f)
			{
				animator.Play("RunDefend");
			}
			else
			{
				animator.Play("Defend");//, -1, 0.0f);
			}
			//base.UpdateBehaviour();
		}
	}
}

