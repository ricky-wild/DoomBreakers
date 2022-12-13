using System;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum PlayerSFXID
    {
        PlayerJumpSFX = 1,
        PlayerDoubleJumpSFX = 2,
        PlayerLandImpactSFX = 3,
        PlayerQuickAttackSFX = 4,
        PlayerChargeAttackSFX = 5,
        PlayerPowerAttackSFX = 6,
        PlayerWaterImpactSFX = 7,
        PlayerWaterMoveSFX = 8,
        PlayerKnockAttackSFX = 9,
        PlayerDodgeSFX = 10,
        PlayerEquippedSFX = 11,
        PlayerHitSFX = 12,
        PlayerArmorHitSFX = 13,
        PlayerArmorBrokenSFX = 14,
        PlayerDefenseHitSFX = 15,
        PlayerDeathSFX = 16
    };
    public enum EnemySFXID
    {
        EnemyJumpSFX = 1,
        EnemyDoubleJumpSFX = 2,
        EnemyLandImpactSFX = 3,
        EnemyQuickAttackSFX = 4,
        EnemyChargeAttackSFX = 5,
        EnemyPowerAttackSFX = 6,
        EnemyWaterImpactSFX = 7,
        EnemyWaterMoveSFX = 8,
        EnemyKnockAttackSFX = 9,
        EnemyDodgeSFX = 10,
        EnemyHitSFX = 11,
        EnemyArmorHitSFX = 12,
        EnemyArmorBrokenSFX = 13,
        EnemyDefenseHitSFX = 14,
        EnemyDeathSFX = 15
    };
    public enum PropSFXID
	{
        PropHitSFX = 1,
        PropDestroySFX = 2,
        PropCoinPickSFX = 3,
	};
    public class AudioEventManager : MonoBehaviour
    {
        public static AudioEventManager _instance = null;

        private static Transform _transform;

        private static float _volumeSFX;

        private static Dictionary<PlayerSFXID, Audio> _playerSFXDict;
        private static Dictionary<EnemySFXID, Audio> _enemySFXDict;
        private static Dictionary<PropSFXID, Audio> _propSFXDict;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _transform = this.transform;
                Setup();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);


        }
        private static void Setup()
        {
            _volumeSFX = 0.15f;

            if (_playerSFXDict == null)
                _playerSFXDict = new Dictionary<PlayerSFXID, Audio>();
            if (_enemySFXDict == null)
                _enemySFXDict = new Dictionary<EnemySFXID, Audio>();
            if (_propSFXDict == null)
                _propSFXDict = new Dictionary<PropSFXID, Audio>();

            InitializePlayersSFX();
            InitializeEnemySFX();
            InitializePropSFX();
        }

        private static void InitializePlayersSFX()
        {
            PlayerSFXID sfxId = PlayerSFXID.PlayerJumpSFX;
            //_playerSFX[0] = new Audio("playerJump0", _volumeSFX, false, _transform);
            _playerSFXDict.Add(sfxId, new Audio("playerJump0", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerDoubleJumpSFX;
			_playerSFXDict.Add(sfxId, new Audio("playerDblJump0", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerLandImpactSFX;
			_playerSFXDict.Add(sfxId, new Audio("playerLand0", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerQuickAttackSFX;
			_playerSFXDict.Add(sfxId, new Audio("playerQuickAtkSFX", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerChargeAttackSFX;
			_playerSFXDict.Add(sfxId, new Audio("weaponCharge0", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerPowerAttackSFX;
            _playerSFXDict.Add(sfxId, new Audio("powerAttack0", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerWaterImpactSFX;
			_playerSFXDict.Add(sfxId, new Audio("waterImpactSFX", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerWaterMoveSFX;
			_playerSFXDict.Add(sfxId, new Audio("waterMovementSFX", _volumeSFX, false, _transform));

			sfxId = PlayerSFXID.PlayerKnockAttackSFX;
			_playerSFXDict.Add(sfxId, new Audio("knockBackAtkSFX0", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerDodgeSFX;
            _playerSFXDict.Add(sfxId, new Audio("playerDodgeSFX", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerEquippedSFX;
            _playerSFXDict.Add(sfxId, new Audio("armorUp", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerHitSFX;
            _playerSFXDict.Add(sfxId, new Audio("hit0", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerArmorHitSFX;
            _playerSFXDict.Add(sfxId, new Audio("playerArmorHit", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerArmorBrokenSFX;
            _playerSFXDict.Add(sfxId, new Audio("armorBreak", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerDefenseHitSFX;
            _playerSFXDict.Add(sfxId, new Audio("playerShieldHit", _volumeSFX, false, _transform));

            sfxId = PlayerSFXID.PlayerDeathSFX;
            _playerSFXDict.Add(sfxId, new Audio("deathEvent", _volumeSFX, false, _transform));
            //
        }
        public static void PlayPlayerSFX(PlayerSFXID playerSFXID) => _playerSFXDict[playerSFXID].PlaySound();
        public static void StopPlayerSFX(PlayerSFXID playerSFXID) => _playerSFXDict[playerSFXID].StopSound();


        private static void InitializeEnemySFX()
		{
            EnemySFXID sfxId = EnemySFXID.EnemyHitSFX;
            _enemySFXDict.Add(sfxId, new Audio("hit2", _volumeSFX, false, _transform));

            sfxId = EnemySFXID.EnemyDoubleJumpSFX;
            _enemySFXDict.Add(sfxId, new Audio("playerDblJump0", _volumeSFX, false, _transform));

            sfxId = EnemySFXID.EnemyLandImpactSFX;
            _enemySFXDict.Add(sfxId, new Audio("playerLand0", _volumeSFX, false, _transform));
        }
        public static void PlayEnemySFX(EnemySFXID enemySFXID) => _enemySFXDict[enemySFXID].PlaySound();
        public static void StopEnemySFX(EnemySFXID enemySFXID) => _enemySFXDict[enemySFXID].StopSound();

        private static void InitializePropSFX()
        {
            PropSFXID sfxId = PropSFXID.PropHitSFX;
            _propSFXDict.Add(sfxId, new Audio("playerLand2", _volumeSFX, false, _transform));

            sfxId = PropSFXID.PropDestroySFX;
            _propSFXDict.Add(sfxId, new Audio("armorBreak", _volumeSFX, false, _transform));

            sfxId = PropSFXID.PropCoinPickSFX;
            _propSFXDict.Add(sfxId, new Audio("currencyPickup", _volumeSFX, false, _transform));

        }
        public static void PlayPropSFX(PropSFXID propSFXID) => _propSFXDict[propSFXID].PlaySound();
        public static void StopPropSFX(PropSFXID propSFXID) => _propSFXDict[propSFXID].StopSound();

    }
}
