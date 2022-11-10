using UnityEngine;

namespace DoomBreakers
{
    public enum CompareTags //int id must align with SetupCompareTags() contents.
    {
        Player = 0,
        Player2 = 1,
        Player3 = 2,
        Player4 = 3,

        Enemy = 4
    }
    public class PlayerCollision : MonoBehaviour, IPlayerCollision
    {
        private ICollisionData _collisionData;
        private CompareTags _compareTags;

        private Collider2D _collider2d;
        private Collider2D[] _enemyTargetsHit;

        private Transform[] _attackPoints;                              //1=quickATK, 2=powerATK, 3=upwardATK
        private float[] _attackRadius;// = new float[3];                //1=quickATK, 2=powerATK, 3=upwardATK
        private Vector3 _vector;                                        //We simply use this cached vector to flip attack points when needed. See FlipAttackPoints(int dir)

        private LayerMask[] _enemyLayerMasks;// = new LayerMask[2];
        private const string _playerLayerMaskStr = "Player";
        private const string _enemyLayerMaskStr = "Enemy";

        private string[] _compareTagStrings;// = new string[10];

        //private ITimer _cooldownTimer;
        private bool _attackCollisionEnabled;

        private IPlayerStateMachine _playerStateMachine;

        public PlayerCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints)
        {
            _collider2d = collider2D;
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            //_cooldownTimer = this.gameObject.AddComponent<Timer>();
            //_cooldownTimer.Setup();

            _collisionData = new CollisionData();
            _collider2d.enabled = true;
            _attackCollisionEnabled = false;
        }
        public void SetupLayerMasks()
		{
            _enemyLayerMasks = new LayerMask[2];

            _enemyLayerMasks[0] = LayerMask.NameToLayer(_playerLayerMaskStr);
            _enemyLayerMasks[1] = LayerMask.NameToLayer(_enemyLayerMaskStr);
            //_enemyLayerMasks[0] = LayerMask.GetMask(_playerLayerMaskStr);
            //_enemyLayerMasks[1] = LayerMask.GetMask(_enemyLayerMaskStr);
        }
        public void SetupAttackRadius()
		{
            _attackRadius = new float[3];
            _attackRadius[0] = 1.2f; //quick attack radius
            _attackRadius[1] = 1.6f; //power attack radius
            _attackRadius[2] = 1.3f; //upward attack radius

        }
        public void SetupCompareTags()
		{
            _compareTagStrings = new string[10];

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

        void Update() 
        { }
        public void UpdateCollision(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, int playerId)
		{
            UpdateDetectEnemyTargets(playerStateMachine, playerSprite, playerId);

        }
        public void UpdateDetectEnemyTargets(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, int playerId)
        {
            if (!_attackCollisionEnabled)
                return;

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
			{
                
                //if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
                //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_enemyLayerMaskStr));
                //if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
                //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], LayerMask.GetMask(_enemyLayerMaskStr));//_enemyLayerMasks[i]);
                //if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
                //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], _enemyLayerMasks[i]);

                DetermineCollisionPurpose(playerStateMachine, i);

                if (_enemyTargetsHit == null)
                    break;

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
                        ProcessCollisionWithBandit(playerStateMachine, playerSprite, enemy, playerId);//, 0);
                        //EventManager.TriggerEvent("BanditHitByPlayer");
                        //_attackCollisionEnabled = false;
                        //return;
                        //if (enemy.GetComponent<Bandit>() != null) //Guard clause.
                        //{
                        //    _collisionData.PluginPlayerState(playerStateMachine);
                        //    enemy.GetComponent<Bandit>().ReportCollisionWithPlayer(_collisionData);
                        //Think of GetComponent as a check for interface implementation.
                        //Unity is a component based architecture / OOP hybrid and using GetComponent is unavoidable.
                        //}
                    }
                }

            }
            _attackCollisionEnabled = false;
        }
        private void ProcessCollisionWithBandit(IPlayerStateMachine playerStateMachine, IPlayerSprite playerSprite, Collider2D enemy, int playerId)//, int enemyId)
        {
            if (enemy.GetComponent<Bandit>() == null) //Guard clause.
                return;

            _collisionData.PluginPlayerState(playerStateMachine, playerId);
            _collisionData.PluginPlayerSprite(playerSprite, playerId);
            enemy.GetComponent<Bandit>().ReportCollisionWithPlayer(_collisionData, playerId);//RegisterHitByAttack();

        }
        private void DetermineCollisionPurpose(IPlayerStateMachine playerStateMachine, int i)
        {
            if(playerStateMachine.IsQuickAttack())
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_enemyLayerMaskStr));
                return;
			}
            if(playerStateMachine.IsPowerAttackRelease())
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], LayerMask.GetMask(_enemyLayerMaskStr));
                return;
            }
            if(playerStateMachine.IsUpwardAttack())
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], LayerMask.GetMask(_enemyLayerMaskStr));//_enemyLayerMasks[i]);
                return;
            }
        }
        public IPlayerStateMachine RegisterHitByAttack(ICollisionData collisionData, int playerId, int banditId)
        {
            if(IsIgnoreDamage(collisionData.GetCachedPlayerState(playerId)))
                return collisionData.GetCachedPlayerState(playerId);

            if (IsDefendingSelf(collisionData.GetCachedPlayerState(playerId)))
			{
                if (collisionData.GetCachedEnemyState(banditId).IsQuickAttack())
				{

                    if (IsDefendingCorrectDirection(collisionData.GetCachedPlayerSprite(playerId), collisionData.GetCachedBanditSprite(banditId)))
                        collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsQuickHitWhileDefending);
                    else
                        collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsHitByQuickAttack); //GREAT SUCCESS!

                }
                if (collisionData.GetCachedEnemyState(banditId).IsPowerAttackRelease())
                {
                    if (IsDefendingCorrectDirection(collisionData.GetCachedPlayerSprite(playerId), collisionData.GetCachedBanditSprite(banditId)))
                        collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsHitWhileDefending);
                    else
                        collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsHitByReleaseAttack);
                }
            }
            if (!IsDefendingSelf(collisionData.GetCachedPlayerState(playerId)))
			{

                if (collisionData.GetCachedEnemyState(banditId).IsQuickAttack())
                    collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsHitByQuickAttack);
                if (collisionData.GetCachedEnemyState(banditId).IsPowerAttackRelease())
                    collisionData.GetCachedPlayerState(playerId).SetPlayerState(state.IsHitByReleaseAttack);
            }



            _playerStateMachine = collisionData.GetCachedPlayerState(playerId);
            return collisionData.GetCachedPlayerState(playerId);
        }
        private bool IsDefendingCorrectDirection(IPlayerSprite playerSprite, IBanditSprite banditSprite)
		{
            //Detrmine which way the player is facing whilst defending & the enemy bandit is attacking.
            //Why? Player doesn't successfully defend against enemy attack defending the wrong face direction.

            int playerFaceDir = playerSprite.GetSpriteDirection();
            int enemyFaceDir = banditSprite.GetSpriteDirection();

            //Enemy would only ever be attacking if directly in front of player.
            //So if player face direction is 1 (right) then enemy would have to be -1 (left) 
            //case sceneria true for successful defence.
            if (playerFaceDir == 1 && enemyFaceDir == -1 ||
                playerFaceDir == -1 && enemyFaceDir == 1)
                return true;

            return false;
		}
        private bool IsDefendingSelf(IPlayerStateMachine playerStateMachine)
		{
            if (playerStateMachine.IsDefendingPrepare())
                return true;
            if (playerStateMachine.IsDefendingMoving())
                return true;
            if (playerStateMachine.IsQuickHitWhenDefending())
                return true;
            if (playerStateMachine.IsPowerHitWhenDefending())
                return true;

            return false;
		}
        private bool IsIgnoreDamage(IPlayerStateMachine playerStateMachine)
		{
            if (playerStateMachine.IsDodgeLeftPrepare())
                return true;
            if (playerStateMachine.IsDodgeRightPrepare())
                return true;
            if (playerStateMachine.IsDodgeRelease())
                return true;

            return false;
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
        public void FlipAttackPoints(int dir)
		{
            //Circles we draw(in editor) & detect enemies against. These must all be flipped 
            //as this method will be called on player face direction change.
            if (dir == 1)//Facing Right
			{
                for(int i = 0; i < _attackPoints.Length; i++)
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
