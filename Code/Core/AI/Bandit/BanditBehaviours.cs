using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class BanditBehaviours : MonoBehaviour, IBanditBehaviours
    {

        private Controller2D _controller2D;
        private Vector3 _velocity;
        private Transform _transform;
        private float _targetVelocityX, _maxJumpVelocity, _moveSpeed, _sprintSpeed, _gravity;

        private int _quickAttackIncrement; //2+ variations of this animation.
		private int _attackCooldownCounter;
		private int _attackCooldownLimit;
		private float _textureFlashTime, _cooldownWaitTime, _idleWaitTime, _quickAtkWaitTime;

		private IPlayerStateMachine _playerStateMachineRef;

        private ITimer _behaviourTimer, _cooldownTimer, _spriteColourSwapTimer;

        public void Setup(Transform t, Controller2D controller2D)
        {
			_controller2D = controller2D;
			_transform = t;
			_velocity = new Vector3();
			_moveSpeed = 3.5f;//3.75f;
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_maxJumpVelocity = 14.0f;//13.25f;
			_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
			_quickAttackIncrement = 0;
			_attackCooldownCounter = 0;
			_attackCooldownLimit = 15;
			_textureFlashTime = 0.1f;
			_cooldownWaitTime = 3.0f;
			_idleWaitTime = 1.5f;
			_quickAtkWaitTime = 0.133f;
			//_jumpedFlag = false;

			//_behaviourTimer = new Timer();
			_behaviourTimer = this.gameObject.AddComponent<Timer>();
			_behaviourTimer.Setup("_behaviourTimer");
			_cooldownTimer = this.gameObject.AddComponent<Timer>();
			_cooldownTimer.Setup("_cooldownTimer");
			_spriteColourSwapTimer = this.gameObject.AddComponent<Timer>();
			_spriteColourSwapTimer.Setup("_spriteColourSwapTimer");
		}
        public int GetQuickAttackIndex()
		{
			return _quickAttackIncrement;
		}
		private void Awake()
		{
		}
		void Start()
		{
		}
		public void IdleProcess(IEnemyStateMachine enemyStateMachine, IBanditCollision banditCollider)
		{
			_velocity.x = 0f;
			_targetVelocityX = 0f;
			//_velocity.y = 0f;
			//print("\n_velocity.y=" + Mathf.Abs(_velocity.y));

			_behaviourTimer.StartTimer(_idleWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				if (_cooldownTimer.HasTimerFinished())
					_attackCooldownCounter = 0;
				banditCollider.EnableTargetCollisionDetection(); //Begin player detection & trigger PersueTarget() Method here.
			}


			//if (Mathf.Abs(_velocity.y) >= 3.0f)
			//	enemyStateMachine.SetEnemyState(state.IsFalling);
		}
		public void WaitingProcess(IEnemyStateMachine enemyStateMachine)
		{
			_velocity.x = 0f;
			_targetVelocityX = 0f;

			_cooldownTimer.StartTimer(_cooldownWaitTime);
			if (_cooldownTimer.HasTimerFinished())
			{
				_attackCooldownCounter = 0;
				enemyStateMachine.SetEnemyState(state.IsIdle);
				return;
			}
		}
		public bool JumpProcess(IEnemyStateMachine enemyStateMachine)
		{
			return true;
		}
		public void FallProcess(IEnemyStateMachine enemyStateMachine)
		{

		}
		public void PersueTarget(IEnemyStateMachine enemyStateMachine, Transform targetTransform, IBanditSprite banditSprite)
		{
			if (!SafeToPersueTarget(enemyStateMachine, targetTransform)) //Guard Clause
				return;

			int trackingDir = 0; //Face direction either -1 left, or 1 right.

			if (targetTransform.position.x > _transform.position.x)
				trackingDir = 1;
			if (targetTransform.position.x < _transform.position.x)
				trackingDir = -1;

			//if (banditSprite.GetSpriteDirection() == -1)
			if(trackingDir == -1)
			{
				if (_transform.position.x > targetTransform.position.x + 1.0f)
					_targetVelocityX = -(0.5f * (_moveSpeed * _sprintSpeed));
				else
					PersueTargetReached(enemyStateMachine);
				return;
			}
			//if (banditSprite.GetSpriteDirection() == 1)
			if (trackingDir == 1)
			{
				if (_transform.position.x < targetTransform.position.x - 1.0f)
					_targetVelocityX = 0.5f * (_moveSpeed * _sprintSpeed);
				else
					PersueTargetReached(enemyStateMachine);
				return;
			}
		}
		private void PersueTargetReached(IEnemyStateMachine enemyStateMachine)
		{
			_targetVelocityX = 0f;
			enemyStateMachine.SetEnemyState(state.IsQuickAttack);
		}
		public void QuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{

			SetBehaviourTextureFlash(_textureFlashTime, banditSprite, Color.white);
			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				if (_attackCooldownCounter < _attackCooldownLimit)
					_attackCooldownCounter++;
				else
				{
					enemyStateMachine.SetEnemyState(state.IsWaiting); //_cooldownTimer.StartTimer(2.0f);
					return;
				}

				banditSprite.ResetTexture2DColor();
				if (_quickAttackIncrement >= 0 && _quickAttackIncrement < 2)
					_quickAttackIncrement++;
				else
					_quickAttackIncrement = 0;
				enemyStateMachine.SetEnemyState(state.IsIdle);
			}

		}
		public void HitByQuickAttackProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{

			SetBehaviourTextureFlash(_textureFlashTime, banditSprite, Color.red);
			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished())
			{
				banditSprite.ResetTexture2DColor();
				_targetVelocityX = 0f;
				enemyStateMachine.SetEnemyState(state.IsIdle);
			}
			

		}
		void Update()
        {

        }

        public void UpdateMovement(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{
			//print("\nPlayerBehaviour.cs UpdateMovement() playerStateMachine=" + playerStateMachine.GetPlayerState());
			UpdateGravity(enemyStateMachine);

			//if (!SafeToMove(playerStateMachine))//Guard Clause
			//{
			//	//print("\nPlayerBehaviour.cs UpdateMovement()= NOT SafeToMove()");
			//	input = Vector2.zero;
			//	UpdateTransform(input);
			//	return;
			//}

			if (enemyStateMachine.GetEnemyState() == state.IsSprinting)
				_sprintSpeed = 1.75f;
			if (enemyStateMachine.GetEnemyState() != state.IsSprinting)
				_sprintSpeed = 1.0f;
			//_targetVelocityX = (input.x * (_moveSpeed * _sprintSpeed));
			_velocity.x = _targetVelocityX;

			//print("\nx=" + _velocity.x);

			//DetectMovement(enemyStateMachine);
			DetectFaceDirection(banditSprite);

			UpdateTransform();
		}
        public void UpdateTransform()
		{
			_controller2D.Move(_velocity * Time.deltaTime, _velocity);
		}
        public void UpdateGravity(IEnemyStateMachine enemyStateMachine)
		{
			//print("\n_velocity.y=" + _velocity.y);
			if (!_controller2D.collisions.below)
			{
				_velocity.y += _gravity * Time.deltaTime;

				if (enemyStateMachine.GetEnemyState() != state.IsQuickAttack)
					return;
				if (enemyStateMachine.GetEnemyState() != state.IsJumping)
					return;

				enemyStateMachine.SetEnemyState(state.IsFalling);
			}
			if (enemyStateMachine.GetEnemyState() == state.IsJumping)
				return;
			if (enemyStateMachine.GetEnemyState() == state.IsFalling)
			{
				//_velocity.y += _gravity * Time.deltaTime;
				return;
			}
			if (_controller2D.collisions.below)
				_velocity.y = 0f;
		}

		private void DetectFaceDirection(IBanditSprite banditSprite)
		{
			if (_velocity.x < 0f)
			{
				if (banditSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
					banditSprite.FlipSprite();
				return;
			}
			if (_velocity.x > 0f)
			{
				if (banditSprite.GetSpriteDirection() == -1)
					banditSprite.FlipSprite();
				return;
			}
		}

		private void SetBehaviourTextureFlash(float time, IBanditSprite banditSprite, Color colour)
		{
			_spriteColourSwapTimer.StartTimer(time);//flash sprite colour timer.
			if (_spriteColourSwapTimer.HasTimerFinished())
				banditSprite.SetTexture2DColor(colour);
		}

		public bool SafeToPersueTarget(IEnemyStateMachine enemyStateMachine, Transform targetTransform)
		{
			if (targetTransform == null) //Guard Clause
				return false;

			if (enemyStateMachine.GetEnemyState() == state.IsHitByQuickAttack)
				return false;
			if (enemyStateMachine.GetEnemyState() == state.IsQuickAttack)
				return false;

			return true;
		}
	}

}

