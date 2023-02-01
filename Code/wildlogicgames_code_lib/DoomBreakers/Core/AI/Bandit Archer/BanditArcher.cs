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

    public class BanditArcher : ArcherStateMachine
    {
        [Header("Bandit Archer ID")]
        [Tooltip("ID ranges from 0 to ?")]          //Max ? enemies.
        public int _enemyID;                 //Set in editor per enemy?

        [Header("Arrow Aim Transform")]
        public Transform _aimTransform;

        [Header("Health Meter")]
        [Tooltip("The transforms used representing enemy health")]
        public Transform[] _healthTransform;
        private BanditStats _banditStats;


        private Transform _transform;
        private CharacterController2D _controller2D;
        private Animator _animator;

        private ArcherCollision _banditCollider;
        private BanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;
        private float _playerAttackedButtonTime;
        private ITimer _healthDisplayTimer, _bleedingTimer;

        private Action[] _actionListener = new Action[2];

        public void InitializeBanditArcher()
        {
            _transform = this.transform;
            _controller2D = this.GetComponent<CharacterController2D>();
            _animator = this.GetComponent<Animator>();

            _banditCollider = new ArcherCollision(this.GetComponent<Collider2D>(), ref _transform, ref _banditStats, _enemyID);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>(), "EnemyAnimControllers", "HumanoidBandit", "Bandit_with_bow&arrows_controller");
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _enemyID);

            _banditStats = new BanditStats(ref _healthTransform, ref _healthTransform, ref _healthTransform, 100.0, 100.0, 0.0);
            _healthDisplayTimer = this.gameObject.AddComponent<Timer>();
            _bleedingTimer = this.gameObject.AddComponent<Timer>();
            _banditStats.DisplayBleedFillBar(false);

            _actionListener[0] = new Action(AttackedByPlayer);//AttackedByPlayer()
            _actionListener[1] = new Action(DetectedAnPlayer);//DetectedAnPlayer()

            //OnEnable()
            //this.gameObject.SetActive(true);
            BattleColliderManager.Subscribe("ReportCollisionWithBanditArcher" + _enemyID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBanditArcher" + _enemyID.ToString(), _actionListener[1]);
        }
        private void OnEnable()
        {
			//Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 
			//BattleColliderManager.Subscribe("ReportCollisionWithBanditArcher" + _enemyID.ToString(), _actionListener[0]);
			//AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBanditArcher" + _enemyID.ToString(), _actionListener[1]);
		}
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithBanditArcher" + _enemyID.ToString(), _actionListener[0]);
            AITargetTrackingManager.Unsubscribe("ReportDetectionWithPlayerForBanditArcher" + _enemyID.ToString(), _actionListener[1]);
        }
        //private void Awake() => InitializeBanditArcher();

        void Start()
        {
            _banditAnimator.SetAnimatorController(BanditAnimatorController.Bandit_with_bow_and_arrows_controller);

            SetState(new BanditArcherIdle(this, _velocity, _enemyID));
        }
        void Update()
        {
            UpdateCollisions();
            UpdateStateBehaviours();
            UpdateStats();
        }
        public void UpdateStateBehaviours()
        {
            _state.IsIdleBowman(ref _animator, ref _banditCollider);
            _state.IsAiming(ref _animator, ref _banditCollider, ref _banditSprite, ref _transform);
            _state.IsShootTarget(ref _animator, ref _banditSprite, ref _banditCollider, ref _aimTransform);
            _state.IsArcherHit(ref _animator, ref _banditSprite);
            _state.IsDying(ref _animator, ref _banditSprite);
            _state.IsDead(ref _animator, ref _banditSprite);

            _state.UpdateBehaviour(ref _controller2D, ref _animator, ref _transform, ref _banditStats);
        }
        public void UpdateCollisions() //=> _banditCollider.UpdateCollision(ref _state, _banditSprite);
		{
            if (IsDying()) return;
            _banditCollider.UpdateCollision(ref _state, _banditSprite, ref _banditStats);
        }
        private void UpdateStats()
        {
            if (!_banditStats.Process()) return;

            if (_healthDisplayTimer.HasTimerFinished())
                _banditStats.DisplayHealthFillBar(false);

            if (_banditStats.Health <= 0f)
            {
                if (SafeToSetDying())
                {
                    UIPlayerManager.TriggerEvent("ReportUIPlayerKillScoreEvent");
                    SetState(new BanditDying(this, _velocity, _enemyID));
                    _banditStats.DisplayHealthFillBar(false);
                    _banditStats.DisplayBleedFillBar(false);
                    _banditStats.Disable();
                }
                return;
            }

        }
        private void AttackedByPlayer()
        {
            if (IsDying()) return;

            int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
            int playerFaceDir = BattleColliderManager.GetAssignedPlayerFaceDir(playerId);
            BaseState attackingPlayerState = BattleColliderManager.GetAssignedPlayerState(playerId);

            double playerAttackDamage = 0.0025;

            ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
            if (itemBase == null) return;

            if (itemBase.GetType() == typeof(Sword))
            {
                //ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
                Sword weaponDervived = itemBase as Sword;

                playerAttackDamage = weaponDervived.Damage();
            }
            if (itemBase.GetType() == typeof(Mace))
            {
                //ItemBase itemBase = BattleColliderManager.GetAssignedPlayerWeapon(playerId);
                Mace weaponDervived = itemBase as Mace;

                playerAttackDamage = weaponDervived.Damage();

            }

            AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX); 
            ObjectPooler._instance.InstantiateForEnemy(PrefabID.Prefab_BloodHitFX, _transform, _enemyID, _banditSprite.GetSpriteDirection());
            SetState(new BanditArcherHit(this, _velocity, _enemyID));
            _banditStats.Health -= playerAttackDamage;
            _healthDisplayTimer.StartTimer(1.0f);
        }

        //AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBanditArcher")
        private void DetectedAnPlayer() => SetState(new BanditArcherShoot(this, _velocity, ref _transform, ref _aimTransform, AITargetTrackingManager.GetAssignedTargetTransform(_enemyID, EnemyAI.BanditArcher), _enemyID));

        private void OnDrawGizmosSelected()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 12.0f);

        }
    }
}


