using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{

    public class BanditCollision : MonoBehaviour, IBanditCollision
    {
        public enum CollisionTargetPurpose
		{
            noPurpose = 0,
            toPersue = 1,
            toAttack = 2
		};
        private CollisionTargetPurpose _collisionTargetPurpose; //Flag for player collision purpose. UpdateDetectEnemyTargets()

        private CompareTags _compareTags;

        private Collider2D _collider2d;
        private Collider2D[] _enemyTargetsHit;

        private Transform[] _attackPoints;                            //1=quickATK, 2=powerATK, 3=upwardATK
        private float[] _attackRadius = new float[3];                //1=quickATK, 2=powerATK, 3=upwardATK

        private LayerMask[] _enemyLayerMasks = new LayerMask[2];
        private const string _playerLayerMaskStr = "Player";
        private const string _enemyLayerMaskStr = "Enemy";

        private string[] _compareTagStrings = new string[10];

        private bool _attackCollisionEnabled;

        //private Action _eventListener;
       private IEnemyStateMachine _banditStateMachine;

        public BanditCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints)
        {
            _collider2d = collider2D;
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            _collider2d.enabled = true;
            _attackCollisionEnabled = false;
            _collisionTargetPurpose = CollisionTargetPurpose.noPurpose;
            //_eventListener = new Action(TestMethod);
        }
        //public void TestMethod(){print("TestMethod() activated via event handler!");}
        //public void SetOnEnabled() {//EventManager.StartListening("BanditHitByPlayer", _eventListener);}
        //public void SetOnDisabled(){//EventManager.StopListening("BanditHitByPlayer", _eventListener);}
        public void SetupLayerMasks()
        {
            _enemyLayerMasks[0] = LayerMask.NameToLayer(_playerLayerMaskStr);
            _enemyLayerMasks[1] = LayerMask.NameToLayer(_enemyLayerMaskStr);
        }
        public void SetupAttackRadius()
        {
            _attackRadius[0] = 1.2f; //quick attack radius
            _attackRadius[1] = 1.6f; //power attack radius
            _attackRadius[2] = 1.3f; //upward attack radius

        }
        public void SetupCompareTags()
        {
            _compareTagStrings[0] = "Player";
            _compareTagStrings[1] = "Player2";
            _compareTagStrings[2] = "Player3";
            _compareTagStrings[3] = "Player4";

            _compareTagStrings[4] = "Enemy";
        }

        public string GetCompareTag(CompareTags compareTagId)
        {
            return _compareTagStrings[(int)compareTagId];

        }

        void Start() { }

        void Update() { }

        public void UpdateCollision(IEnemyStateMachine banditStateMachine)
        {
            UpdateDetectEnemyTargets(banditStateMachine);

            if (_banditStateMachine != banditStateMachine) //RegisterHitByAttack()
                _banditStateMachine = banditStateMachine;
        }
        public void UpdateDetectEnemyTargets(IEnemyStateMachine banditStateMachine)
        {
            if (!_attackCollisionEnabled)
                return;

            DetermineTargetCollisionPurpose(banditStateMachine);

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
            {
                if (_collisionTargetPurpose == CollisionTargetPurpose.toAttack)
                {
                    if (banditStateMachine.GetEnemyState() == state.IsQuickAttack)
                        _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], _enemyLayerMasks[i]);
                    if (banditStateMachine.GetEnemyState() == state.IsAttackRelease)
                        _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], _enemyLayerMasks[i]);
                    if (banditStateMachine.GetEnemyState() == state.IsUpwardAttack)
                        _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], _enemyLayerMasks[i]);
                }

                if (_collisionTargetPurpose == CollisionTargetPurpose.toPersue)
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, 12.0f, LayerMask.GetMask(_playerLayerMaskStr));

                foreach (Collider2D enemy in _enemyTargetsHit)
                {
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player)))
                    {

                    }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player2))) { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player3))) { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player4))) { }

                    if (enemy.CompareTag(GetCompareTag(CompareTags.Enemy)))
                    {
                        //enemy.GetComponent<BanditBehaviours>()
                    }
                }

            }
            _attackCollisionEnabled = false;
        }
        private void DetermineTargetCollisionPurpose(IEnemyStateMachine banditStateMachine)
		{

            //For initial player detection purposes.
            if (banditStateMachine.GetEnemyState() == state.IsQuickAttack)
                _collisionTargetPurpose = CollisionTargetPurpose.toAttack;
            if (banditStateMachine.GetEnemyState() == state.IsAttackRelease)
                _collisionTargetPurpose = CollisionTargetPurpose.toAttack;
            if (banditStateMachine.GetEnemyState() == state.IsUpwardAttack)
                _collisionTargetPurpose = CollisionTargetPurpose.toAttack;

            if (banditStateMachine.GetEnemyState() == state.IsIdle)
                _collisionTargetPurpose = CollisionTargetPurpose.toPersue;
        }
        public void ProcessCollisionFlags(Collider2D collision)
        {
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
            //if (collision.CompareTag("")) { }
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            ProcessCollisionFlags(collision);
        }
        void OnTriggerStay2D(Collider2D collision) { }
        void OnTriggerExit2D(Collider2D collision) { }

        public void EnableAttackCollisions()
        {
            _attackCollisionEnabled = true;
        }

        public IEnemyStateMachine RegisterHitByAttack(IPlayerStateMachine playerStateMachine)
		{
            if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
                _banditStateMachine.SetEnemyState(state.IsHitByQuickAttack); 
            if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
                _banditStateMachine.SetEnemyState(state.IsHitByReleaseAttack);
            //if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
            //    _banditStateMachine.SetEnemyState(state.IsHitByQuickAttack);



            return _banditStateMachine;
        }
    }

}
