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
        public int _banditArcherID;                 //Set in editor per enemy?

        [Header("Health Meter")]
        [Tooltip("The transforms used representing enemy health")]
        public Transform[] _healthTransform;
        private BanditStats _banditStats;


        private Transform _transform;
        private CharacterController2D _controller2D;
        private Animator _animator;


        private BanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;
        private float _playerAttackedButtonTime;
        private ITimer _healthDisplayTimer, _bleedingTimer;

        private Action[] _actionListener = new Action[1];

        private void InitializeBandit()
        {
            _transform = this.transform;
            _controller2D = this.GetComponent<CharacterController2D>();
            _animator = this.GetComponent<Animator>();

            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>(), "EnemyAnimControllers", "HumanoidBandit", "Bandit_with_nothing_controller");
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _banditArcherID);

            _banditStats = new BanditStats(ref _healthTransform, ref _healthTransform, 100.0, 100.0, 0.0);
            _healthDisplayTimer = this.gameObject.AddComponent<Timer>();
            _bleedingTimer = this.gameObject.AddComponent<Timer>();


            _actionListener[0] = new Action(AttackedByPlayer);//AttackedByPlayer()


        }
        private void OnEnable()
        {
            //Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 
            BattleColliderManager.Subscribe("ReportCollisionWithBandit" + _banditArcherID.ToString(), _actionListener[0]);
            //AITargetTrackingManager.Subscribe("ReportDetectionWithPlayerForBandit" + _banditArcherID.ToString(), _actionListener[1]);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithBandit" + _banditArcherID.ToString(), _actionListener[0]);
            //AITargetTrackingManager.Unsubscribe("ReportDetectionWithPlayerForBandit" + _banditArcherID.ToString(), _actionListener[1]);
        }
        private void Awake() => InitializeBandit();

        void Start()
        {
            _banditAnimator.SetAnimatorController(BanditAnimatorController.Bandit_with_broadsword_controller);

            SetState(new BanditIdle(this, Vector3.zero, _banditArcherID));
        }
        void Update()
        {
            UpdateStateBehaviours();
            UpdateStats();
        }
        public void UpdateStateBehaviours()
        {
            //_state.IsIdle(ref _animator, ref _banditCollider);


            _state.IsDying(ref _animator, ref _banditSprite);
            _state.IsDead(ref _animator, ref _banditSprite);

            _state.UpdateBehaviour(ref _controller2D, ref _animator, ref _transform);
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
                    SetState(new BanditDying(this, _velocity, _banditArcherID));
                    _banditStats.DisplayHealthFillBar(false);
                    _banditStats.DisplayBleedFillBar(false);
                    _banditStats.Disable();
                }
                return;
            }

        }
        private void AttackedByPlayer()
        {
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

            _banditStats.Health -= playerAttackDamage;
            _healthDisplayTimer.StartTimer(1.0f);
        }

        private void OnDrawGizmosSelected()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, 14.0f);

        }
    }
}


