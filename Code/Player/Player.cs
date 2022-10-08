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
    public class Player : MonoBehaviour, IPlayer
    {
        [Header("Player ID")]
        [Tooltip("ID ranges from 0 to 3")]  //Max 4 players.
        public int _playerID;               //Set in editor per player.

        private IPlayerStateMachine _playerState;
        private IPlayerInput _playerInput;
        private IPlayerBehaviours _playerBehaviours;
        private IPlayerAnimator _playerAnimator;

        private void InitializePlayer()
		{
            _playerState = new PlayerStateMachine(state.IsIdle);
            _playerInput = new PlayerInput(_playerID);
            _playerBehaviours = new PlayerBehaviours(this.transform, this.GetComponent<Controller2D>());
            _playerAnimator = new PlayerAnimator(this.GetComponent<Animator>());
        }

		private void Awake()
		{
            InitializePlayer();
		}

		void Start()
        {

        }

        void Update()
        {
            UpdateInput();
            UpdateStateBehaviours();
            UpdateAnimator();
        }

        public void UpdateInput()
		{
            _playerInput.ResetInput();
            _playerInput.UpdateInput();
            switch(_playerInput.GetInputState())
			{
                case PlayerInput.inputState.Empty:
                    break;
                case PlayerInput.inputState.Jump:
                    _playerState.SetPlayerState(state.IsJumping);
                    break;
                case PlayerInput.inputState.Attack:
                    _playerState.SetPlayerState(state.IsQuickAttack);
                    break;
            }
            
		}

        public void UpdateStateBehaviours()
		{
           switch (_playerState.GetPlayerState())
			{
                case state.IsIdle:
                    _playerBehaviours.IdleProcess();
                    break;
                case state.IsJumping:
                    _playerBehaviours.JumpProcess();
                    break;
                case state.IsQuickAttack:
                    _playerBehaviours.QuickAttackProcess();
                    break;
            }
            _playerBehaviours.UpdateMovement(_playerInput.GetInputVector2(), _playerState);//UpdateMovement();
        }

        public void UpdateAnimator()
		{
            _playerAnimator.UpdateAnimator();
        }
    }
}

