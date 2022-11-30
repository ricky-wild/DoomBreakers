using System;
using UnityEngine;

namespace DoomBreakers
{
    [RequireComponent(typeof(Controller2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bandit : BanditStateMachine, IEnemy
    {
        [Header("Bandit ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? enemies.
        public int _banditID;               //Set in editor per enemy?

        [Header("Enemy Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK
        private Transform _transform;
        private Controller2D _controller2D;
        private Animator _animator;

        private IBanditCollision _banditCollider;
        private IBanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;
        private float _playerAttackedButtonTime;

        private Action[] _actionListener = new Action[2];

        private void InitializeBandit()
        {
            _transform = this.transform;
            _controller2D = this.GetComponent<Controller2D>();
            _animator = this.GetComponent<Animator>();

            _banditCollider = new BanditCollision(this.GetComponent<Collider2D>(), ref _attackPoints, _banditID);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>());
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _banditID);

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
        }
        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator, ref _banditCollider);
            _state.IsWaiting(ref _animator);
            _state.IsFalling(ref _animator, ref _controller2D, ref _banditSprite);
            _state.IsPersueTarget(ref _animator, ref _banditSprite, ref _banditCollider);
            _state.IsDefending(ref _animator, ref _controller2D, ref _banditSprite);
            _state.IsQuickAttack(ref _animator, ref _banditCollider, ref _banditSprite, ref _quickAttackIncrement);
            _state.IsHitByQuickAttack(ref _animator, ref _banditSprite);
            _state.IsHitByPowerAttack(ref _animator, ref _banditSprite, _playerAttackedButtonTime);

            _state.UpdateBehaviour(ref _controller2D, ref _animator);
        }

        public void UpdateCollisions()
		{
            _banditCollider.UpdateCollision(ref _state, _banditSprite);
        }

        private void AttackedByPlayer()
		{
            //print("\nBandit.cs= AttackedByPlayer() called!");


            int playerId = BattleColliderManager.GetRecentCollidedPlayerId();
            BaseState attackingPlayerState = BattleColliderManager.GetAssignedPlayerState(playerId);

            if(attackingPlayerState.GetType() == typeof(PlayerQuickAttack))
			{
                if(_state.GetType() != typeof(BanditDefending))
                    SetState(new BanditHitByQuickAttack(this, _velocity, _banditID));
                else
                    SetState(new BanditHitDefending(this, _velocity, _banditID));
            }

            if (attackingPlayerState.GetType() == typeof(PlayerReleaseAttack))
            {
                _playerAttackedButtonTime = BattleColliderManager.GetPlayerHeldAttackButtonTime();
                SetState(new BanditHitByPowerAttack(this, _velocity, _banditID));
            }

            //if (attackingPlayerState.GetType() == typeof(PlayerUpwardAttack))
            //    SetState(new BanditHitByPowerAttack(this, _velocity, _banditID));
        }
        private void DetectedAnPlayer()
		{
            //print("\nBandit.cs= DetectedAnPlayer() called!");
            SetState(new BanditPersue(this, _velocity, 
                _transform,_banditID));
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


