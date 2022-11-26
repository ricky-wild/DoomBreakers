using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{

    public class BanditCollision : MonoBehaviour, IBanditCollision
    {
        private int _banditID;
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
        //private Transform _collidedTargetTransform;

        private Transform[] _attackPoints;                            //1=quickATK, 2=powerATK, 3=upwardATK
        private float[] _attackRadius = new float[3];                //1=quickATK, 2=powerATK, 3=upwardATK
        private Vector3 _vector;                                    //We simply use this cached vector to flip attack points when needed. See FlipAttackPoints(int dir)

        private LayerMask[] _enemyLayerMasks = new LayerMask[2];
        private const string _playerLayerMaskStr = "Player";
        private const string _enemyLayerMaskStr = "Enemy";

        private string[] _compareTagStrings = new string[10];

        private bool _detectTargetCollisionEnabled;

        //private Action _eventListener;
        private IEnemyStateMachine _banditStateMachine;

        public BanditCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints, int banditId)
        {
            _banditID = banditId;
            _collider2d = collider2D;
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            _collider2d.enabled = true;
            _detectTargetCollisionEnabled = false;
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

        public void UpdateCollision(ref BanditBaseState banditState, IBanditSprite banditSprite)
        {
            UpdateDetectEnemyTargets(ref banditState, banditSprite);

        }
        public void UpdateDetectEnemyTargets(ref BanditBaseState banditState, IBanditSprite banditSprite)
        {
            if (!_detectTargetCollisionEnabled)
                return;

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
            {
                DetermineCollisionPurpose(ref banditState, i);
                foreach (Collider2D enemy in _enemyTargetsHit)
                {
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player)))
                    {
                        if (_collisionTargetPurpose == CollisionTargetPurpose.toPersue)
                        {
                            AITargetTrackingManager.AssignTargetTransform(enemy.transform, _banditID, EnemyAI.Bandit);
                            AITargetTrackingManager.TriggerEvent("ReportDetectionWithPlayer");
                        }
                        if (_collisionTargetPurpose == CollisionTargetPurpose.toAttack)
						{
                            BattleColliderManager.TriggerEvent("ReportCollisionWithPlayer");
                        }           
                    }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player2)))
                    { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player3)))
                    { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player4)))
                    { }

                    //if (enemy.CompareTag(GetCompareTag(CompareTags.Enemy))){}
                }

            }

            _detectTargetCollisionEnabled = false;
        }

        private void DetermineCollisionPurpose(ref BanditBaseState banditState, int i)
        {


            if (banditState.GetType() == typeof(BanditIdle))
            {
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, 12.0f, LayerMask.GetMask(_playerLayerMaskStr));
                //AITargetTrackingManager.TriggerEvent("ReportDetectionWithPlayer");//Through this we can set the state machine as appropriate.
                _collisionTargetPurpose = CollisionTargetPurpose.toPersue;
                return;
            }
            if (banditState.GetType() == typeof(BanditQuickAttack))
            {
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_playerLayerMaskStr));
                _collisionTargetPurpose = CollisionTargetPurpose.toAttack;
                
                return;
            }
            //if (banditStateMachine.IsPowerAttackRelease())
            //{
            //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], _enemyLayerMasks[i]);
            //    _collisionTargetPurpose = CollisionTargetPurpose.toAttack;
            //    return;
            //}
            //if (banditStateMachine.IsUpwardAttack())
            //{
            //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], _enemyLayerMasks[i]);
            //    _collisionTargetPurpose = CollisionTargetPurpose.toAttack;
            //    return;
            //}
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

        public void EnableTargetCollisionDetection()
        {
            _detectTargetCollisionEnabled = true;
        }


        public void FlipAttackPoints(int dir)
        {
            //Circles we draw(in editor) & detect enemies against. These must all be flipped 
            //as this method will be called on enemy face direction change.
            if (dir == 1)//Facing Right
            {
                for (int i = 0; i < _attackPoints.Length; i++)
                {
                    _vector = _attackPoints[i].localPosition;
                    _vector.x = Mathf.Abs(_vector.x);
                    _attackPoints[i].localPosition = _vector;
                }


                return;
            }
            if (dir == -1)//Facing Left
            {
                for (int i = 0; i < _attackPoints.Length; i++)
                {
                    _vector = _attackPoints[i].localPosition;
                    _vector.x = -Mathf.Abs(_vector.x);
                    _attackPoints[i].localPosition = _vector;
                }


                return;
            }
        }

    }

}
