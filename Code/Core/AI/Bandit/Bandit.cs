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
        [Header("Player ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? enemies.
        public int _enemyID;               //Set in editor per enemy?

        [Header("Enemy Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK

        private IEnemyStateMachine _banditState;
        //private IPlayerInput _playerInput;
        private IBanditBehaviours _banditBehaviours;
        private IBanditAnimator _banditAnimator;
        //private IPlayerSprite _playerSprite;
        //private IPlayerCollision _playerCollider;

        private void InitializeBandit()
        {
            _banditState = new EnemyStateMachine(state.IsIdle);
            _banditAnimator = new BanditAnimator(this.GetComponent<Animator>());
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
            UpdateStateBehaviours();
            UpdateAnimator(); //Finish removing previous condition links within editor for remaining bandit anim controllers.
            UpdatePrintMsg();
        }

        public void UpdatePlayerPathFinding()
		{

		}
        public void UpdateStateBehaviours()
		{
            switch (_banditState.GetEnemyState())
            {
                case state.IsIdle:
                case state.IsDefenceRelease:
                    _banditAnimator.SetAnimationState(AnimationState.IdleAnim);
                    //_banditBehaviours.IdleProcess(_banditState);
                    break;
                case state.IsMoving:
                    _banditAnimator.SetAnimationState(AnimationState.MoveAnim);
                    break;
                case state.IsJumping:
                    //if (_banditBehaviours.JumpProcess(_banditState))
                    //    _banditAnimator.SetAnimationState(AnimationState.JumpAnim);
                    break;
                case state.IsFalling:
                    _banditAnimator.SetAnimationState(AnimationState.FallenAnim);
                    //_banditBehaviours.FallProcess(_banditState);
                    break;
                case state.IsQuickAttack:
                    _banditAnimator.SetAnimationState(AnimationState.QuickAtkAnim);
                    //_banditBehaviours.QuickAttackProcess(_banditState, _playerSprite);
                    //_banditCollider.EnableAttackCollisions();
                    break;
                case state.IsAttackPrepare:
                    _banditAnimator.SetAnimationState(AnimationState.HoldAtkAnim);
                    //_banditBehaviours.HoldAttackProcess(_banditState);
                    //_banditSprite.SetWeaponChargeTextureFXFlag(true);
                    break;
                case state.IsAttackRelease:
                    _banditAnimator.SetAnimationState(AnimationState.ReleaseAtkAnim);
                    //_banditBehaviours.ReleaseAttackProcess(_banditState);
                    //_banditCollider.EnableAttackCollisions();
                    //_banditSprite.SetWeaponChargeTextureFXFlag(false);
                    break;
                case state.IsDefencePrepare:
                    //_banditAnimator.SetAnimationState(AnimationState.DefendAnim);
                    //_banditBehaviours.IdleDefenceProcess(_banditState);
                    break;
                case state.IsDefenceMoving:
                    _banditAnimator.SetAnimationState(AnimationState.DefendMoveAnim);
                    //_banditBehaviours.IdleDefenceProcess(_banditState);
                    break;

            }
            //UpdateMovement();
        }
        public void UpdateAnimator()
		{
            _banditAnimator.UpdateAnimator(_banditBehaviours);
        }
        public void UpdateCollisions()
		{

		}

        private void UpdatePrintMsg()
        {
            print("\n_banditState=" + _banditState.GetEnemyState());
            print("\n_animationState=" + _banditAnimator.GetAnimationState());
        }
    }
}


