
using UnityEngine;

namespace DoomBreakers
{
	public class BanditQuickAttack : BasicEnemyBaseState
	{

		public BanditQuickAttack(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_quickAtkWaitTime = 0.2f;//0.133f;
			_behaviourTimer = new Timer();
			_cooldownTimer = new Timer();
			//print("\nQuickAttack State.");
		}

		public override void IsQuickAttack(ref Animator animator, ref IBanditCollision banditCollider, ref IBanditSprite banditSprite, ref int quickAttackIncrement)
		{

			_cooldownTimer.StartTimer(_quickAtkWaitTime/2);
			if (_cooldownTimer.HasTimerFinished())
				banditCollider.EnableTargetCollisionDetection(); //Must be set before changing state here.

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
			banditSprite.SetBehaviourTextureFlash(0.25f, Color.white);
			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();

				if (quickAttackIncrement >= 0 && quickAttackIncrement < 2)
					quickAttackIncrement++;
				else
					quickAttackIncrement = 0;

				AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyQuickAttackSFX);
				_stateMachine.SetState(new BanditIdle(_stateMachine, _velocity, _enemyID));
			}

			//base.UpdateBehaviour();
		}
	}
}

