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
	public class NPC : NPCStateMachine
	{
        private NPCStateMachine _stateMachine;

        [Header("NPC ID")]
        [Tooltip("ID ranges from 0 to ?")]  //Max ? npc.
        public int _npcID;               //Set in editor per enemy?

        [Header("Health Meter")]
        [Tooltip("The transforms used representing enemy health")]
        public Transform[] _healthTransform;

        [Header("Bleed Meter")]
        [Tooltip("The transforms used representing enemy bleeding")]
        public Transform[] _bleedTransform;

        [Header("Bludgeon Meter")]
        [Tooltip("The transforms used representing enemy bludgeon")]
        public Transform[] _bludgeonTransform;

        private Transform _transform;
        private CharacterController2D _controller2D;
        private Animator _animator;

        private NPCCollision _npcCollider;
        private NPCAnimator _npcAnimator;
        private NPCSprite _npcSprite;
        private NPCStats _npcStats;

        private Action[] _actionListener = new Action[4];

        private void InitializeNPC()
        {
            _stateMachine = this;
            _transform = this.transform;
            _controller2D = this.GetComponent<CharacterController2D>();
            _animator = this.GetComponent<Animator>();

            _npcStats = new NPCStats(ref _healthTransform, ref _bleedTransform, ref _bludgeonTransform, 100.0, 100.0, 0.0);
            _npcCollider = new NPCCollision(this.GetComponent<Collider2D>(), ref _transform, ref _npcStats, _npcID);

            //NPCAnimControllers\Standard\NPC_Traveller.controller
            _npcAnimator = new NPCAnimator(this.GetComponent<Animator>(), "NPCAnimControllers", "Standard", "NPC_Traveller");
            _npcSprite = this.gameObject.AddComponent<NPCSprite>();
            _npcSprite.Setup(this.GetComponent<SpriteRenderer>(), _npcID);

			_actionListener[0] = new Action(FollowDetected);//FollowDetected()
			_actionListener[1] = new Action(JumpDetected);//JumpDetected()
			_actionListener[2] = new Action(RestDetected);//RestDetected()
			_actionListener[3] = new Action(FleeDetected);//FleeDetected()

		}
        private void OnEnable()
        {
            //NPC.cs->NPCCollision.cs->UpdateTargetDetection(); 
            AIEventNPCManager.Subscribe("ReportFollowDetection" + _npcID.ToString(), _actionListener[0]);
            AIEventNPCManager.Subscribe("ReportJumpDetection" + _npcID.ToString(), _actionListener[1]);
            AIEventNPCManager.Subscribe("ReportRestDetection" + _npcID.ToString(), _actionListener[2]);
            AIEventNPCManager.Subscribe("ReportFleeDetection" + _npcID.ToString(), _actionListener[3]);
        }
        private void OnDisable()
        {
            //NPC.cs->NPCCollision.cs->UpdateTargetDetection(); 
            AIEventNPCManager.Unsubscribe("ReportFollowDetection" + _npcID.ToString(), _actionListener[0]);
            AIEventNPCManager.Unsubscribe("ReportJumpDetection" + _npcID.ToString(), _actionListener[1]);
            AIEventNPCManager.Unsubscribe("ReportRestDetection" + _npcID.ToString(), _actionListener[2]);
            AIEventNPCManager.Unsubscribe("ReportFleeDetection" + _npcID.ToString(), _actionListener[3]);
        }
        private void Awake() => InitializeNPC();

        void Start()
        {
            SetState(new NPCIdle(this, Vector3.zero, _transform,_npcID));
        }
        void Update()
        {
            UpdateStateBehaviours();
            UpdateCollisions();
            //UpdateStats();
        }
        public void UpdateStateBehaviours()
        {
            _state.IsIdle(ref _npcAnimator);
            _state.IsTravelling(ref _npcAnimator, ref _npcSprite);
            _state.IsFleeing(ref _npcAnimator, ref _npcSprite);
            _state.IsJumping(ref _npcAnimator, ref _npcSprite);
            _state.IsWaiting(ref _npcAnimator, ref _npcSprite);
            _state.IsFalling(ref _npcAnimator, ref _controller2D, ref _npcSprite);
            //_state.IsPersueTarget(ref _animator, ref _banditSprite, ref _banditCollider, ref _banditStats);
            //_state.IsDefending(ref _animator, ref _controller2D, ref _banditSprite);
            //_state.IsDying(ref _animator, ref _banditSprite);
            //_state.IsDead(ref _animator, ref _banditSprite);

            _state.UpdateBehaviour(ref _controller2D, ref _animator, ref _transform, ref _npcStats);
        }
		public void UpdateCollisions()
		{
			//if (IsDying()) return;
			_npcCollider.UpdateCollision(ref _state, _npcSprite, ref _npcStats);
		}
        //private void UpdateStats() //=> _npcStats.UpdateStatus(ref _stateMachine, ref _velocity, _npcID);
        //{
        //    bool SetDeath = SafeToSetDying(); //SafeToSetDying()
        //    _npcStats.UpdateStatus(ref _stateMachine, ref _velocity, _npcID, SetDeath);
        //}

        private void FollowDetected()
		{
            print("\nFollowDetected()");
		}
        private void JumpDetected()
        {
            if (_state.GetType() == typeof(NPCJump)) return;

            //print("\nJumpDetected()");
            SetState(new NPCJump(this, _velocity, _transform, _npcID));
        }
        private void RestDetected()
        {
            if (_state.GetType() == typeof(NPCWaiting)) return;

            //print("\nRestDetected()");
            SetState(new NPCWaiting(this, _velocity, _transform, _npcID));
        }
        private void FleeDetected()
        {
            if (_state.GetType() == typeof(NPCJump)) return;

            //print("\nFleeDetected()");
            SetState(new NPCFlee(this, _velocity, _transform, _npcID));
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform == null) return;

            //0=toFollow, 1=toJump, 2=toRest, 3=toFlee
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_transform.position, _npcCollider.GetDetectRadius(0));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_transform.position, _npcCollider.GetDetectRadius(1));

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_transform.position, _npcCollider.GetDetectRadius(2));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_transform.position, _npcCollider.GetDetectRadius(3));

        }
    }
}
