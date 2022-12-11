using System;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class Bandit : BanditStateMachine, IEnemy
    {
        [Header("Bandit ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? enemies.
        public int _banditID;               //Set in editor per enemy?

        [Header("Health Meter")]
        [Tooltip("The transforms used representing enemy health")]
        public Transform[] _healthTransform;
        private BanditStats _banditStats;

        [Header("Enemy Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK
        private Transform _transform;
        private CharacterController2D _controller2D;
        private Animator _animator;

        private IBanditCollision _banditCollider;
        private BanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;
        private float _playerAttackedButtonTime;
        private ITimer _healthDisplayTimer;

        private Action[] _actionListener = new Action[2];

        private void InitializeBandit()
        {
            _transform = this.transform;
            _controller2D = this.GetComponent<CharacterController2D>();
            _animator = this.GetComponent<Animator>();

            _banditCollider = new BanditCollision(this.GetComponent<Collider2D>(), ref _attackPoints, _banditID);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>());
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _banditID);
            _banditStats = new BanditStats(ref _healthTransform, 100.0, 100.0, 0.0);
            _healthDisplayTimer = this.gameObject.AddComponent<Timer>();
            //_healthDisplayTimer.StartTimer(0.05f);


            _actionListener[0] = new Action(AttackedByPlayer);//AttackedByPlayer()
            _actionListener[1] = new Action(DetectedAnPlayer);//DetectedAnPlayer()

        }
        private void OnEnable()
        {
            //Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 
            BattleColliderManager.Subscribe("ReportCollisionWithBandit" + _banditID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBandit" + _banditID.ToString(), _actionListener[1]);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithBandit" + _banditID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Unsubscribe("ReportDetectionWithPlayerForBandit" + _banditID.ToString(), _actionListener[1]);
        }
        private void Awake()
        {
            InitializeBandit();
        }
        void Start()
        {
            _banditAnimator.SetAnimatorController(BanditAnimatorController.Bandit_with_broadsword_controller);
            
            SetState(new BanditIdle(this, Vector3.zero, _banditID));
        }
        void Update() 
        {
            UpdateStateBehaviours();
            UpdateCollisions();
            UpdateStats();
        }
        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator, ref _banditCollider);
            _state.IsJumping(ref _animator, ref _banditSprite);
            _state.IsWaiting(ref _animator);
            _state.IsFalling(ref _animator, ref _controller2D, ref _banditSprite);
            _state.IsPersueTarget(ref _animator, ref _banditSprite, ref _banditCollider);
            _state.IsDefending(ref _animator, ref _controller2D, ref _banditSprite);
            
            _state.IsQuickAttack(ref _animator, ref _banditCollider, ref _banditSprite, ref _quickAttackIncrement);
            _state.IsHoldAttack(ref _animator, ref _banditSprite);
            _state.IsReleaseAttack(ref _animator, ref _banditCollider, ref _banditSprite);

            _state.IsHitByQuickAttack(ref _animator, ref _banditSprite);
            _state.IsHitByPowerAttack(ref _animator, ref _banditSprite, _playerAttackedButtonTime);
            _state.IsHitWhileDefending(ref _animator, ref _controller2D, ref _banditSprite);
            _state.IsHitByUpwardAttack(ref _animator, ref _banditSprite);
            _state.IsHitByKnockAttack(ref _animator, ref _banditSprite);

            _state.IsDying(ref _animator, ref _banditSprite);
            _state.IsDead(ref _animator, ref _banditSprite);

            _state.UpdateBehaviour(ref _controller2D, ref _animator, ref _transform);
        }

        public void UpdateCollisions()
		{
            _banditCollider.UpdateCollision(ref _state, _banditSprite);
        }
        private void UpdateStats()
		{
            if (!_banditStats.Process()) return;

            if(_healthDisplayTimer.HasTimerFinished())
                _banditStats.DisplayFillBar(false);

            if (_banditStats.Health <= 0f)
            {
                if(SafeToSetDying())
				{
                    UIPlayerManager.TriggerEvent("ReportUIPlayerKillScoreEvent");
                    SetState(new BanditDying(this, _velocity, _banditID));
                    _banditStats.DisplayFillBar(false);
                    _banditStats.Disable();
                }
            }
        }
        private void AttackedByPlayer()
		{
            //print("\nBandit.cs= AttackedByPlayer() called!");

            
            int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
            int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);
            BaseState attackingPlayerState = BattleColliderManager.GetAssignedPlayerState(playerId);

            double playerQuickAttackDamage = 0.005;
            double playerPowerAttackDamage = 0.025;

            if(BattleColliderManager.GetAssignedPlayerWeapon(playerId).GetType() == typeof(Sword))
			{
                ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
                Sword weaponDervived = itemBase as Sword;

                playerQuickAttackDamage = weaponDervived.Damage();
                playerPowerAttackDamage += weaponDervived.Damage();
            }


            if (ProcessQuickAttackFromPlayer(ref attackingPlayerState, playerId, playerFaceDir, _banditID, _banditSprite.GetSpriteDirection()))
			{
                _banditStats.Health -= playerQuickAttackDamage;
                AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX);
            }
            _playerAttackedButtonTime = ProcessPowerAttackFromPlayer(ref attackingPlayerState, _banditID);
            if(_playerAttackedButtonTime != 0f) _banditStats.Health -= playerPowerAttackDamage;
            if (ProcessUpwardAttackFromPlayer(ref attackingPlayerState, _banditID))
			{
                _banditStats.Health -= playerQuickAttackDamage;
            }
            if(ProcessKnockAttackFromPlayer(ref attackingPlayerState, playerId, playerFaceDir, _banditID, _banditSprite.GetSpriteDirection()))
			{
                _banditStats.Health -= playerPowerAttackDamage;
			}
            _healthDisplayTimer.StartTimer(1.0f);
        }
        private void DetectedAnPlayer()
		{
            //print("\nBandit.cs= DetectedAnPlayer() called!");

            if (!SafeToSetPersue())
                return;

            SetState(new BanditPersue(this, _velocity, _transform,_banditID));
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _attackPoints.Length; i++)
            {
                if (_attackPoints[i].position == null)
                    return;
            }


            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoints[0].position, 1.2f);
            Gizmos.DrawWireSphere(_attackPoints[1].position, 1.6f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_attackPoints[2].position, 1.3f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, 12.0f);
        }
    }
}


