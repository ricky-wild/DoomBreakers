using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class ArcherCollision : MonoBehaviour
	{
        private int _enemyID;
        public enum CollisionTargetPurpose
        {
            noPurpose = 0,
            toPersue = 1,
            toAttack = 2,
            toShoot = 3
        };
        private CollisionTargetPurpose _collisionTargetPurpose; //Flag for player collision purpose. UpdateDetectEnemyTargets()

        private CompareTags _compareTags;

        private Collider2D _collider2d;
        private Collider2D[] _enemyTargetsHit;


        private Transform[] _attackPoints;                           
        private Vector3 _vector;                                    //We simply use this cached vector to flip attack points when needed. See FlipAttackPoints(int dir)

        private LayerMask[] _enemyLayerMasks = new LayerMask[1];
        private const string _playerLayerMaskStr = "Player";
        //private const string _enemyLayerMaskStr = "Enemy";

        private string[] _compareTagStrings = new string[10];

        private bool _detectTargetCollisionEnabled, _disableShootFlag;

        private BanditStats _banditStats;

        //private ITimer _timer;
        //private const float _waitTime = 3.0f;

        public ArcherCollision(Collider2D collider2D, ref Transform shootDetectionPoint,
            ref BanditStats banditStats, int enemyId)
        {
            //Bandit Archer
            _attackPoints = new Transform[1];
            _attackPoints[0] = shootDetectionPoint;
            Setup(collider2D, ref banditStats, enemyId);
        }

        private void Setup(Collider2D collider2D, ref BanditStats banditStats, int enemyId)
        {
            SetupClassVars(collider2D, ref banditStats, enemyId);
            SetupLayerMasks();
            SetupCompareTags();
        }
        private void SetupClassVars(Collider2D collider2D, ref BanditStats banditStats, int enemyId)
        {
            _enemyID = enemyId;
            _collider2d = collider2D;
            _banditStats = banditStats;
            _collider2d.enabled = true;
            _detectTargetCollisionEnabled = false;
            _disableShootFlag = false;
            _collisionTargetPurpose = CollisionTargetPurpose.noPurpose;
            //_timer = new Timer();
            //_timer.StartTimer(_waitTime);
        }
        public void SetupLayerMasks()
        {
            _enemyLayerMasks[0] = LayerMask.NameToLayer(_playerLayerMaskStr);
            //_enemyLayerMasks[1] = LayerMask.NameToLayer(_enemyLayerMaskStr);
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

        public void UpdateCollision(ref BasicEnemyBaseState banditState, IBanditSprite banditSprite, ref BanditStats banditStats)
        {
            UpdateDetectEnemyTargets(ref banditState, banditSprite, ref banditStats);

			//if (_timer.HasTimerFinished())
			//{
			//	_timer.Reset();
			//	_timer.StartTimer(_waitTime);
			//	if (!_disableShootFlag) _disableShootFlag = true;

			//}
		}
        public void UpdateDetectEnemyTargets(ref BasicEnemyBaseState banditState, IBanditSprite banditSprite, ref BanditStats banditStats)
        {
            //if (_disableShootFlag) return;
            if (!_detectTargetCollisionEnabled)
                return;

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
            {
                DetermineCollisionPurpose(ref banditState, i);
				if (_enemyTargetsHit == null) return;

				foreach (Collider2D enemy in _enemyTargetsHit)
                {

					if (enemy.CompareTag(GetCompareTag(CompareTags.Player)))
                    {
                        if (_collisionTargetPurpose == CollisionTargetPurpose.toShoot)
                        {
                            AITargetTrackingManager.AssignTargetTransform("ReportDetectionWithPlayerForBanditArcher" + _enemyID.ToString(), enemy.transform, _enemyID, EnemyAI.BanditArcher);
                            _detectTargetCollisionEnabled = false;
                            _disableShootFlag = false;
                            return;
                        }
                    }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player2)))
                    { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player3)))
                    { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player4)))
                    { }
                    if (enemy.CompareTag("FallenFlag"))
                        _banditStats.Health -= _banditStats.Health;


                    //if (enemy.CompareTag(GetCompareTag(CompareTags.Enemy))){}
                }

            }

            _detectTargetCollisionEnabled = false;
        }

        private void DetermineCollisionPurpose(ref BasicEnemyBaseState banditState, int i)
        {
            if (banditState.GetType() == typeof(BanditArcherAim))
            {
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, 12.0f, LayerMask.GetMask(_playerLayerMaskStr));
                _collisionTargetPurpose = CollisionTargetPurpose.toShoot;
                return;
            }

        }

        public void ProcessCollisionFlags(Collider2D collision)
        {
            if (collision.CompareTag("FallenFlag"))
                _banditStats.Health -= _banditStats.Health;
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
            if (!_detectTargetCollisionEnabled) _detectTargetCollisionEnabled = true;
        }
        public bool IsDisableShootFlag() => _disableShootFlag;
        public void IsDisableShootFlag(bool b) => _disableShootFlag = b;


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
