using Rewired;
using System;
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
        private Controller2D _controller2D;
        private Animator _animator;

        private IPlayerCollision _playerCollider;
        private IPlayerEquipment _playerEquipment;
        private IPlayerAnimator _playerAnimator;
        private IPlayerSprite _playerSprite;
        private PlayerStats _playerStats;
        private ITimer _buttonHeldTimer, _staminaTimer;

        private Action[] _actionListener = new Action[2];

        private void InitializePlayer()
		{
            _controller2D = this.GetComponent<Controller2D>();
            _rewirdInputPlayer = ReInput.players.GetPlayer(_playerID);
            _inputVector2 = new Vector2();
            _animator = this.GetComponent<Animator>();

            _playerCollider = this.gameObject.AddComponent<PlayerCollision>(); //Required for OnTriggerEnter2D()
            _playerCollider.Setup(this.GetComponent<Collider2D>(), ref _attackPoints, _playerID);
            
            _playerEquipment = new PlayerEquipment(_playerID);
            _playerAnimator = new PlayerAnimator(this.GetComponent<Animator>());
            _playerSprite = this.gameObject.AddComponent<PlayerSprite>();
            _playerSprite.Setup(this.GetComponent<SpriteRenderer>(), _playerID);
            _playerStats = new PlayerStats(100.0, 100.0, 0.0);

            _buttonHeldTimer = new Timer();
            _staminaTimer = new Timer();
            _staminaTimer.StartTimer(0.05f); //increment stamina every 20th of a sec.

            _actionListener[0] = new Action(AttackedByBandit);//AttackedByBandit()
        }
        private void OnEnable()
        {
            ////Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 
            //BattleColliderManager.Subscribe("ReportCollisionWithPlayer" + _playerID.ToString(), _actionListener);
            BattleColliderManager.Subscribe("ReportCollisionWithPlayerFor" + _playerID.ToString(), _actionListener[0]);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithPlayerFor" + _playerID.ToString(), _actionListener[0]);
        }
        private void Awake()
		{
            InitializePlayer();
		}

		void Start()
        {
            //_playerEquipment.ApplySword(PlayerEquipType.Broadsword_Steel, PlayerItem.IsBroadsword);
            _playerAnimator.SetAnimatorController(ref _playerEquipment);//AnimatorController.Player_with_broadsword_with_shield_controller, false);

            SetState(new PlayerIdle(this, _inputVector2));
        }

        void Update()
        {
            UpdateInput();
            UpdateStateBehaviours();
            UpdateCollisions();
            UpdateStats();
        }


        public float GetAttackButtonHeldTime() => _buttonHeldTimer.GetTimeRecord();
        public void UpdateInput()
		{
            if (_rewirdInputPlayer == null)
                return;

            if (_state.GetType() == typeof(PlayerGainedEquipment))
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
                if (SafeToSetIdle()) SetState(new PlayerIdle(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("Sprint"))
               return;
            if (_rewirdInputPlayer.GetButtonUp("Sprint"))
                return;
            if (_rewirdInputPlayer.GetButtonDown("Jump"))
            {
                if (SafeToSetJump())
                    SetState(new PlayerJump(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeL"))
            {
                _inputDodgedLeft = true;
                if (_state.GetType() != typeof(PlayerDodge))
                {
                    SetState(new PlayerDodge(this, _inputVector2));
                    _playerStats.Stamina -= 0.05;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                }
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeR"))
            {
                _inputDodgedLeft = false;
                if (_state.GetType() != typeof(PlayerDodge))
                {
                    SetState(new PlayerDodge(this, _inputVector2));
                    _playerStats.Stamina -= 0.05;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                }
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
            {
                if (_inputVector2.y > 0.44f)
                    SetState(new PlayerUpwardAttack(this, _inputVector2));
                else
                {
                    if (_state.GetType() != typeof(PlayerHoldAttack))
                    {
                        SetState(new PlayerQuickAttack(this, _inputVector2));//, _quickAttackIncrement));
                    }
                }
                _playerCollider.EnableAttackCollisions();
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("KnockBack", 0.01f))
			{
                SetState(new PlayerKnockAttack(this, _inputVector2));
                _playerCollider.EnableAttackCollisions();
                _playerStats.Stamina -= 0.08;
                UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
            }
            if (_rewirdInputPlayer.GetButtonDown("Defend"))
			{
                SetState(new PlayerDefend(this, _inputVector2));
			}

            if (_rewirdInputPlayer.GetButtonUp("Defend"))
			{
                //if (_state.GetType() == typeof(PlayerDefend))
                    SetState(new PlayerIdle(this, _inputVector2));
			}
            if (_rewirdInputPlayer.GetButtonTimedPressDown("Attack", 0.25f))
			{
                if(SafeToSetHoldAttack())
				{
                    _buttonHeldTimer.BeginTimeRecord();
                    SetState(new PlayerHoldAttack(this, _inputVector2));
                    _playerStats.Stamina -= 0.125;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                }
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.25f))
			{
                if (_state.GetType() == typeof(PlayerHoldAttack))
                {
                    _buttonHeldTimer.FinishTimeRecord();
                    BattleColliderManager.SetPlayerHeldAttackButtonTime(_buttonHeldTimer.GetTimeRecord());
                    SetState(new PlayerReleaseAttack(this, _inputVector2));
                    _playerCollider.EnableAttackCollisions();
                }
            }
        }
        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator);
            _state.IsGainedEquipment(ref _animator, ref _playerSprite, ref _playerEquipment);
            _state.IsMoving(ref _animator, ref _inputVector2, ref _playerSprite, ref _playerCollider);
            _state.IsJumping(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsFalling(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsDodging(ref _animator, ref _controller2D, ref _inputVector2, _inputDodgedLeft, ref _playerSprite, ref _playerCollider);
            _state.IsDodged(ref _animator, ref _controller2D, ref _inputVector2);
            _state.IsQuickAttack(ref _animator, ref _playerSprite, ref _inputVector2, ref _quickAttackIncrement);
            _state.IsUpwardAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsKnockAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsHoldAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsReleaseAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsDefending(ref _animator, ref _inputVector2);
            _state.IsHitByQuickAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsHitByReleaseAttack(ref _animator, ref _playerSprite, ref _inputVector2);
            _state.IsHitWhileDefending(ref _animator, ref _inputVector2);
            _state.UpdateBehaviour(ref _controller2D, ref _animator);
        }

        public void UpdateCollisions()
		{
            _playerCollider.UpdateCollision(ref _state, _playerID, ref _playerEquipment, ref _playerSprite);
            if(_playerEquipment.NewEquipmentGained())
			{
                SetState(new PlayerGainedEquipment(this, _velocity));
                _playerAnimator.SetAnimatorController(ref _playerEquipment);
                _playerEquipment.NewEquipmentGained(false);
			}
		}

        private void AttackedByBandit()
		{
            //print("\nPlayer.cs= AttackedByBandit() called!");

            int banditId = BattleColliderManager.GetRecentCollidedBanditId();
            int banditFaceDir = BattleColliderManager.GetAssignedBanditFaceDir(banditId);
            BanditBaseState attackingBanditState = BattleColliderManager.GetAssignedBanditState(banditId);

            if (IsIgnoreDamage())
                return;

            double banditQuickAttackDamage = 0.0025;
            double banditPowerAttackDamage = 0.005;

            if (ProcessQuickAttackFromBandit(ref attackingBanditState, banditFaceDir, _playerSprite.GetSpriteDirection()))
			{
                if(!_playerStats.IsArmored()) _playerStats.Health -= banditQuickAttackDamage;
                else
                    _playerStats.Defence -= banditQuickAttackDamage;
            }
            if(ProcessPowerAttackFromBandit(ref attackingBanditState, banditFaceDir, _playerSprite.GetSpriteDirection()))
			{
                if (!_playerStats.IsArmored()) _playerStats.Health -= banditPowerAttackDamage;
                else
                    _playerStats.Defence -= banditPowerAttackDamage;
            }
            UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
        }
        private void UpdateStats()
		{
            UpdateStamina();

        }
        private void UpdateStamina()
		{
            if (_staminaTimer.HasTimerFinished())
            {
                _playerStats.Stamina += 0.008; //magic numbers are bad.
                UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                _staminaTimer.StartTimer(0.05f);
            }
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

