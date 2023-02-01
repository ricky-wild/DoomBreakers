using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	public class NPCCollision : MonoBehaviour
	{
        private int _npcID;
        public enum CollisionTargetPurpose
        {
            noPurpose = 0,
            toFollow = 1,
            toJump = 2,
            toRest = 3,
            toFlee = 4
        };
        private CollisionTargetPurpose _collisionTargetPurpose; //Flag for player collision purpose. UpdateDetectEnemyTargets()

        private CompareTags _compareTags;

        private Collider2D _collider2d;
        private Collider2D[] _enemyTargetsHit;

        private Transform _detectPoint;                              //1=toFollow, 2=toJump, 3=toRest, 4=toFlee
        private float[] _detectRadius = new float[4];                //1=toFollow, 2=toJump, 3=toRest, 4=toFlee
        private Vector3 _vector;                                    //We simply use this cached vector to flip attack points when needed. See FlipAttackPoints(int dir)

        private LayerMask[] _enemyLayerMasks = new LayerMask[4];
        private const string _playerLayerMaskStr = "Player";
        private const string _enemyLayerMaskStr = "Enemy";
        private const string _platformLayerMaskStr = "Obstacle";
        private const string _defaultLayerMaskStr = "Default";

        private string[] _compareTagStrings = new string[12];

        private ITimer _detectTimer;
        private int _detectionTypeFlag;
        private bool _detectTargetCollisionEnabled;

        private NPCStats _npcStats;

        public NPCCollision(Collider2D collider2D, ref Transform detectPoint,
                                ref NPCStats npcStats, int npcId)// => Setup(collider2D, ref arrayAtkPoints, ref banditStats, enemyId);
        {
            _detectPoint = detectPoint;
            Setup(collider2D, ref npcStats, npcId);
        }
        private void Setup(Collider2D collider2D, ref NPCStats npcStats, int npcId)
        {
            SetupClassVars(collider2D, ref npcStats, npcId);
            SetupLayerMasks();
            SetupDetectRadius();
            SetupCompareTags();
        }
        private void SetupClassVars(Collider2D collider2D, ref NPCStats npcStats, int npcId)
        {
            _npcID = npcId;
            _collider2d = collider2D;
            _npcStats = npcStats;
            _collider2d.enabled = true;
            _detectTargetCollisionEnabled = false;
            _collisionTargetPurpose = CollisionTargetPurpose.noPurpose;
            _detectionTypeFlag = 0;
            _detectTimer = new Timer();
        }
        public void SetupLayerMasks()
        {
            _enemyLayerMasks[0] = LayerMask.NameToLayer(_playerLayerMaskStr);
            _enemyLayerMasks[1] = LayerMask.NameToLayer(_enemyLayerMaskStr);
            _enemyLayerMasks[2] = LayerMask.NameToLayer(_platformLayerMaskStr);
            _enemyLayerMasks[3] = LayerMask.NameToLayer(_defaultLayerMaskStr);
        }
        public void SetupDetectRadius()
        {
            //1=toFollow, 2=toJump, 3=toRest, 4=toFlee
            _detectRadius[0] = 3.3f; //toFollow
            _detectRadius[1] = 0.95f; //toJump
            _detectRadius[2] = 2.5f; //toRest
            _detectRadius[3] = 5.5f;//6.6f; //toFlee
        }
        public void SetupCompareTags()
        {
            _compareTagStrings[0] = "Player";
            _compareTagStrings[1] = "Player2";
            _compareTagStrings[2] = "Player3";
            _compareTagStrings[3] = "Player4";

            _compareTagStrings[4] = "Enemy";
            _compareTagStrings[10] = "Platform";
            _compareTagStrings[11] = "CampsiteFlag";
        }
        public string GetCompareTag(CompareTags compareTagId)
        {
            return _compareTagStrings[(int)compareTagId];

        }
        public float GetDetectRadius(int i) => _detectRadius[i];
        void Start() { }
        void Update() { }
        private void UpdateDetectionGateway()
		{
            if(_detectTimer.HasTimerFinished())
			{
                _detectTimer.Reset();
                _detectTimer.StartTimer(2.0f);
                _detectTargetCollisionEnabled = true;
            }
		}
        public void UpdateCollision(ref BasicNPCBaseState npcState, NPCSprite npcSprite, ref NPCStats npcStats)
        {
            UpdateDetectionGateway();
            UpdateTargetDetection(ref npcState, npcSprite, ref npcStats);
        }
        public void UpdateTargetDetection(ref BasicNPCBaseState npcState, NPCSprite npcSprite, ref NPCStats npcStats)
        {
            if (!_detectTargetCollisionEnabled) return;

            

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
            {
                //AIEventNPCManager.Subscribe("ReportFollowDetection" + _npcID.ToString(), _actionListener[0]);
                //AIEventNPCManager.Subscribe("ReportJumpDetection" + _npcID.ToString(), _actionListener[1]);
                //AIEventNPCManager.Subscribe("ReportRestDetection" + _npcID.ToString(), _actionListener[2]);
                //AIEventNPCManager.Subscribe("ReportFleeDetection" + _npcID.ToString(), _actionListener[3]);
                DetermineCollisionPurpose(ref npcState, i);
                if (_enemyTargetsHit == null) return;
                foreach (Collider2D enemy in _enemyTargetsHit)
                {
                    switch(_collisionTargetPurpose)
					{
                        case CollisionTargetPurpose.toFollow:
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Player)))
                            {
                                int playerID = enemy.GetComponent<Player>()._playerID;
                                //AIEventNPCManager.TriggerEvent("ReportFollowDetection" + _npcID.ToString());
                            }
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Player2)))
                            { }
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Player3)))
                            { }
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Player4)))
                            { }
                            break;
                        case CollisionTargetPurpose.toJump:
                            if (npcState.GetType() == typeof(NPCJump)) return;
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Platform)))
                            {
                                AIEventNPCManager.TriggerEvent("ReportJumpDetection" + _npcID.ToString());
                            }
                            break;
                        case CollisionTargetPurpose.toRest:
                            if (npcState.GetType() == typeof(NPCWaiting)) return;
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Rest)))
                            {
                                AIEventNPCManager.TriggerEvent("ReportRestDetection" + _npcID.ToString());
                            }
                            break;
                        case CollisionTargetPurpose.toFlee:

                            if (npcState.GetType() == typeof(NPCFlee)) return;
                            
                            if (enemy.CompareTag(GetCompareTag(CompareTags.Enemy)))
                            {
                                if (enemy.GetComponent<Bandit>() != null)
                                {
                                    if (enemy.GetComponent<Bandit>()._healthTransform[1].localScale.x == 0) //return;
                                    {
                                        break;
                                    }
                                }
                                AIEventNPCManager.TriggerEvent("ReportFleeDetection" + _npcID.ToString());
                            }
                            break;
                    }
				}

            }

            if(_detectionTypeFlag == 3)
			{
                _detectionTypeFlag = 0;
                _detectTargetCollisionEnabled = false;
            }
            if (_detectionTypeFlag < 3) _detectionTypeFlag++;
        }
        private void DetermineCollisionPurpose(ref BasicNPCBaseState npcState, int i)
        {
            //0=toFollow, 1=toJump, 2=toRest, 3=toFlee

            switch (_detectionTypeFlag)
			{
                case 0://toFollow
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_detectPoint.position, GetDetectRadius(0), LayerMask.GetMask(_playerLayerMaskStr));
                    _collisionTargetPurpose = CollisionTargetPurpose.toFollow;
                    break;
                case 1://toJump
                    //Vector3 v = _detectPoint.position;
                    //v.y -= 1.0f;
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_detectPoint.position, GetDetectRadius(1), LayerMask.GetMask(_platformLayerMaskStr));
                    _collisionTargetPurpose = CollisionTargetPurpose.toJump;
                    break;
                case 2://toRest
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_detectPoint.position, GetDetectRadius(2), LayerMask.GetMask(_defaultLayerMaskStr));
                    _collisionTargetPurpose = CollisionTargetPurpose.toRest;
                    break;
                case 3://toFlee
                    
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_detectPoint.position, GetDetectRadius(3), LayerMask.GetMask(_enemyLayerMaskStr));
                    _collisionTargetPurpose = CollisionTargetPurpose.toFlee;
                    break;
            }
        }
    }
}
