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
        private Controller2D _controller2D;
        private Animator _animator;

        private IBanditCollision _banditCollider;
        private IBanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;

        private Action _actionListener;

        private void InitializeBandit()
        {
            _controller2D = this.GetComponent<Controller2D>();
            _animator = this.GetComponent<Animator>();

            _banditCollider = new BanditCollision(this.GetComponent<Collider2D>(), ref _attackPoints, _banditID);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>());
            _banditSprite = this.gameObject.AddComponent<BanditSprite>();
            _banditSprite.Setup(this.GetComponent<SpriteRenderer>(), _banditID);

            _actionListener = new Action(AttackedByPlayer);//AttackedByPlayer()

        }
        private void OnEnable()
        {
            //Player.cs->PlayerCollision.cs->enemy.GetComponent<Bandit>()->BattleColliderManager.TriggerEvent("ReportCollisionWithBandit"); 
            BattleColliderManager.Subscribe("ReportCollisionWithBandit", _actionListener);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithBandit", _actionListener);
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
            //UpdateStateBehaviours();
            UpdateCollisions();
        }

        public void UpdatePlayerPathFinding()
		{

		}
        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator);
            _state.IsWaiting(ref _animator);
            _state.IsPersueTarget(ref _animator);
        }

        public void UpdateCollisions()
		{
            //_banditCollider.UpdateCollision(_banditState, _banditSprite);
        }

        private void AttackedByPlayer()
		{
            print("\nBandit.cs= AttackedByPlayer() called!");
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


