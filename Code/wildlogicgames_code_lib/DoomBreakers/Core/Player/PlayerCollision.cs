using System;
using UnityEngine;

namespace DoomBreakers
{
    public enum CompareTags //int id must align with SetupCompareTags() contents.
    {
        Player = 0,
        Player2 = 1,
        Player3 = 2,
        Player4 = 3,

        Enemy = 4,
        Item = 5,
        Container = 6,
        Health = 7
    };

    public class PlayerCollision : MonoBehaviour//, IPlayerCollision
    {
        private int _playerID;
        private CompareTags _compareTags;

        private Collider2D _collider2d, _equipCollider2d, _itemCollider2d;
        private Collider2D[] _enemyTargetsHit;

        private Transform[] _attackPoints;                              //1=quickATK, 2=powerATK, 3=upwardATK
        private float[] _attackRadius;// = new float[3];                //1=quickATK, 2=powerATK, 3=upwardATK
        private Vector3 _vector;                                        //We simply use this cached vector to flip attack points when needed. See FlipAttackPoints(int dir)

        private LayerMask[] _enemyLayerMasks;// = new LayerMask[2];
        private const string _playerLayerMaskStr = "Player";
        private const string _enemyLayerMaskStr = "Enemy";
        //private const string _propsLayerMaskStr = "Items";

        private string[] _compareTagStrings;// = new string[10];

        //private ITimer _cooldownTimer;
        private bool _attackCollisionEnabled;
        private bool _equipCollisionEnabled;
        private bool _equipPickupEnabled;
        private bool _equipPickupPossible;
        private bool _itemPickupEnabled;


        private IPlayerEquipment _playerEquipment;

        //public PlayerCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints) { }
        public void Setup(Collider2D collider2D, ref Transform[] arrayAtkPoints, int playerId)
		{
            _playerID = playerId;
            _collider2d = collider2D;
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            _collider2d.enabled = true;
            _attackCollisionEnabled = false;
            
            _equipCollisionEnabled = false;
            _equipPickupEnabled = false;
            _equipPickupPossible = false;

            _itemPickupEnabled = false;

        }
        public void SetupLayerMasks()
		{
            _enemyLayerMasks = new LayerMask[3];

            _enemyLayerMasks[0] = LayerMask.NameToLayer(_playerLayerMaskStr);
            _enemyLayerMasks[1] = LayerMask.NameToLayer(_enemyLayerMaskStr);
            //_enemyLayerMasks[2] = LayerMask.NameToLayer(_propsLayerMaskStr);
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

            _compareTagStrings[5] = "Item";
            _compareTagStrings[6] = "Container";
            _compareTagStrings[7] = "Health";
        }

        public string GetCompareTag(CompareTags compareTagId) => _compareTagStrings[(int)compareTagId];

        void Start() { }

        void Update() 
        { }
        public void UpdateCollision(ref BaseState playerState, int playerId, ref IPlayerEquipment playerEquipment, ref IPlayerSprite playerSprite, ref PlayerStats playerStat)
		{
            UpdateDetectEnemyTargets(ref playerState, playerId, ref playerSprite);
            ProcessEquipmentCollision();
            UpdateEquipmentTargets(ref playerEquipment);
            ProcessItemsCollision(ref playerStat);
        }
        public void UpdateDetectEnemyTargets(ref BaseState playerState, int playerId, ref IPlayerSprite playerSprite)
        {
            if (!_attackCollisionEnabled)
                return;

            for (int i = 0; i < _enemyLayerMasks.Length; i++)
			{
                
                DetermineCollisionPurpose(ref playerState, i);

                if (_enemyTargetsHit == null)
                    break;

                foreach (Collider2D enemy in _enemyTargetsHit)
				{
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player))) { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player2))) { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player3))) { }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Player4))) { }

                    if (enemy.CompareTag(GetCompareTag(CompareTags.Enemy))) 
                    {
                        if (enemy.GetComponent<Bandit>() == null) //Guard clause.
                            return;

                        int banditID = enemy.GetComponent<Bandit>()._banditID;
                        BattleColliderManager.AssignCollisionDetails("ReportCollisionWithBandit" + banditID.ToString(), 
                                                ref playerState, playerId, playerSprite, _playerEquipment.GetWeapon());
                    }
                    if (enemy.CompareTag(GetCompareTag(CompareTags.Container)))
					{
                        ProcessCollisionWithBarrel(enemy);
                    }
                }

            }
            _attackCollisionEnabled = false;
        }

        private void DetermineCollisionPurpose(ref BaseState playerState, int i)
        {

            if (playerState.GetType() == typeof(PlayerQuickAttack))
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_enemyLayerMaskStr));
                return;
			}
            if (playerState.GetType() == typeof(PlayerKnockAttack))
            {
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[0].position, _attackRadius[0], LayerMask.GetMask(_enemyLayerMaskStr));
                return;
            }
            if (playerState.GetType() == typeof(PlayerReleaseAttack))
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[1].position, _attackRadius[1], LayerMask.GetMask(_enemyLayerMaskStr));
                return;
            }
            if(playerState.GetType() == typeof(PlayerUpwardAttack))
			{
                _enemyTargetsHit = Physics2D.OverlapCircleAll(_attackPoints[2].position, _attackRadius[2], LayerMask.GetMask(_enemyLayerMaskStr));//_enemyLayerMasks[i]);
                return;
            }
        }
        

        public void UpdateEquipmentTargets(ref IPlayerEquipment playerEquipment)
        {
            if (_playerEquipment == null) _playerEquipment = playerEquipment; //For ProcessCollisionFlags() Item use.

            if (!_equipCollisionEnabled) return;

            playerEquipment = _playerEquipment; //Any changes made apply to original parent class, Player.cs.
            playerEquipment.NewEquipmentGained(true);

            _equipCollisionEnabled = false;
            SignalItemPickupCollision(false);
            EnableItemPickupCollision(false);
        }
        private void ProcessEquipmentCollision()
		{
            if (!_equipPickupPossible) return;
            if (!_equipPickupEnabled) return;

            ProcessItemCollisionFlags(_equipCollider2d);
        }
        public void ProcessItemCollisionFlags(Collider2D collision)
        {
            if (collision.CompareTag(GetCompareTag(CompareTags.Item)))
            {
                ProcessCollisionWithSword(collision);
                ProcessCollisionWithShield(collision);
                ProcessCollisionWithArmor(collision);
            }
        }
        private void ProcessCollisionWithSword(Collider2D collision)
		{
            if (collision.GetComponent<Sword>() == null)//Exists on Items Layer, Tag=Item
                return; //Then NOT a Sword. Get outta here!

            if(_playerEquipment.ApplySword(collision.GetComponent<Sword>()))
			{
                //UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.Sword,_playerID);
                collision.GetComponent<Sword>().Destroy();
                _equipCollisionEnabled = true; //Flag so we update players equipment.
            }
            return;
        }
        private void ProcessCollisionWithShield(Collider2D collision)
        {
            if (collision.GetComponent<Shield>() == null)
                return; //Then NOT a Shield. Get outta here!

            if(_playerEquipment.ApplyShield(collision.GetComponent<Shield>()))
			{
                //UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandShield, _playerID);
                collision.GetComponent<Shield>().Destroy();
                _equipCollisionEnabled = true;
            }
            return;
        }
        private void ProcessCollisionWithArmor(Collider2D collision)
        {
            if (collision.GetComponent<Breastplate>() == null)
                return; //Then NOT a Armor. Get outta here!

            if(_playerEquipment.ApplyArmor(collision.GetComponent<Breastplate>()))
			{
                //UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.Sword, _playerID);
                collision.GetComponent<Breastplate>().Destroy();
                _equipCollisionEnabled = true;
            }
            return;
        }

        private void ProcessItemsCollision(ref PlayerStats playerStat)
        {
            if (!_itemPickupEnabled) return;
            if (_itemCollider2d == null) return;
            if (_itemCollider2d.CompareTag(GetCompareTag(CompareTags.Health)))
            {
                ProcessCollisionWithApple(_itemCollider2d, ref playerStat);

            }
        }
        private void ProcessCollisionWithBarrel(Collider2D collision)
        {
            if (collision.GetComponent<Barrel>() == null) //Exists on Enemy Layer, Tag=Container
                return; //Then NOT a Barrel. Get outta here!

            collision.GetComponent<Barrel>().IsHit();


            return;
        }
        private void ProcessCollisionWithApple(Collider2D collision,ref PlayerStats playerStat)
        {
            if (collision.GetComponent<Apple>() == null) //Exists on Items Layer, Tag=Health
                return; //Then NOT a Apple. Get outta here!


            double healPoints = collision.GetComponent<Apple>().Health();
            playerStat.Health += healPoints;
            UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStat, _playerID);

            collision.GetComponent<Apple>().Destroy();
            _itemPickupEnabled = false;
            _itemCollider2d = null;
            return;
        }


        public void OnTriggerEnter2D(Collider2D collision)
		{
            if (collision.CompareTag(GetCompareTag(CompareTags.Item)))
            {
                _equipCollider2d = collision;
                SignalItemPickupCollision(true);
            }
            if (collision.CompareTag(GetCompareTag(CompareTags.Health)))
            {
                _itemCollider2d = collision;
                _itemPickupEnabled = true;
            }
        }
        //void OnTriggerStay2D(Collider2D collision) => ProcessCollisionFlags(collision); //unreliable. We use ProcessItemCollision() instead.
        void OnTriggerExit2D(Collider2D collision)
		{
            if (collision.CompareTag(GetCompareTag(CompareTags.Item)))
            {
                _equipCollider2d = null;
                SignalItemPickupCollision(false);
            }
            if (collision.CompareTag(GetCompareTag(CompareTags.Health))) _itemCollider2d = null;
        }

        public void EnableAttackCollisions() =>  _attackCollisionEnabled = true;


        public void EnableItemPickupCollision() => _equipPickupEnabled = true;
        private void EnableItemPickupCollision(bool b) => _equipPickupEnabled = b;
        
        public bool SignalItemPickupCollision() => _equipPickupPossible;
        private bool SignalItemPickupCollision(bool b) => _equipPickupPossible = b;

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
