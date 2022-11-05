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
        public void UpdateCollision(IPlayerStateMachine playerStateMachine)
		{
            UpdateDetectEnemyTargets(playerStateMachine);

        }
        public void UpdateDetectEnemyTargets(IPlayerStateMachine playerStateMachine)
        {
            if (!_attackCollisionEnabled)
                return;

            //Collider2D[] enemyTargets = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], _enemyLayerMasks[0]);

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
			{
                
                if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_enemyLayerMaskStr));
                //if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
                //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], _enemyLayerMasks[i]);
                //if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
                //    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], _enemyLayerMasks[i]);

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
                        //EventManager.TriggerEvent("BanditHitByPlayer");
                        //_attackCollisionEnabled = false;
                        //return;
                        if (enemy.GetComponent<Bandit>() != null) //Guard clause.
                            enemy.GetComponent<Bandit>().ReportCollisionWithPlayer(playerStateMachine);//RegisterHitByAttack(playerStateMachine);
                    }
                }

            }
            _attackCollisionEnabled = false;
        }
        public IPlayerStateMachine RegisterHitByAttack(IEnemyStateMachine enemyStateMachine, IPlayerStateMachine playerStateMachine)
		{
            if (enemyStateMachine.GetEnemyState() == state.IsQuickAttack)
                playerStateMachine.SetPlayerState(state.IsHitByQuickAttack);
            if (enemyStateMachine.GetEnemyState() == state.IsAttackRelease)
                playerStateMachine.SetPlayerState(state.IsHitByReleaseAttack);



            _playerStateMachine = playerStateMachine;
            return playerStateMachine;
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
        //public bool IsAttackCollisionsEnabled()
		//{
        //    return _attackCollisionEnabled;
		//}
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
