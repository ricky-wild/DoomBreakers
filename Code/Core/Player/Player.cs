using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoomBreakers
{
    [RequireComponent(typeof(Controller2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IPlayer
    {
        [Header("Player ID")]
        [Tooltip("ID ranges from 0 to 3")]  //Max 4 players.
        public int _playerID;               //Set in editor per player.

        [Header("Player Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK

        private IPlayerStateMachine _playerState;
        private IPlayerInput _playerInput;
        private IPlayerBehaviours _playerBehaviours;
        private IPlayerAnimator _playerAnimator;
        private IPlayerSprite _playerSprite;
        private IPlayerCollision _playerCollider;

        private void InitializePlayer()
		{
            _playerState = new PlayerStateMachine(state.IsIdle);
            _playerInput = new PlayerInput(_playerID);
            _playerAnimator = new PlayerAnimator(this.GetComponent<Animator>());
            _playerCollider = new PlayerCollision(this.GetComponent<Collider2D>(), ref _attackPoints);
            //_playerSprite = new PlayerSprite(this.GetComponent<SpriteRenderer>(), _playerID);

            //PlayerBehaviours.cs ALSO needs to inherit from MonoBehaviour.
            //Creating a new Obj() constructor is not allowed under this case.
            //In Unity anything using MonoBehaviour must be attached to an gameobject,
            //or the script will just exist somewhere & won't work.
            //_playerBehaviours = new PlayerBehaviours(this.transform, this.GetComponent<Controller2D>());
            _playerBehaviours = this.gameObject.AddComponent<PlayerBehaviours>();
            _playerBehaviours.Setup(this.transform, this.GetComponent<Controller2D>());
            _playerSprite = this.gameObject.AddComponent<PlayerSprite>();
            _playerSprite.Setup(this.GetComponent<SpriteRenderer>(), _playerID);
            //_playerCollider = this.gameObject.AddComponent<PlayerCollision>();
            //_playerCollider.Setup(this.GetComponent<Collider2D>(), ref _attackPoints);
        }

		private void Awake()
		{
            InitializePlayer();
		}

		void Start()
        {
            _playerAnimator.SetAnimatorController(AnimatorController.Player_with_broadsword_with_shield_controller, false);
        }

        void Update()
        {
            UpdateInput();
            UpdateStateBehaviours();
            UpdateCollisions();
            UpdateAnimator();
            //UpdatePrintMsg();
        }

        public void UpdateInput()
		{
            _playerInput.ResetInput();
            _playerInput.UpdateInput();
            switch(_playerInput.GetInputState())
			{
                case PlayerInput.inputState.Empty:
                    //_playerState.SetPlayerState(state.IsIdle); //Cannot do this without interfering with other anims.
                    break;
                case PlayerInput.inputState.Jump:
                    //if(_playerState.GetPlayerState() != state.IsJumping || _playerState.GetPlayerState() != state.IsFalling)
                    _playerState.SetPlayerState(state.IsJumping);
                    break;
                case PlayerInput.inputState.Attack:
                    _playerState.SetPlayerState(state.IsQuickAttack);
                    break;
                case PlayerInput.inputState.UpwardAttack:
                    _playerState.SetPlayerState(state.IsUpwardAttack);
                    break;
                case PlayerInput.inputState.HoldAttack:
                    _playerState.SetPlayerState(state.IsAttackPrepare);
                    break;
                case PlayerInput.inputState.ReleaseAttack:
                    _playerState.SetPlayerState(state.IsAttackRelease);
                    break;
                case PlayerInput.inputState.KnockBackAttack:
                    _playerState.SetPlayerState(state.IsKnockBackAttack);
                    break;
                case PlayerInput.inputState.Defend:
                    if(_playerInput.GetInputVector2().x != 0.0f)
                        _playerState.SetPlayerState(state.IsDefenceMoving);
                    else
                        _playerState.SetPlayerState(state.IsDefencePrepare);
                    break;
                case PlayerInput.inputState.DefenceReleased:
                    _playerState.SetPlayerState(state.IsDefenceRelease);
                    break;
                case PlayerInput.inputState.DodgeL:
                    if(_playerState.GetPlayerState() != state.IsDodgeRelease)
                        _playerState.SetPlayerState(state.IsDodgeLPrepare);
                    break;
                case PlayerInput.inputState.DodgeR:
                    if (_playerState.GetPlayerState() != state.IsDodgeRelease)
                        _playerState.SetPlayerState(state.IsDodgeRPrepare);
                    break;
                case PlayerInput.inputState.Sprint:
                    if (_playerInput.GetInputVector2().x != 0.0f)
                        _playerState.SetPlayerState(state.IsSprinting);
                    break;
            }
            
		}

        public void UpdateStateBehaviours()
		{
            
            switch (_playerState.GetPlayerState())
			{
                case state.IsIdle:
                case state.IsDefenceRelease:
                    _playerAnimator.SetAnimationState(AnimationState.IdleAnim);
                    _playerBehaviours.IdleProcess(_playerState);
                    break;
                case state.IsMoving:
                    _playerAnimator.SetAnimationState(AnimationState.MoveAnim);
                    break;
                case state.IsSprinting:
                    _playerAnimator.SetAnimationState(AnimationState.SprintAnim);
                    break;
                case state.IsJumping:                  
                    if(_playerBehaviours.JumpProcess(_playerState))
                        _playerAnimator.SetAnimationState(AnimationState.JumpAnim);
                    break;
                case state.IsFalling:
                    _playerAnimator.SetAnimationState(AnimationState.FallenAnim);
                    _playerBehaviours.FallProcess(_playerState);
                    break;
                case state.IsQuickAttack:
                    _playerAnimator.SetAnimationState(AnimationState.QuickAtkAnim);
                    _playerBehaviours.QuickAttackProcess(_playerState, _playerSprite);//, _playerCollider);
                    _playerCollider.EnableAttackCollisions();
                    break;
                case state.IsUpwardAttack:
                    _playerAnimator.SetAnimationState(AnimationState.UpwardAtkAnim);
                    _playerBehaviours.UpwardAttackProcess(_playerState, _playerSprite);
                    _playerCollider.EnableAttackCollisions();
                    break;
                case state.IsAttackPrepare:
                    _playerAnimator.SetAnimationState(AnimationState.HoldAtkAnim);
                    _playerBehaviours.HoldAttackProcess(_playerState);
                    _playerSprite.SetWeaponChargeTextureFXFlag(true);
                    break;
                case state.IsAttackRelease:
                    _playerAnimator.SetAnimationState(AnimationState.ReleaseAtkAnim);
                    _playerBehaviours.ReleaseAttackProcess(_playerState);
                    _playerCollider.EnableAttackCollisions();
                    _playerSprite.SetWeaponChargeTextureFXFlag(false);
                    break;
                case state.IsKnockBackAttack:
                    _playerAnimator.SetAnimationState(AnimationState.KnockBackAtkAnim);
                    _playerBehaviours.KnockbackAttackProcess(_playerState);
                    _playerCollider.EnableAttackCollisions();
                    break;
                case state.IsDefencePrepare:
                    _playerAnimator.SetAnimationState(AnimationState.DefendAnim);
                    _playerBehaviours.IdleDefenceProcess(_playerState);
                    break;
                case state.IsDefenceMoving:
                    _playerAnimator.SetAnimationState(AnimationState.DefendMoveAnim);
                    _playerBehaviours.IdleDefenceProcess(_playerState);
                    break;
                case state.IsQuickHitWhileDefending:
                case state.IsHitWhileDefending:
                    _playerAnimator.SetAnimationState(AnimationState.DefendHitAnim);
                    _playerBehaviours.IdleDefenceProcess(_playerState);
                    break;
                case state.IsDodgeLPrepare:
                    _playerAnimator.SetAnimationState(AnimationState.DodgeAnim);
                    _playerBehaviours.DodgeInitiatedProcess(_playerState, true, _playerSprite, _playerCollider);
                    break;
                case state.IsDodgeRPrepare:
                    _playerAnimator.SetAnimationState(AnimationState.DodgeAnim);
                    _playerBehaviours.DodgeInitiatedProcess(_playerState, false, _playerSprite, _playerCollider);
                    break;
                case state.IsDodgeRelease:
                    _playerBehaviours.DodgeReleasedProcess(_playerState);
                    break;
                case state.IsHitByQuickAttack:
                    _playerAnimator.SetAnimationState(AnimationState.SmallHitAnim);
                    _playerBehaviours.HitByQuickAttackProcess(_playerState, _playerSprite);
                    break;
            }
            _playerBehaviours.UpdateMovement(_playerInput.GetInputVector2(), _playerState, _playerSprite, _playerCollider);//UpdateMovement();
        }

        public void UpdateAnimator()
		{
            _playerAnimator.UpdateAnimator(_playerBehaviours);
        }

        public void UpdateCollisions()
		{
            _playerCollider.UpdateCollision(_playerState, _playerSprite, _playerID);
		}
        public void ReportCollisionWithEnemy(ICollisionData collisionData, int banditId)//IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
        {
            //_playerState = _playerCollider.RegisterHitByAttack(enemyStateMachine, _playerState, _playerSprite, banditSprite);
            collisionData.PluginPlayer(this, _playerID);
            collisionData.PluginPlayerState(_playerState, _playerID);
            collisionData.PluginPlayerSprite(_playerSprite, _playerID);
            _playerState = _playerCollider.RegisterHitByAttack(collisionData, _playerID, banditId);
        }

        private void UpdatePrintMsg()
		{
            print("\n_playerState=" + _playerState.GetPlayerState());
            print("\n_animationState=" + _playerAnimator.GetAnimationState());
            print("\n_playerInput.GetInputState()=" + _playerInput.GetInputState());
            print("\n_playerCollider=");
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

