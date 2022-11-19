using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoomBreakers
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Controller2D))]
    public class Player : MyPlayerStateMachine, IPlayer
    {
        [Header("Player ID")]
        [Tooltip("ID ranges from 0 to 3")]  //Max 4 players.
        public int _playerID;               //Set in editor per player.

        [Header("Player Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK

        private Rewired.Player _rewirdInputPlayer;
        private Vector2 _inputVector2;
        private Transform _playerTransform;
        private Controller2D _controller2D;
        private Animator _animator;

        private IPlayerStateMachine _playerState;
        private PlayerInput _playerInput;
        private IPlayerBehaviours _playerBehaviours;
        private IPlayerAnimator _playerAnimator;
        private IPlayerSprite _playerSprite;
        private IPlayerCollision _playerCollider;
        private IPlayerEquipment _playerEquipment;

        private void InitializePlayer()
		{
            _playerState = new PlayerStateMachine(state.IsIdle);
            _playerInput = new PlayerInput(_playerID, this.transform, this.GetComponent<Controller2D>());//, _playerStateMachine);


            _playerTransform = this.transform;
            _controller2D = this.GetComponent<Controller2D>();
            _rewirdInputPlayer = ReInput.players.GetPlayer(_playerID);
            _inputVector2 = new Vector2();
            _animator = this.GetComponent<Animator>();


            _playerAnimator = new PlayerAnimator(this.GetComponent<Animator>());
            _playerEquipment = new PlayerEquipment(PlayerEquipType.Empty_None, PlayerEquipType.Empty_None, PlayerEquipType.Empty_None);



            _playerBehaviours = this.gameObject.AddComponent<PlayerBehaviours>();
            _playerBehaviours.Setup(this.transform, this.GetComponent<Controller2D>());
            _playerSprite = this.gameObject.AddComponent<PlayerSprite>();
            _playerSprite.Setup(this.GetComponent<SpriteRenderer>(), _playerID);
            _playerCollider = this.gameObject.AddComponent<PlayerCollision>(); //Required for OnTriggerEnter2D()
            _playerCollider.Setup(this.GetComponent<Collider2D>(), ref _attackPoints);
        }

		private void Awake()
		{
            InitializePlayer();
		}

		void Start()
        {
            _playerAnimator.SetAnimatorController(_playerEquipment);//AnimatorController.Player_with_broadsword_with_shield_controller, false);
            SetState(new PlayerIdle(this, _inputVector2));
        }

        void Update()
        {
            UpdateInput();
            UpdateStateBehaviours();
            //UpdateCollisions();
            //UpdateAnimator();
        }



        public void UpdateInput()
		{
            if (_rewirdInputPlayer == null)
                return;

            _inputVector2.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");
            _inputVector2.y = _rewirdInputPlayer.GetAxis("MoveVertical");

            if (_inputVector2.x > 0f || _inputVector2.x < 0f)
            {
                if (SafeToSetMove())
                    SetState(new PlayerMove(this, _inputVector2));
            }
            if (Mathf.Abs(_inputVector2.x) == 0f && Mathf.Abs(_inputVector2.y) == 0f)//else
            {
                if (SafeToSetIdle())
                    SetState(new PlayerIdle(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("Jump"))
            {
                if (SafeToSetJump())
                    SetState(new PlayerJump(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeL"))
            {
                _inputDodgedLeft = true;
                SetState(new PlayerDodge(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeR"))
            {
                _inputDodgedLeft = false;
                SetState(new PlayerDodge(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
            {
                if (_inputVector2.y > 0.44f)
                    return;// _inputState = inputState.UpwardAttack;
                else
                    SetState(new PlayerQuickAttack(this, _inputVector2));
            }

        }

        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator);
            _state.IsMoving(ref _animator, ref _inputVector2, ref _playerSprite, ref _playerCollider);
            _state.IsJumping(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsFalling(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsDodging(ref _animator, ref _controller2D, ref _inputVector2, _inputDodgedLeft, ref _playerSprite, ref _playerCollider);
            _state.IsDodged(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsQuickAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.UpdateBehaviour(ref _controller2D, ref _animator);
        }

        public void UpdateAnimator()
		{
            _playerAnimator.UpdateAnimator(_playerBehaviours);
        }

        public void UpdateCollisions()
		{
            _playerCollider.UpdateCollision(_playerState, _playerSprite, _playerID, _playerEquipment);
		}
        public void ReportCollisionWithEnemy(ICollisionData collisionData, int banditId)//IEnemyStateMachine enemyStateMachine, IBanditSprite banditSprite)
        {
            //_playerState = _playerCollider.RegisterHitByAttack(enemyStateMachine, _playerState, _playerSprite, banditSprite);
            collisionData.PluginPlayer(this, _playerID);
            collisionData.PluginPlayerState(_playerState, _playerID);
            collisionData.PluginPlayerSprite(_playerSprite, _playerID);
            _playerState = _playerCollider.RegisterHitByAttack(collisionData, _playerID, banditId);
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

