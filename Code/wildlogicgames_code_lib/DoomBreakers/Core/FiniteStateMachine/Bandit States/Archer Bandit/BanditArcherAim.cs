

using UnityEngine;

namespace DoomBreakers
{
	public class BanditArcherAim : BasicEnemyBaseState
	{

		public BanditArcherAim(BasicEnemyStateMachine s, Vector3 v, int id) : base(velocity: v, enemyId: id)//=> _stateMachine = s; 
		{
			_enemyID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_idleWaitTime = 0.25f;
			_behaviourTimer = new Timer();


		}

		public override void IsAiming(ref Animator animator, ref ArcherCollision banditCollider, ref IBanditSprite banditSprite, ref Transform transform)
		{
			animator.Play("Aim");//, 0, 0.0f);
			_velocity.x = 0f;




			//_behaviourTimer.StartTimer(_idleWaitTime);
			//if (_behaviourTimer.HasTimerFinished())
			banditCollider.EnableTargetCollisionDetection(); //BanditCollision.cs->Finds Player->BanditArcher.cs->DetectedPlayer->BanditArcherShoot.cs

			if (AITargetTrackingManager._instance != null)
			{
				Transform targetTransform = AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.BanditArcher);//.position;

				if (targetTransform != null)
					_stateMachine.SetState(new BanditArcherShoot(_stateMachine, _velocity, ref transform, ref targetTransform, targetTransform, _enemyID));

				if (!banditCollider.IsDisableShootFlag())
				{

				}
				//if (!banditCollider.IsDisableShootFlag())
				//{
				//	_stateMachine.SetState(new BanditArcherIdle(_stateMachine, _velocity, _enemyID));
				//}

			}
		}

	}
}

