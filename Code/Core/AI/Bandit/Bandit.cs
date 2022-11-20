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
    public class Bandit : MonoBehaviour, IEnemy
    {
        [Header("Bandit ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? enemies.
        public int _banditID;               //Set in editor per enemy?

        [Header("Enemy Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK

        private IEnemyStateMachine _banditState;
        private IBanditBehaviours _banditBehaviours;
        private IBanditAnimator _banditAnimator;
        private IBanditSprite _banditSprite;
        private IBanditCollision _banditCollider;

        private Action _actionListener;

        private void InitializeBandit()
        {
            _banditState = new EnemyStateMachine(state.IsIdle);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>());
            _banditCollider = new BanditCollision(this.GetComponent<Collider2D>(), ref _attackPoints, _banditID);

            _banditBehaviours = this.gameObject.AddComponent<BanditBehaviours>();
            _banditBehaviours.Setup(this.transform, this.GetComponent<Controller2D>());
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
        }
        void Update() 
        {
            //UpdateStateBehaviours();
            UpdateCollisions();
            UpdateAnimator(); 
            //UpdatePrintMsg();
        }

        public void UpdatePlayerPathFinding()
		{

		}
        public void UpdateStateBehaviours()
		{
            //switch (_banditState.GetEnemyState())
            //{
            //    case state.IsIdle:
            //    case state.IsDefenceRelease:
            //        _banditAnimator.SetAnimationState(AnimationState.IdleAnim);
            //        _banditBehaviours.IdleProcess(_banditState, _banditCollider);
            //        break;
            //    case state.IsWaiting:
            //        _banditAnimator.SetAnimationState(AnimationState.IdleAnim);
            //        _banditBehaviours.WaitingProcess(_banditState);
            //        break;
            //    case state.IsMoving:
            //        _banditAnimator.SetAnimationState(AnimationState.MoveAnim);
            //        _banditBehaviours.PersueTarget(_banditState, _banditCollider.GetRecentCollision(), _banditSprite);
            //        break;
            //    case state.IsJumping:
            //        //if (_banditBehaviours.JumpProcess(_banditState))
            //        //    _banditAnimator.SetAnimationState(AnimationState.JumpAnim);
            //        break;
            //    case state.IsFalling:
            //        _banditAnimator.SetAnimationState(AnimationState.FallenAnim);
            //        _banditBehaviours.FallProcess(_banditState, _banditSprite, _banditID);
            //        break;
            //    case state.IsQuickAttack:
            //        _banditAnimator.SetAnimationState(AnimationState.QuickAtkAnim);
            //        _banditBehaviours.QuickAttackProcess(_banditState, _banditSprite);
            //        _banditCollider.EnableTargetCollisionDetection();
            //        break;
            //    case state.IsAttackPrepare:
            //        _banditAnimator.SetAnimationState(AnimationState.HoldAtkAnim);
            //        //_banditBehaviours.HoldAttackProcess(_banditState);
            //        //_banditSprite.SetWeaponChargeTextureFXFlag(true);
            //        break;
            //    case state.IsAttackRelease:
            //        _banditAnimator.SetAnimationState(AnimationState.ReleaseAtkAnim);
            //        //_banditBehaviours.ReleaseAttackProcess(_banditState);
            //        //_banditCollider.EnableAttackCollisions();
            //        //_banditSprite.SetWeaponChargeTextureFXFlag(false);
            //        break;
            //    case state.IsDefencePrepare:
            //        //_banditAnimator.SetAnimationState(AnimationState.DefendAnim);
            //        //_banditBehaviours.IdleDefenceProcess(_banditState);
            //        break;
            //    case state.IsDefenceMoving:
            //        _banditAnimator.SetAnimationState(AnimationState.DefendMoveAnim);
            //        //_banditBehaviours.IdleDefenceProcess(_banditState);
            //        break;
            //    case state.IsHitByQuickAttack:
            //        _banditAnimator.SetAnimationState(AnimationState.SmallHitAnim);
            //        _banditBehaviours.HitByQuickAttackProcess(_banditState, _banditSprite);
            //        break;
            //    case state.IsHitByReleaseAttack:
            //        _banditAnimator.SetAnimationState(AnimationState.PowerHitAnim);
            //        _banditBehaviours.HitByPowerAttackProcess(_banditCollider.GetRecentCollision(), _banditID);
            //        //_banditBehaviours.HitByPowerAttackProcess(_banditState, _banditSprite);
            //        break;

            //}
            //_banditBehaviours.UpdateMovement(_banditState, _banditSprite, _banditCollider);//UpdateMovement();
            //UpdateMovement();
        }
        public void UpdateAnimator()
		{
            _banditAnimator.UpdateAnimator(_banditBehaviours);
        }
        public void UpdateCollisions()
		{
            _banditCollider.UpdateCollision(_banditState, _banditSprite);
        }

        private void AttackedByPlayer()
		{
            print("\nBandit.cs= AttackedByPlayer() called!");
		}

        private void UpdatePrintMsg()
        {
            print("\n_banditState=" + _banditState.GetEnemyState());
            print("\n_animationState=" + _banditAnimator.GetAnimationState());
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


