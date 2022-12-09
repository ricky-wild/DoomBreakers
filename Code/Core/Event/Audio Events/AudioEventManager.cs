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
        PlayerHitSFX = 12
    }
    public class AudioEventManager : MonoBehaviour
    {
        public static AudioEventManager _instance = null;

        private static Transform _transform;

        private static float _volumeSFX;

        private static Dictionary<PlayerSFXID, Audio> _playerSFXDict;

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
            _volumeSFX = 1.0f;

            if (_playerSFXDict == null)
                _playerSFXDict = new Dictionary<PlayerSFXID, Audio>();

            InitializePlayersSFX();
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

        }
        public static void PlayPlayerSFX(PlayerSFXID playerSFXID) => _playerSFXDict[playerSFXID].PlaySound();
        public static void StopPlayerSFX(PlayerSFXID playerSFXID) => _playerSFXDict[playerSFXID].StopSound();




    }
}
