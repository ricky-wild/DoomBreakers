using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DoomBreakers
{
	//<summary>
	//The PlayerUI purpose is to bring all the objects together in UI space
	//that represent a Player' UI. Some would include, Health, Stamina, Armor
	//Currency obj and so on. Then we communicate with the UIPlayerManager.cs to
	//recieve appropriate data regarding the Player.cs.
	//</summary>

	public enum UIAnimationFlag
	{
		None = -1,
		UIFrame = 0,
		UILeftHandSword = 1,
		UIRightHandSword = 2,
		UILeftHandShield = 3,
		UIRightHandShield = 4,
		UITorsoEquip = 5
	}
	public class PlayerUI : MonoBehaviour
	{

		private UIAnimationFlag _UIAnimFlag;

		public enum UIFrameAnimID //_UIFrameAnim.Play(animName);//"P1_Hit", "P1_Idle", "P1_Heal", "P1_Dead"
		{
			Idle = 0,//_UIframeAnimStr[0]
			Hit = 1,//_UIframeAnimStr[1] and so on..
			Heal = 2,
			Dead = 3
		}; //Must corrispond to string[] _UIframeAnimStr assignment order.

		[Header("Player ID")]
		public int _playerID;
		[Header("Username Text")]
		public TMPro.TextMeshProUGUI _usernameText;
		[Header("Currency Text")]
		public TextMeshProUGUI _currencyText;
		[Header("Deathcount Text")]
		public TextMeshProUGUI _killCountText;

		[Header("UI Frame Animator")]
		public Animator _UIFrameAnim;

		[Header("LeftHand Equipment Animator")]
		public Animator _leftHandEquipAnim;
		[Header("RightHand Equipment Animator")]
		public Animator _rightHandEquipAnim;
		[Header("Torso Equipment Animator")]
		public Animator _torsoEquipAnim;

		[Header("Health Image")]
		public Image _healthUIImage;
		[Header("Stamina Image")]
		public Image _staminaUIImage;
		[Header("Defense Image")]
		public Image _defenseUIImage;

		//Cache the anim strings _UIFrameAnim.Play(animName);//"P1_Hit", "P1_Idle", "P1_Heal", "P1_Dead"
		private string[] _UIframeAnimStr = new string[4];

		private PlayerStats _playerStats, _prevPlayerStats;
		private ITimer _timer;

		private Action[] _actionListener = new Action[2];

		private void InitializeUI()
		{
			_usernameText.text = "Joe Bloggs";//MenuManager._instance.GetUserName(_id + 1);

			_currencyText.text = "0";//GameManager.GetPlayerStatData(_id).Currency.ToString();
			_killCountText.text = "0";//GameManager.GetPlayerStatData(_id).KillCount.ToString();

			_leftHandEquipAnim.Play("nothing");
			_rightHandEquipAnim.Play("nothing");
			_torsoEquipAnim.Play("nothing");

			_UIAnimFlag = UIAnimationFlag.UIFrame;
			PlayUIAnimation("P" + (_playerID + 1).ToString() + "_Idle");
			_UIAnimFlag = UIAnimationFlag.None;

			_healthUIImage.fillAmount = 1.0f;
			_staminaUIImage.fillAmount = 1.0f;
			_defenseUIImage.fillAmount = 0.0f;
			_playerStats = new PlayerStats(1, 1, 0);
			_prevPlayerStats = new PlayerStats(1, 1, 0);

			_UIframeAnimStr[0] = "P" + (_playerID + 1).ToString() + "_Idle";
			_UIframeAnimStr[1] = "P" + (_playerID + 1).ToString() + "_Hit";
			_UIframeAnimStr[2] = "P" + (_playerID + 1).ToString() + "_Heal";
			_UIframeAnimStr[3] = "P" + (_playerID + 1).ToString() + "_Dead";

			_timer = new Timer();

			_actionListener[0] = new Action(UIPlayerStatsEvent);//UIPlayerStatsEvent()
			_actionListener[1] = new Action(UIPlayerEquipEvent);//UIPlayerEquipEvent()
		}
		private string GetFrameAnim(UIFrameAnimID id) => _UIframeAnimStr[(int)id];

		private void Awake() => InitializeUI();

		private void UIPlayerEquipEvent()
		{
			_UIAnimFlag = UIPlayerManager.GetEquipmentGainedFlag(_playerID);

			if (_UIAnimFlag == UIAnimationFlag.UILeftHandSword) PlayUIAnimation("sword");
			if (_UIAnimFlag == UIAnimationFlag.UIRightHandSword) PlayUIAnimation("sword");
			if (_UIAnimFlag == UIAnimationFlag.UILeftHandShield) PlayUIAnimation("shield");
			if (_UIAnimFlag == UIAnimationFlag.UIRightHandShield) PlayUIAnimation("shield");
			if (_UIAnimFlag == UIAnimationFlag.UITorsoEquip)
			{
				PlayUIAnimation("armor");
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_playerStats.IsArmored(true);
				UIPlayerManager.SetPlayerStats(ref _playerStats, _playerID);
			}
		}
		private void UIPlayerStatsEvent() 
		{

			//Then this indicates health has lowered.
			if (_prevPlayerStats.Health > UIPlayerManager.GetPlayerStats(_playerID).Health)
			{
				if(!_playerStats.IsArmored())
				{
					_UIAnimFlag = UIAnimationFlag.UIFrame;
					PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Hit));//("P" + (_playerID + 1).ToString() + "_Hit"); //anim length 1.017 sec
					_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
					_prevPlayerStats.Health = _playerStats.Health;
					_timer.StartTimer(1.0f); //We don't loop P1_Hit anim.

				}
			}
			//Then this indicates stamina has lowered.
			if (_prevPlayerStats.Stamina > UIPlayerManager.GetPlayerStats(_playerID).Stamina)
			{
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Stamina = _playerStats.Stamina;
			}
			//Then this indicates defense has lowered.
			if (_prevPlayerStats.Defence > UIPlayerManager.GetPlayerStats(_playerID).Defence && _playerStats.IsArmored())
			{
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Defence = _playerStats.Defence;
			}
			if (_playerStats.Defence <= 0f) _torsoEquipAnim.Play("nothing");
			//{
			//_UIAnimFlag = UIAnimationFlag.UITorsoEquip;
			//PlayUIAnimation("nothing");
			//}

			UIHealthUpdate((float)_playerStats.Health);
			UIStaminaUpdate((float)_playerStats.Stamina);
			UIDefenseUpdate((float)_playerStats.Defence);
		}
		private void UIHealthUpdate(float value) => _healthUIImage.fillAmount = value;
		private void UIStaminaUpdate(float value) => _staminaUIImage.fillAmount = value;
		private void UIDefenseUpdate(float value) => _defenseUIImage.fillAmount = value;

		private void OnEnable()
		{
			UIPlayerManager.Subscribe("ReportUIPlayerStatEvent", _actionListener[0]);
			UIPlayerManager.Subscribe("ReportUIPlayerEquipEvent", _actionListener[1]);
		}
		private void OnDisable()
		{
			UIPlayerManager.Unsubscribe("ReportUIPlayerStatEvent", _actionListener[0]);
			UIPlayerManager.Unsubscribe("ReportUIPlayerEquipEvent", _actionListener[1]);
		}

		void Start() { }
		private void Update()
		{
			if(_timer.HasTimerFinished())
				PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Idle));
		}

		private void PlayUIAnimation(string animName)
		{
			switch(_UIAnimFlag)
			{
				case UIAnimationFlag.None: return;
				case UIAnimationFlag.UIFrame:
					_UIFrameAnim.Play(animName);//"P1_Hit", "P1_Idle", "P1_Heal", "P1_Dead"
					break;
				case UIAnimationFlag.UILeftHandSword:
					_leftHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UIRightHandSword:
					_rightHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UILeftHandShield:
					_leftHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UIRightHandShield:
					_rightHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UITorsoEquip:
					_torsoEquipAnim.Play(animName);
					break;
			}
		}

	}
}
