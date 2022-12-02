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

	public class PlayerUI : MonoBehaviour
	{

		public enum UIAnimationFlag
		{
			None = -1,
			UIFrame = 0,
			UILeftHandEquip = 1,
			UIRightHandEquip = 2,
			UITorsoEquip = 3
		}
		private UIAnimationFlag _UIAnimFlag;

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

		private PlayerStats _playerStats;

		private Action[] _actionListener = new Action[1];

		private void InitializeUI()
		{
			_usernameText.text = "Joe Bloggs";//MenuManager._instance.GetUserName(_id + 1);

			_currencyText.text = "0";//GameManager.GetPlayerStatData(_id).Currency.ToString();
			_killCountText.text = "0";//GameManager.GetPlayerStatData(_id).KillCount.ToString();

			_UIAnimFlag = UIAnimationFlag.None;
			_leftHandEquipAnim.Play("nothing");//_leftHandEquipAnim.gameObject.SetActive(false);
			_rightHandEquipAnim.Play("nothing");//_rightHandEquipAnim.gameObject.SetActive(false);
			_torsoEquipAnim.Play("nothing");//_torsoEquipAnim.gameObject.SetActive(false);

			_healthUIImage.fillAmount = 1.0f;
			_staminaUIImage.fillAmount = 1.0f;
			_defenseUIImage.fillAmount = 0.0f;
			_playerStats = new PlayerStats(0, 0, 0);

			_actionListener[0] = new Action(UIPlayerEvent);//UIPlayerEvent()
		}

		private void Awake() => InitializeUI();

		private void UIPlayerEvent() 
		{
			_playerStats = UIPlayerManager.GetPlayerStats(_playerID);

			//Then this indicates health has lowered.
			if (_playerStats.Health > UIPlayerManager.GetPlayerStats(_playerID).Health) PlayUIAnimation(UIAnimationFlag.UIFrame, "hit");

			UIHealthUpdate((float)_playerStats.Health);
			UIStaminaUpdate((float)_playerStats.Stamina);
			UIDefenseUpdate((float)_playerStats.Defence);
		}
		private void UIHealthUpdate(float value) => _healthUIImage.fillAmount = value;
		private void UIStaminaUpdate(float value) => _staminaUIImage.fillAmount = value;
		private void UIDefenseUpdate(float value) => _defenseUIImage.fillAmount = value;

		private void OnEnable() => UIPlayerManager.Subscribe("ReportUIPlayerStatEvent", _actionListener[0]);
		private void OnDisable() => UIPlayerManager.Unsubscribe("ReportUIPlayerStatEvent", _actionListener[0]);

		void Start()
		{

		}

		private void PlayUIAnimation(UIAnimationFlag UIanimationFlag, string animName)
		{
			switch(UIanimationFlag)
			{
				case UIAnimationFlag.None: return;
				case UIAnimationFlag.UIFrame:
					_UIFrameAnim.Play(animName);//"hit", "heal", "dead".
					break;
				case UIAnimationFlag.UILeftHandEquip:
					_leftHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UIRightHandEquip:
					_rightHandEquipAnim.Play(animName);//"nothing","sword","shield"
					break;
				case UIAnimationFlag.UITorsoEquip:
					_torsoEquipAnim.Play(animName);
					break;
			}
		}

	}
}
