﻿using Rewired;
using System;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterController2D))]//[RequireComponent(typeof(Controller2D))]
    public class Player : PlayerStateMachine, IPlayer
    {
        private PlayerStateMachine _stateMachine;

        [Header("Player ID")]
        [Tooltip("ID ranges from 0 to 3")]  //Max 4 players.
        public int _playerID;               //Set in editor per player.

        [Header("Player Attack Points")]
        [Tooltip("Vectors that represent point of attack radius")]
        public Transform[] _attackPoints; //1=quickATK, 2=powerATK, 3=upwardATK

        [Header("Player Indicator Icon")]
        [Tooltip("The animator that indicates player ID within game")]
        public Animator _playerIndicatorAnimator;

        private Rewired.Player _rewirdInputPlayer;
        private Vector2 _inputVector2;
        private CharacterController2D _controller2D;//private Controller2D _controller2D;
        private Animator _animator;
        private Transform _transform;

        private PlayerCollision _playerCollider;
        private IPlayerEquipment _playerEquipment;
        private PlayerAnimator _playerAnimator;
        private IPlayerSprite _playerSprite;
        private PlayerStats _playerStats;
        private ITimer _buttonHeldTimer;

        private Action[] _actionListener = new Action[3];

        private bool _setup;

        private void InitializePlayer()
		{
            _setup = false;
            _stateMachine = this;
            _controller2D = this.GetComponent<CharacterController2D>();//this.GetComponent<Controller2D>();
            _controller2D.IgnoreEdgeDetection(true);
            _rewirdInputPlayer = ReInput.players.GetPlayer(_playerID);
            _inputVector2 = new Vector2();
            _animator = this.GetComponent<Animator>();
            _transform = this.transform;
            _playerStats = new PlayerStats(100.0, 100.0, 0.0);

            _playerCollider = this.gameObject.AddComponent<PlayerCollision>(); //Required for OnTriggerEnter2D()
            _playerCollider.Setup(this.GetComponent<Collider2D>(), ref _attackPoints, _playerID, ref _transform, ref _playerStats);
            
            _playerEquipment = new PlayerEquipment(_playerID);
            _playerAnimator = new PlayerAnimator(ref _animator, "HumanAnimControllers", "Unarmored", "Player_with_nothing_controller", ref _playerIndicatorAnimator, _playerID);

            _playerSprite = this.gameObject.AddComponent<PlayerSprite>();
            _playerSprite.Setup(this.GetComponent<SpriteRenderer>(), _playerID);

            _buttonHeldTimer = new Timer();


            _actionListener[0] = new Action(AttackedByBandit);//AttackedByBandit()
            _actionListener[1] = new Action(AttackedByArrow);//AttackedByArrow()
            _actionListener[2] = new Action(CampsiteReachedEvent);//CampsiteReachedEvent()

            
        }
        private void OnEnable()
        {
            ////Bandit.cs->BanditCollision.cs->enemy.GetComponent<Player>()->BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer"); 

            BattleColliderManager.Subscribe("ReportCollisionWithPlayerFor" + _playerID.ToString(), _actionListener[0]);
            BattleColliderManager.Subscribe("ReportCollisionWithArrowForPlayer" + _playerID.ToString(), _actionListener[1]);
            LevelEventManager.Subscribe("CampsiteReachedEvent", _actionListener[2]);
        }
        private void OnDisable()
        {
            BattleColliderManager.Unsubscribe("ReportCollisionWithPlayerFor" + _playerID.ToString(), _actionListener[0]);
            BattleColliderManager.Unsubscribe("ReportCollisionWithArrowForPlayer" + _playerID.ToString(), _actionListener[1]);
            LevelEventManager.Unsubscribe("CampsiteReachedEvent", _actionListener[2]);
        }
        private void Awake()
		{
            InitializePlayer();
		}

		void Start()
        {
            _playerAnimator.SetAnimatorController(ref _playerEquipment);
            SetState(new PlayerIdle(this, _inputVector2));
        }
        private void Setup()
		{
            if (_playerID == 0)
			{
                LevelEventManager.ActivateStartLevel(_transform); //Only call once from Player 1.
            }
            _setup = true;
        }

        void Update()
        {
            if (!_setup) Setup();


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
            if (IsDying())
                return;
            if (_state.GetType() == typeof(PlayerGainedEquipment))
                return;
            if (LevelEventManager.IsStartOfLevel()) return;
            if (LevelEventManager.IsEndOfLevel()) return;

            _inputVector2.x = _rewirdInputPlayer.GetAxis("MoveHorizontal");
            _inputVector2.y = _rewirdInputPlayer.GetAxis("MoveVertical");


            if (_inputVector2.x > 0f || _inputVector2.x < 0f)
            {
                if (SafeToSetMove())
                    SetState(new PlayerMove(this, _inputVector2, _transform, ref _playerSprite));
            }
            if (Mathf.Abs(_inputVector2.x) == 0f && Mathf.Abs(_inputVector2.y) == 0f)//else
            {
                if (SafeToSetIdle()) SetState(new PlayerIdle(this, _inputVector2));
            }
            if (_rewirdInputPlayer.GetButtonDown("Sprint"))
			{
                _playerStats.Stamina -= 0.1; //magic numbers are bad.
                if (SafeToSetSprint())
                    SetState(new PlayerSprint(this, _inputVector2, _transform, ref _playerSprite));
            }
            if (_rewirdInputPlayer.GetButtonUp("Sprint"))
                return;
            if (_rewirdInputPlayer.GetButtonDown("Jump"))
            {
                if (SafeToSetJump())
				{
                    SetState(new PlayerJump(this, _inputVector2, _transform, ref _playerSprite));
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerJumpSFX);
                }
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeL"))
            {
                _inputDodgedLeft = true;
                if (_state.GetType() != typeof(PlayerDodge))
                {
                    SetState(new PlayerDodge(this, _inputVector2));
                    _playerStats.Stamina -= 0.125;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerDodgeSFX);
                }
            }
            if (_rewirdInputPlayer.GetButtonDown("DodgeR"))
            {
                _inputDodgedLeft = false;
                if (_state.GetType() != typeof(PlayerDodge))
                {
                    SetState(new PlayerDodge(this, _inputVector2));
                    _playerStats.Stamina -= 0.125;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerDodgeSFX);
                }
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.01f))
            {
                if (_inputVector2.y > 0.44f)
                    SetState(new PlayerUpwardAttack(this, _inputVector2, _transform));
                else
                {
                    if (_state.GetType() != typeof(PlayerHoldAttack))
                    {
                        SetState(new PlayerQuickAttack(this, _inputVector2));//, _quickAttackIncrement));
                    }
                }
                
                _playerCollider.EnableAttackCollisions();
                if (_playerCollider.SignalItemPickupCollision()) _playerCollider.EnableItemPickupCollision();
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("KnockBack", 0.01f))
			{
                SetState(new PlayerKnockAttack(this, _inputVector2, _transform));
                _playerCollider.EnableAttackCollisions();
                _playerStats.Stamina -= 0.1;
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
                    SetState(new PlayerHoldAttack(this, _inputVector2, _transform));
                    _playerStats.Stamina -= 0.15;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerChargeAttackSFX);
                }
            }
            if (_rewirdInputPlayer.GetButtonTimedPressUp("Attack", 0.25f))
			{
                if (_state.GetType() == typeof(PlayerHoldAttack))
                {
                    _buttonHeldTimer.FinishTimeRecord();
                    BattleColliderManager.SetPlayerHeldAttackButtonTime(_buttonHeldTimer.GetTimeRecord());
                    SetState(new PlayerReleaseAttack(this, _inputVector2, _transform));
                    _playerCollider.EnableAttackCollisions();
                    AudioEventManager.StopPlayerSFX(PlayerSFXID.PlayerChargeAttackSFX);
                    //AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
                }
            }
        }
        public void UpdateStateBehaviours()
		{
            _state.IsIdle(ref _animator, ref _playerSprite);
            
            _state.IsGainedEquipment(ref _animator, ref _playerSprite, ref _playerEquipment);
            _state.IsBrokenEquipment(ref _animator, ref _playerSprite, ref _playerEquipment);
            _state.IsExhausted(ref _animator, ref _playerSprite);
            
            _state.IsMoving(ref _animator, ref _inputVector2, ref _playerSprite, ref _playerCollider);
            _state.IsSprinting(ref _animator, ref _inputVector2, ref _playerSprite, ref _playerCollider);
            
            _state.IsJumping(ref _animator, ref _controller2D, ref _inputVector2, ref _playerSprite);
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

            _state.IsDying(ref _animator, ref _playerSprite);
            _state.IsDead(ref _animator, ref _playerSprite);
            _state.UpdateBehaviour(ref _controller2D, ref _animator, ref _state);
        }
        public void UpdateCollisions()
		{
            if (!SafeToProcessCollisions()) return;

            _playerCollider.UpdateCollision(ref _state, ref _stateMachine, ref _velocity, _playerID, ref _playerAnimator,
                ref _playerEquipment, ref _playerSprite, ref _playerStats);
        }
        private void AttackedByBandit()
		{
            //print("\nPlayer.cs= AttackedByBandit() called!");

            int enemyId = BattleColliderManager.GetRecentCollidedBanditId();
            int banditFaceDir = BattleColliderManager.GetAssignedBanditFaceDir(enemyId);
            BasicEnemyBaseState attackingBanditState = BattleColliderManager.GetAssignedBanditState(enemyId);

            if (IsIgnoreDamage())
                return;

            BanditStats tempStat = new BanditStats(0,0,0);
            tempStat = BattleColliderManager.GetAssignedBanditStatus(enemyId);

            //if (tempStat == null) return;

            double banditQuickAttackDamage = tempStat.QuickAttackDamage;
            double banditPowerAttackDamage = tempStat.PowerAttackDamage;
            bool process = false;

            if(process = ProcessQuickAttackFromBandit(ref attackingBanditState, banditFaceDir, _playerSprite.GetSpriteDirection(), ref _transform))
			{
                if (!_playerStats.IsArmored())
                {
                    ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_BloodHitFX, _transform, _playerID, _playerSprite.GetSpriteDirection());
                    _playerStats.Health -= banditQuickAttackDamage;
                    AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyHitSFX);//AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX);
                }
                else
                {
                    ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_ArmorHitFX, _transform, _playerID, _playerSprite.GetSpriteDirection());
                    _playerStats.Defence -= banditQuickAttackDamage;
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerArmorHitSFX);
                }
            }
            if(process = ProcessPowerAttackFromBandit(ref attackingBanditState, banditFaceDir, _playerSprite.GetSpriteDirection(), ref _transform))
			{
                if (!_playerStats.IsArmored())
                {
                    _playerStats.Health -= banditPowerAttackDamage;
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
                }
                else
                {
                    ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_ArmorHitFX, _transform, _playerID, _playerSprite.GetSpriteDirection());
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerArmorHitSFX);
                    _playerStats.Defence -= banditPowerAttackDamage;
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerPowerAttackSFX);
                }
            }
            if(process) UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
        }
        private void AttackedByArrow()
		{
            if (IsIgnoreDamage())
                return;

            double arrowAttackDamage = 0.1;//0.0025;
            bool process = false;

            if(process = ProcessArrowAttack(_playerSprite.GetSpriteDirection(),ref _transform))
			{
                if (!_playerStats.IsArmored())
                {
                    ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_BloodHitFX, _transform, _playerID, _playerSprite.GetSpriteDirection());
                    _playerStats.Health -= arrowAttackDamage;
                    AudioEventManager.PlayEnemySFX(EnemySFXID.EnemyHitSFX);//AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerHitSFX);
                }
                else
                {
                    ObjectPooler._instance.InstantiateForPlayer(PrefabID.Prefab_ArmorHitFX, _transform, _playerID, _playerSprite.GetSpriteDirection());
                    _playerStats.Defence -= arrowAttackDamage;
                    AudioEventManager.PlayPlayerSFX(PlayerSFXID.PlayerArmorHitSFX);
                }
            }
            if (process) UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref _playerStats, _playerID);
        }
        private void UpdateStats() //=> _playerStats.UpdateStatus();
		{
            bool SetDeath = SafeToSetDying(); //if(!SafeToSetDying()) return;
            _playerStats.UpdateStatus(ref _stateMachine, ref _transform, ref _playerAnimator, ref _playerEquipment, ref _playerStats,
                ref _playerIndicatorAnimator, ref _velocity, _playerID, SetDeath);
        }
        private void CampsiteReachedEvent()
		{
            print("\nCampsiteReachedEvent() Trigger from Player.cs - do work here.");
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

