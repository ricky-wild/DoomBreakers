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
        Health = 7,
        Currency = 8
    };

    public enum C2D
	{
        PlayerCollider2D = 0,
        EquipmentCollider2D = 1,
        HealthCollider2D = 2,
        CurrencyCollider2D = 3
	};//_colliderIdentities2D[]; //_collider2d, _equipCollider2d, _healthCollider2d;

    public class PlayerCollision : MonoBehaviour//, IPlayerCollision
    {
        private int _playerID;
        private CompareTags _compareTags;

        private Collider2D[] _colliderIdentities2D = new Collider2D[4];
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
        private bool _healthPickupEnabled;
        private bool _currencyPickupEnabled;


        private IPlayerEquipment _playerEquipment;

        //public PlayerCollision(Collider2D collider2D, ref Transform[] arrayAtkPoints) { }
        public void Setup(Collider2D collider2D, ref Transform[] arrayAtkPoints, int playerId)
		{
            _playerID = playerId;
            SetupCollider2D(collider2D);
            _attackPoints = arrayAtkPoints;

            SetupLayerMasks();
            SetupAttackRadius();
            SetupCompareTags();

            //_collider2d.enabled = true;
            _attackCollisionEnabled = false;
            
            _equipCollisionEnabled = false;
            _equipPickupEnabled = false;
            _equipPickupPossible = false;

            _healthPickupEnabled = false;
            _currencyPickupEnabled = false;

        }
        private void SetupCollider2D(Collider2D collider2D)
		{
            SetCollider2DIdentity(collider2D, C2D.PlayerCollider2D); //_collider2d = collider2D;
            _colliderIdentities2D[1] = null; //C2D.EquipmentCollider2D
            _colliderIdentities2D[2] = null; //C2D.HealthCollider2D
            _colliderIdentities2D[3] = null; //C2D.CurrencyCollider2D
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
            _compareTagStrings[8] = "Currency";//oops
        }

        public string GetCompareTag(CompareTags compareTagId) => _compareTagStrings[(int)compareTagId];
        private Collider2D GetCollider2DIdentity(C2D colliderIdentity) => _colliderIdentities2D[(int)colliderIdentity];
        private void SetCollider2DIdentity(Collider2D collider2D, C2D colliderIdentity) => _colliderIdentities2D[(int)colliderIdentity] = collider2D;

        void Start() { }

        void Update() 
        { }
        public void UpdateCollision(ref BaseState playerState, int playerId, ref IPlayerEquipment playerEquipment, ref IPlayerSprite playerSprite, ref PlayerStats playerStat)
		{
            UpdateDetectEnemyTargets(ref playerState, playerId, ref playerSprite);
            ProcessEquipmentCollisions();
            UpdateEquipmentTargets(ref playerEquipment);
            ProcessHealthCollisions(ref playerStat, ref playerSprite);
            ProcessCurrencyCollisions(ref playerStat);
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
            UIPlayerManager.SetPlayerEquipment(ref playerEquipment, _playerID);

            _equipCollisionEnabled = false;
            SignalItemPickupCollision(false);
            EnableItemPickupCollision(false);
        }
        private void ProcessEquipmentCollisions()
		{
            if (!_equipPickupPossible) return;
            if (!_equipPickupEnabled) return;

            ProcessItemCollisionFlags(GetCollider2DIdentity(C2D.EquipmentCollider2D));//_equipCollider2d
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

        private void ProcessHealthCollisions(ref PlayerStats playerStat, ref IPlayerSprite playerSprite)
        {
            if (!_healthPickupEnabled) return;
            if (GetCollider2DIdentity(C2D.HealthCollider2D) == null) return;//_healthCollider2d
            if (GetCollider2DIdentity(C2D.HealthCollider2D).CompareTag(GetCompareTag(CompareTags.Health)))
            {
                ProcessCollisionWithApple(GetCollider2DIdentity(C2D.HealthCollider2D), ref playerStat, ref playerSprite);
                ProcessCollisionWithChicken(GetCollider2DIdentity(C2D.HealthCollider2D), ref playerStat, ref playerSprite);
                ProcessCollisionWithFish(GetCollider2DIdentity(C2D.HealthCollider2D), ref playerStat, ref playerSprite);
                SetCollider2DIdentity(null, C2D.HealthCollider2D);//_healthCollider2d = null;
            }
		}
        private void ProcessCollisionWithApple(Collider2D collision,ref PlayerStats playerStat, ref IPlayerSprite playerSprite)
        {
            if (collision == null) return;
            if (collision.GetComponent<Apple>() == null) //Exists on Items Layer, Tag=Health
                return; //Then NOT a Apple. Get outta here!

            if (playerStat.Health != playerStat.GetMaxHealthLimit())
			{
                double healPoints = collision.GetComponent<Apple>().Health();
                playerStat.Health += healPoints;
                playerStat.SetRecentHealItemType(HealingItemType.Apple);
                playerSprite.SetBehaviourTextureFlash(0.5f, Color.green);
                UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStat, _playerID);
                collision.GetComponent<Apple>().Destroy();
                AudioEventManager.PlayPropSFX(PropSFXID.PropHealUpSFX);
            }

            _healthPickupEnabled = false;
            
            return;
        }
        private void ProcessCollisionWithChicken(Collider2D collision, ref PlayerStats playerStat, ref IPlayerSprite playerSprite)
        {
            if (collision == null) return;
            if (collision.GetComponent<Chicken>() == null) //Exists on Items Layer, Tag=Health
                return; //Then NOT a Chicken. Get outta here!

            if (playerStat.Health != playerStat.GetMaxHealthLimit())
            {
                double healPoints = collision.GetComponent<Chicken>().Health();
                playerStat.Health += healPoints;
                playerStat.SetRecentHealItemType(HealingItemType.Chicken);
                playerSprite.SetBehaviourTextureFlash(0.5f, Color.green);
                UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStat, _playerID);
                collision.GetComponent<Chicken>().Destroy();
                AudioEventManager.PlayPropSFX(PropSFXID.PropHealUpSFX);
            }

            _healthPickupEnabled = false;

            return;
        }
        private void ProcessCollisionWithFish(Collider2D collision, ref PlayerStats playerStat, ref IPlayerSprite playerSprite)
        {
            if (collision == null) return;
            if (collision.GetComponent<Fish>() == null) //Exists on Items Layer, Tag=Health
                return; //Then NOT a Fish. Get outta here!

            if (playerStat.Health != playerStat.GetMaxHealthLimit())
            {
                double healPoints = collision.GetComponent<Fish>().Health();
                playerStat.Health += healPoints;
                playerStat.SetRecentHealItemType(HealingItemType.Fish);
                playerSprite.SetBehaviourTextureFlash(0.5f, Color.green);
                UIPlayerManager.TriggerEvent("ReportUIPlayerStatEvent", ref playerStat, _playerID);
                collision.GetComponent<Fish>().Destroy();
                AudioEventManager.PlayPropSFX(PropSFXID.PropHealUpSFX);
            }

            _healthPickupEnabled = false;

            return;
        }

        private void ProcessCurrencyCollisions(ref PlayerStats playerStat)
        {
            if (!_currencyPickupEnabled) return;

            if (GetCollider2DIdentity(C2D.CurrencyCollider2D).CompareTag(GetCompareTag(CompareTags.Currency)))
            {
                ProcessCollisionWithGoldCoin(GetCollider2DIdentity(C2D.CurrencyCollider2D), ref playerStat);
                ProcessCollisionWithRuby(GetCollider2DIdentity(C2D.CurrencyCollider2D), ref playerStat);
                ProcessCollisionWithSapphire(GetCollider2DIdentity(C2D.CurrencyCollider2D), ref playerStat);
                ProcessCollisionWithEmerald(GetCollider2DIdentity(C2D.CurrencyCollider2D), ref playerStat);
                ProcessCollisionWithDiamond(GetCollider2DIdentity(C2D.CurrencyCollider2D), ref playerStat);
                SetCollider2DIdentity(null, C2D.CurrencyCollider2D);
            }
        }
        private void ProcessCollisionWithGoldCoin(Collider2D collision, ref PlayerStats playerStat)
        {
            if (collision == null) return;
            if (collision.GetComponent<GoldCoin>() == null) //Exists on Items Layer, Tag=Currency
                return; //Then NOT a GoldCoin. Get outta here!

            int currencyValue = collision.GetComponent<GoldCoin>().Amount();
            playerStat.Currency += currencyValue;
            UIPlayerManager.TriggerEvent("ReportUIPlayerGoldscoreEvent", ref playerStat, _playerID);
            collision.GetComponent<GoldCoin>().Destroy();
            AudioEventManager.PlayPropSFX(PropSFXID.PropCoinPickSFX);
            _currencyPickupEnabled = false;
            
            return;
        }
        private void ProcessCollisionWithRuby(Collider2D collision, ref PlayerStats playerStat)
        {
            if (collision == null) return;
            if (collision.GetComponent<Ruby>() == null) //Exists on Items Layer, Tag=Currency
                return; //Then NOT a Ruby. Get outta here!

            int currencyValue = collision.GetComponent<Ruby>().Amount();
            playerStat.Currency += currencyValue;
            UIPlayerManager.TriggerEvent("ReportUIPlayerGoldscoreEvent", ref playerStat, _playerID);
            collision.GetComponent<Ruby>().Destroy();
            AudioEventManager.PlayPropSFX(PropSFXID.PropCoinPickSFX);
            _currencyPickupEnabled = false;
            return;
        }
        private void ProcessCollisionWithSapphire(Collider2D collision, ref PlayerStats playerStat)
        {
            if (collision == null) return;
            if (collision.GetComponent<Sapphire>() == null) //Exists on Items Layer, Tag=Currency
                return; //Then NOT a Sapphire. Get outta here!

            int currencyValue = collision.GetComponent<Sapphire>().Amount();
            playerStat.Currency += currencyValue;
            UIPlayerManager.TriggerEvent("ReportUIPlayerGoldscoreEvent", ref playerStat, _playerID);
            collision.GetComponent<Sapphire>().Destroy();
            AudioEventManager.PlayPropSFX(PropSFXID.PropCoinPickSFX);
            _currencyPickupEnabled = false;
            return;
        }
        private void ProcessCollisionWithEmerald(Collider2D collision, ref PlayerStats playerStat)
        {
            if (collision == null) return;
            if (collision.GetComponent<Emerald>() == null) //Exists on Items Layer, Tag=Currency
                return; //Then NOT a Emerald. Get outta here!

            int currencyValue = collision.GetComponent<Emerald>().Amount();
            playerStat.Currency += currencyValue;
            UIPlayerManager.TriggerEvent("ReportUIPlayerGoldscoreEvent", ref playerStat, _playerID);
            collision.GetComponent<Emerald>().Destroy();
            AudioEventManager.PlayPropSFX(PropSFXID.PropCoinPickSFX);
            _currencyPickupEnabled = false;
            return;
        }
        private void ProcessCollisionWithDiamond(Collider2D collision, ref PlayerStats playerStat)
        {
            if (collision == null) return;
            if (collision.GetComponent<Diamond>() == null) //Exists on Items Layer, Tag=Currency
                return; //Then NOT a Diamond. Get outta here!

            int currencyValue = collision.GetComponent<Diamond>().Amount();
            playerStat.Currency += currencyValue;
            UIPlayerManager.TriggerEvent("ReportUIPlayerGoldscoreEvent", ref playerStat, _playerID);
            collision.GetComponent<Diamond>().Destroy();
            AudioEventManager.PlayPropSFX(PropSFXID.PropCoinPickSFX);
            _currencyPickupEnabled = false;
            return;
        }


        private void ProcessCollisionWithBarrel(Collider2D collision)
        {
            if (collision.GetComponent<Barrel>() == null) //Exists on Enemy Layer, Tag=Container
                return; //Then NOT a Barrel. Get outta here!

            collision.GetComponent<Barrel>().IsHit();


            return;
        }
        public void OnTriggerEnter2D(Collider2D collision)
		{
            if (collision.CompareTag(GetCompareTag(CompareTags.Item)))
            {
                SetCollider2DIdentity(collision, C2D.EquipmentCollider2D);//_equipCollider2d = collision;
                SignalItemPickupCollision(true);
            }
            if (collision.CompareTag(GetCompareTag(CompareTags.Health)))
            {
                SetCollider2DIdentity(collision, C2D.HealthCollider2D);//_healthCollider2d = collision;
                _healthPickupEnabled = true;
            }
			if (collision.CompareTag(GetCompareTag(CompareTags.Currency)))
			{
                SetCollider2DIdentity(collision, C2D.CurrencyCollider2D);
                _currencyPickupEnabled = true;
			}
		}
        //void OnTriggerStay2D(Collider2D collision) => ProcessCollisionFlags(collision); //unreliable. We use ProcessItemCollision() instead.
        void OnTriggerExit2D(Collider2D collision)
		{
            if (collision.CompareTag(GetCompareTag(CompareTags.Item)))
            {
                SetCollider2DIdentity(null, C2D.EquipmentCollider2D);//_equipCollider2d = null;
                SignalItemPickupCollision(false);
            }
            if (collision.CompareTag(GetCompareTag(CompareTags.Health))) SetCollider2DIdentity(null, C2D.HealthCollider2D); //_healthCollider2d = null;
            if (collision.CompareTag(GetCompareTag(CompareTags.Currency))) SetCollider2DIdentity(null, C2D.CurrencyCollider2D);
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
