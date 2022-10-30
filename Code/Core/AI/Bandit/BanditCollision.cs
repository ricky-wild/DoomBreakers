using UnityEngine;

namespace DoomBreakers
{

    public class BanditCollision : MonoBehaviour, IBanditCollision
    {

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

        private IEnemyStateMachine _banditStateRef;

        public BanditCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints)
        {
            _collider2d = collider2D;
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            _collider2d.enabled = true;
            _attackCollisionEnabled = false;
        }
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

            if (_banditStateRef != banditStateMachine) //RegisterHitByAttack()
                _banditStateRef = banditStateMachine;
        }
        public void UpdateDetectEnemyTargets(IEnemyStateMachine banditStateMachine)
        {
            if (!_attackCollisionEnabled)
                return;

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
            {
                if (banditStateMachine.GetEnemyState() == state.IsQuickAttack)
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], _enemyLayerMasks[i]);
                if (banditStateMachine.GetEnemyState() == state.IsAttackRelease)
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], _enemyLayerMasks[i]);
                if (banditStateMachine.GetEnemyState() == state.IsUpwardAttack)
                    _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], _enemyLayerMasks[i]);

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

        public void RegisterHitByAttack(IPlayerStateMachine playerStateMachine)
		{
            if (playerStateMachine.GetPlayerState() == state.IsQuickAttack)
                _banditStateRef.SetEnemyState(state.IsHitByQuickAttack); 
            if (playerStateMachine.GetPlayerState() == state.IsAttackRelease)
                _banditStateRef.SetEnemyState(state.IsHitByReleaseAttack);
            //if (playerStateMachine.GetPlayerState() == state.IsUpwardAttack)
            //    _banditStateRef.SetEnemyState(state.IsHitByQuickAttack);


        }
    }

}
