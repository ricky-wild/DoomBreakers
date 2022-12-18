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
        public int _enemyID;               //Set in editor per enemy?

        [Header("Health Meter")]
        [Tooltip("The transforms used representing enemy health")]
        public Transform[] _healthTransform;
        private BanditStats _banditStats;

        [Header("Bleed Meter")]
        [Tooltip("The transforms used representing enemy bleeding")]
        public Transform[] _bleedTransform;

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
        private ITimer _healthDisplayTimer, _bleedingTimer;

        private Action[] _actionListener = new Action[2];

        private void InitializeBandit()
        {
            _transform = this.transform;
            _controller2D = this.GetComponent<CharacterController2D>();
            _animator = this.GetComponent<Animator>();

            _banditCollider = new BanditCollision(this.GetComponent<Collider2D>(), ref _attackPoints, ref _banditStats, _enemyID);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>(), "EnemyAnimControllers", "HumanoidBandit", "Bandit_with_nothing_controller");
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _enemyID);
            _banditStats = new BanditStats(ref _healthTransform, ref _bleedTransform, 100.0, 100.0, 0.0);
            _healthDisplayTimer = this.gameObject.AddComponent<Timer>();
            _bleedingTimer = this.gameObject.AddComponent<Timer>();


            _actionListener[0] = new Action(AttackedByPlayer);//AttackedByPlayer()
            _actionListener[1] = new Action(DetectedAnPlayer);//DetectedAnPlayer()

        }
        private void OnEnable()
        {
            //Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 
            BattleColliderManager.Subscribe("ReportCollisionWithBandit" + _enemyID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBandit" + _enemyID.ToString(), _actionListener[1]);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithBandit" + _enemyID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Unsubscribe("ReportDetectionWithPlayerForBandit" + _enemyID.ToString(), _actionListener[1]);
        }
        private void Awake() => InitializeBandit();
        
        void Start()
        {
            _banditAnimator.SetAnimatorController(BanditAnimatorController.Bandit_with_broadsword_controller);
            
            SetState(new BanditIdle(this, Vector3.zero, _enemyID));
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
                _banditStats.DisplayHealthFillBar(false);

            if (_banditStats.Health <= 0f)
            {
                if(SafeToSetDying())
				{
                    UIPlayerManager.TriggerEvent("ReportUIPlayerKillScoreEvent");
                    SetState(new BanditDying(this, _velocity, _enemyID));
                    _banditStats.DisplayHealthFillBar(false);
                    _banditStats.DisplayBleedFillBar(false);
                    _banditStats.Disable();
                }
                return;
            }
            if (_banditStats.IsBleeding()) 
			{
                _banditStats.UpdateBleedingDamage();
                if (_bleedingTimer.HasTimerFinished())
                {
                    if (_banditStats.Bleeding > 0.01f) _banditStats.Bleeding -= 0.01f;
                    if (_banditStats.Bleeding < 0.01f) _banditStats.IsBleeding(false);

                    _bleedingTimer.StartTimer(0.05f);
                }

			}
        }
        private void AttackedByPlayer()
		{
            //print("\nBandit.cs= AttackedByPlayer() called!");

            
            int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
            int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);
            BaseState attackingPlayerState = BattleColliderManager.GetAssignedPlayerState(playerId);

            double playerQuickAttackDamage = 0.0025;
            double playerPowerAttackDamage = 0.01;

            ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
            if (itemBase == null) return;

            if (itemBase.GetType() == typeof(Sword))
            {
                //ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
                Sword weaponDervived = itemBase as Sword;

                playerQuickAttackDamage = weaponDervived.Damage();
                playerPowerAttackDamage += weaponDervived.Damage();
            }
            else
                return;


            if (ProcessQuickAttackFromPlayer(ref attackingPlayerState, playerId, playerFaceDir, _enemyID, _banditSprite.GetSpriteDirection()))
			{
                AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX); //AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyHitSFX);
                ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_BloodHitFX, _transform, _enemyID, _banditSprite.GetSpriteDirection());
                _banditStats.Health -= playerQuickAttackDamage;
                AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX); //AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyHitSFX);
            }
            _playerAttackedButtonTime = ProcessPowerAttackFromPlayer(ref attackingPlayerState, _enemyID);
            if(_playerAttackedButtonTime != 0f) _banditStats.Health -= playerPowerAttackDamage;
            if (ProcessUpwardAttackFromPlayer(ref attackingPlayerState, _enemyID))
			{
                AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX);
                ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_BloodHitFX, _transform, _enemyID, _banditSprite.GetSpriteDirection());
                _banditStats.Health -= playerQuickAttackDamage;
                if (!_banditStats.IsBleeding()) _banditStats.IsBleeding(true);
            }
            if(ProcessKnockAttackFromPlayer(ref attackingPlayerState, playerId, playerFaceDir, _enemyID, _banditSprite.GetSpriteDirection()))
			{
                ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_BloodHitFX, _transform, _enemyID, _banditSprite.GetSpriteDirection());
                _banditStats.Health -= playerPowerAttackDamage;
                AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
                _controller2D.IgnoreEdgeDetection(true);

            }
            _healthDisplayTimer.StartTimer(1.0f);
        }
        private void DetectedAnPlayer()
		{
            //print("\nBandit.cs= DetectedAnPlayer() called!");

            if (!SafeToSetPersue())
                return;

            _controller2D.IgnoreEdgeDetection(false);
            SetState(new BanditPersue(this, _velocity, _transform,_enemyID));
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


