
using UnityEngine;

namespace DoomBreakers
{
	public class BanditPersue : BanditBaseState
	{

		public BanditPersue(EnemyStateMachine s, Vector3 v, Transform transform, int id) : base(velocity: v, banditId: id)//=> _stateMachine = s; 
		{
			_banditID = id;
			_stateMachine = s;
			_velocity = v; //We want to carry this on between states.
			_behaviourTimer = new Timer();
			print("\nPersue State.");
		}

		public override void IsPersueTarget(ref Animator animator)
		{
			animator.Play("Run");//, 0, 0.0f);

			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	_stateMachine.SetState(new PlayerFall(_stateMachine, _velocity));


			int trackingDir = 0; //Face direction either -1 left, or 1 right.

			if (AITargetTrackingManager.GetAssignedTargetTransform(_banditID, EnemyAI.Bandit).position.x > _transform.position.x)
				trackingDir = 1;
			if (AITargetTrackingManager.GetAssignedTargetTransform(_banditID, EnemyAI.Bandit).position.x < _transform.position.x)
				trackingDir = -1;


			//if(trackingDir == -1)
			//{
			//	if (_transform.position.x > collisionData.GetCachedTargetTransform().position.x + 1.0f)
			//		_targetVelocityX = -(0.5f * (_moveSpeed * _sprintSpeed));
			//	else
			//		PersueTargetReached(enemyStateMachine);
			//	return;
			//}
			////if (banditSprite.GetSpriteDirection() == 1)
			//if (trackingDir == 1)
			//{
			//	if (_transform.position.x < collisionData.GetCachedTargetTransform().position.x - 1.0f)
			//		_targetVelocityX = 0.5f * (_moveSpeed * _sprintSpeed);
			//	else
			//		PersueTargetReached(enemyStateMachine);
			//	return;
			//}

			//base.UpdateBehaviour();
		}
	}
}

