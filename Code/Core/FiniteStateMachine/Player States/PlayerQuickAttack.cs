using UnityEngine;

namespace DoomBreakers
{
	public class PlayerQuickAttack : BaseState, IPlayerQuickAttack
	{
		public PlayerQuickAttack(StateMachine s, Vector3 v) : base(velocity: v)//=> _stateMachine = s; 
		{
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			//_quickAttackIncrement = quickAttackIncrement;
			_behaviourTimer = new Timer();
			print("\nQuick Attack State.");
		}


		public override void IsQuickAttack(ref Animator animator, ref IPlayerSprite playerSprite, ref Vector2 input, ref int quickAttackIncrement) 
		{

			switch (quickAttackIncrement)
			{
				case 0:
					animator.Play("SmallAttack");//, -1, 0.0f); //SmallAttack2 - SmallAttack5
					break;
				case 1:
					animator.Play("SmallAttack2");//, -1, 0.0f);
					break;
				case 2:
					animator.Play("SmallAttack3");
					break;
				case 3:
					animator.Play("SmallAttack4");
					break;
			}
			_velocity.x = (input.x * (_moveSpeed * _sprintSpeed));
			playerSprite.SetBehaviourTextureFlash(0.25f, Color.white);
			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				playerSprite.ResetTexture2DColor();

				if (quickAttackIncrement >= 0 && quickAttackIncrement < 4)
					quickAttackIncrement++;
				else
					quickAttackIncrement = 0;
				_stateMachine.SetState(new PlayerIdle(_stateMachine, _velocity));
			}
			//base.UpdateBehaviour();
		}
	}
}