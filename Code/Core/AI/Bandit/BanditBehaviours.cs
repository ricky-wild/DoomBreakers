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
        private float _targetVelocityX, _moveSpeed, _sprintSpeed, _gravity,
			_maxJumpVelocity, _targetVelocityY, _maxPowerStruckVelocityY, _maxPowerStruckVelocityX;

        private int _quickAttackIncrement; //2+ variations of this animation.
		private int _attackCooldownCounter;
		private int _attackCooldownLimit;
		private float _textureFlashTime, _cooldownWaitTime, _idleWaitTime, _quickAtkWaitTime;

		private ICollisionData _collidedData;

        private ITimer _behaviourTimer, _cooldownTimer, _spriteColourSwapTimer;


		public void Setup(Transform t, Controller2D controller2D)
        {
			_controller2D = controller2D;
			_transform = t;
			_velocity = new Vector3();
			_moveSpeed = SetVariedMoveSpeed();
			_sprintSpeed = 1.0f;
			_targetVelocityX = 1.0f;
			_targetVelocityY = 0f;
			_maxJumpVelocity = 14.0f;//13.25f;
			_maxPowerStruckVelocityY = 10.0f; //10.0f for lowest impact. 14.0f for average. 16.0f for maximum impact.
			_maxPowerStruckVelocityX = 0.5f;
			_gravity = -(2 * 0.8f) / Mathf.Pow(0.25f, 2); //_gravity = -(3 * 0.8f) / Mathf.Pow(0.9f, 2);//this will create a moon like gravity effect
			_quickAttackIncrement = 0;
			_attackCooldownCounter = 0;
			_attackCooldownLimit = SetVariedAttackCooldownLimit();
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
		private float SetVariedMoveSpeed()
		{
			//3.5f is standard move speed.
			
			//Move speed must be applied at random within set range. This offers variation in between bandit movements in groups.
			int rand = wildlogicgames.Utilities.GetRandomNumberInt(0, 6);

			if (rand == 0)
				return 3.0f;
			if (rand == 1)
				return 3.15f;
			if (rand == 2)
				return 3.25f;
			if (rand == 3)
				return 3.35f;
			if (rand == 4)
				return 3.45f;
			if (rand == 5)
				return 3.55f;
			if (rand == 6)
				return 3.65f;

			return 3.5f;
		}
		private int SetVariedAttackCooldownLimit()
		{
			//15 is the set standard for furious quick attack jabs at player.

			int rand = wildlogicgames.Utilities.GetRandomNumberInt(1, 15);
			return rand;
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
		public void FallProcess(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite, int banditId)
		{
			_behaviourTimer.StartTimer(_quickAtkWaitTime);
			if (_behaviourTimer.HasTimerFinished()) //So we only activate once.
				SetBehaviourTextureFlash(_textureFlashTime, banditSprite, Color.white);

			if(_collidedData != null)//if(_targetVelocityX == (_maxPowerStruckVelocityX * 1.2f))//float multiplier = 1.2f; from -> HitByPowerAttackProcess()
			{
				//Then we know the FallProcess() has begun after being power attack hit from player.
				//We want to slow down the x velocity upon peak height. (as enemy is struck into air)

				int banditFaceDir = _collidedData.GetCachedBanditSprite(banditId).GetSpriteDirection();
				int playerFaceDir = _collidedData.GetCachedPlayerSprite(0).GetSpriteDirection();

				float subtraction = _targetVelocityX / 2;

				if (banditFaceDir == 1 && playerFaceDir == -1) //Enemy facing right & player facing left, knock enemy to the left.
					_velocity.x -= subtraction;
				if (banditFaceDir == -1 && playerFaceDir == 1) //Enemy facing left & player facing right, knock enemy to the right.
					_velocity.x += subtraction;
				if (banditFaceDir == 1 && playerFaceDir == 1) //Enemy facing right & player facing right behind enemy, knock enemy to the right.
					_velocity.x += subtraction;
				if (banditFaceDir == -1 && playerFaceDir == -1) //Enemy facing left & player facing left behind enemy, knock enemy to the left.
					_velocity.x -= subtraction;
			}

			if (_controller2D.collisions.below) //Means we're finished jumping/falling.
			{
				_collidedData = null;
				_targetVelocityX = 0f;
				_targetVelocityY = 0f;
				_velocity.x = 0f;
				_velocity.y = 0f;
				enemyStateMachine.SetEnemyState(state.IsIdle);
			}
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
		public void HitByPowerAttackProcess(ICollisionData collisionData, int banditId)//IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
		{
			if (collisionData.SomeDataIsNull())// == null)
				return;


			SetBehaviourTextureFlash(_textureFlashTime, collisionData.GetCachedBanditSprite(banditId), Color.red);

			int playerId = collisionData.GetLastCollidedPlayerID();
			int banditFaceDir = collisionData.GetCachedBanditSprite(banditId).GetSpriteDirection();
			int playerFaceDir = collisionData.GetCachedPlayerSprite(playerId).GetSpriteDirection();
			WeaponChargeHold weaponChargeHoldFlag = collisionData.GetCachedPlayerSprite(playerId).GetWeaponTexChargeFlag();
			float multiplier = 1.2f;// 1.66f;
			//print("\nweaponChargeHoldFlag =" + weaponChargeHoldFlag);
			switch(weaponChargeHoldFlag)
			{
				case WeaponChargeHold.None:
					multiplier = 0.75f;
					break;
				case WeaponChargeHold.Minimal:
					multiplier = 1.2f;
					break;
				case WeaponChargeHold.Moderate:
					multiplier = 1.5f;
					break;
				case WeaponChargeHold.Maximal:
					multiplier = 1.9f;
					break;
			}

			if (_velocity.y >= (_maxPowerStruckVelocityY + (multiplier/4)))//_maxPowerStruckVelocityY) //Near peak of jump velocity, set falling state.
			{
				if (_collidedData != collisionData)//== null)
					_collidedData = collisionData;
				_velocity.x = 0f;
				collisionData.GetCachedEnemyState(banditId).SetEnemyState(state.IsFalling);
				return;
			}
			else
			{
				_velocity.y += _maxPowerStruckVelocityY / 6;// (6-multiplier);// = 0f;
				_targetVelocityX = _maxPowerStruckVelocityX * multiplier; //Carries on over to FallProcess() where appropriate.

				if (banditFaceDir == 1 && playerFaceDir == -1) //Enemy facing right & player facing left, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
				if (banditFaceDir == -1 && playerFaceDir == 1) //Enemy facing left & player facing right, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (banditFaceDir == 1 && playerFaceDir == 1) //Enemy facing right & player facing right behind enemy, knock enemy to the right.
				{
					_velocity.x += _targetVelocityX;
					return;
				}
				if (banditFaceDir == -1 && playerFaceDir == -1) //Enemy facing left & player facing left behind enemy, knock enemy to the left.
				{
					_velocity.x -= _targetVelocityX;
					return;
				}
			}
		}
		void Update()
        {
			//print("\n_velocity.x=" + _velocity.x);
			//print("\n_targetVelocityX=" + _targetVelocityX);
		}

        public void UpdateMovement(IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite, IBanditCollision banditCollider)
		{
			//print("\nPlayerBehaviour.cs UpdateMovement() playerStateMachine=" + playerStateMachine.GetPlayerState());
			UpdateGravity(enemyStateMachine);

			if (enemyStateMachine.IsSprinting())
				_sprintSpeed = 1.75f;
			if (!enemyStateMachine.IsSprinting())
				_sprintSpeed = 1.0f;




			if(!enemyStateMachine.IsPowerAttackHit())// || !enemyStateMachine.IsFalling())
			{
				if(!enemyStateMachine.IsFalling())
					_velocity.x = _targetVelocityX;
			}


			DetectFaceDirection(banditSprite, banditCollider);

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
				_targetVelocityY = _gravity * Time.deltaTime;
				_velocity.y += _targetVelocityY;// _gravity * Time.deltaTime;

				if (!enemyStateMachine.IsQuickAttack())
					return;
				if (!enemyStateMachine.IsJumping())
					return;
				if (enemyStateMachine.IsPowerAttackHit())
					return;

				enemyStateMachine.SetEnemyState(state.IsFalling);
			}
			if (enemyStateMachine.IsJumping())
				return;
			if (enemyStateMachine.IsFalling())
				return;
			if (enemyStateMachine.IsPowerAttackHit())
				return;
			if (_controller2D.collisions.below)
				_velocity.y = 0f;
		}

		private void DetectFaceDirection(IBanditSprite banditSprite, IBanditCollision banditCollider)
		{
			if (_velocity.x < 0f)
			{
				if (banditSprite.GetSpriteDirection() == 1)//Guard clause,only flip once.
				{
					banditSprite.FlipSprite();
					banditCollider.FlipAttackPoints(-1);
				}
				return;
			}
			if (_velocity.x > 0f)
			{
				if (banditSprite.GetSpriteDirection() == -1)
				{
					banditSprite.FlipSprite();
					banditCollider.FlipAttackPoints(1);
				}
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

			if (enemyStateMachine.IsQuickAttackHit())
				return false;
			if (enemyStateMachine.IsQuickAttack())
				return false;
			if (enemyStateMachine.IsPowerAttackHit())
				return false;
			if (enemyStateMachine.IsQuickHitWhenDefending())
				return false;
			if (enemyStateMachine.IsPowerHitWhenDefending())
				return false;
			if (enemyStateMachine.IsImpactHit())
				return false;

			if (enemyStateMachine.IsFalling())
				return false;

			return true;
		}
	}

}

