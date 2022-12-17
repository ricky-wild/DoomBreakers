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

		public struct UITextToSceenSpace
		{
			public Vector2 _positionOnScreen, _finalPosition, _originalScale;
			public float _scaleFactor;
			public RectTransform _rectForText;
			public bool _displayFlag, _getPosition, _getStringForTextFlag, _forHealthFlag;
		}
		private UITextToSceenSpace _UIItemTextToScreenSpace, _UIBattleTextToScreenSpace;

		[Header("Player ID")]
		public int _playerID;

		[Header("UI Canvas")]
		public Canvas _parentCanvas;

		[Header("Player Transform")]
		public Transform _playerTransform;

		[Header("Username Text")]
		public TMPro.TextMeshProUGUI _usernameText;

		[Header("Item Collided Text")]
		public TextMeshProUGUI _itemText;

		[Header("Battle Text")]
		public TextMeshProUGUI _battleText;

		[Header("Currency Text")]
		public TextMeshProUGUI _currencyText;
		public Image _currencyImage;

		[Header("Deathcount Text")]
		public TextMeshProUGUI _killCountText;
		public Image _killCountImage;

		[Header("UI Frame Animator")]
		public Animator _UIFrameAnim;
		public Image _UIFrameImage;

		[Header("LeftHand Equipment Animator")]
		public Animator _leftHandEquipAnim;
		public Image _leftHandEquipImage;

		[Header("RightHand Equipment Animator")]
		public Animator _rightHandEquipAnim;
		public Image _rightHandEquipImage;

		[Header("Torso Equipment Animator")]
		public Animator _torsoEquipAnim;
		public Image _torsoEquipImage;

		[Header("Health Image")]
		public Image _healthUIImage;
		[Header("Stamina Image")]
		public Image _staminaUIImage;
		[Header("Defense Image")]
		public Image _defenseUIImage;

		private Color32[] _UIcolours = new Color32[4];

		//Cache the anim strings _UIFrameAnim.Play(animName);//"P1_Hit", "P1_Idle", "P1_Heal", "P1_Dead"
		private string[] _UIframeAnimStr = new string[4];

		private PlayerStats _playerStats, _prevPlayerStats;
		private ITimer _timer, _textItemDisplayTimer, _textBattleDisplayTimer;

		private bool _hitFrameAnimFlag;

		private Action[] _actionListener = new Action[4];

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

			//_UIcolours[] array element order corrisponds to that of _UIframeAnimStr[].
			_UIcolours[0] = _healthUIImage.color;
			_UIcolours[1] = new Color32(255, 28, 28, 175);//Hit colour.
			_UIcolours[2] = _healthUIImage.color;
			_UIcolours[3] = new Color32(51, 51, 51, 175);//Dead colour.

			SetupTextToScreenSpace();

			_timer = new Timer();
			_hitFrameAnimFlag = false;

			_actionListener[0] = new Action(UIPlayerStatsEvent);//UIPlayerStatsEvent()
			_actionListener[1] = new Action(UIPlayerEquipEvent);//UIPlayerEquipEvent()
			_actionListener[2] = new Action(UIPlayerKillScoreEvent);//UIPlayerKillScoreEvent()
			_actionListener[3] = new Action(UIPlayerGoldScoreEvent);//UIPlayerGoldScoreEvent()
		}
		private void SetupTextToScreenSpace()
		{
			_UIItemTextToScreenSpace._displayFlag = false;
			_UIItemTextToScreenSpace._getPosition = false;
			_UIItemTextToScreenSpace._forHealthFlag = false;
			_UIItemTextToScreenSpace._rectForText = _itemText.gameObject.GetComponent<RectTransform>();
			_UIItemTextToScreenSpace._originalScale = _UIItemTextToScreenSpace._rectForText.localScale;
			_itemText.text = "";
			_textItemDisplayTimer = new Timer();

			_UIBattleTextToScreenSpace._displayFlag = false;
			_UIBattleTextToScreenSpace._getPosition = false;
			_UIBattleTextToScreenSpace._rectForText = _battleText.gameObject.GetComponent<RectTransform>();
			_UIBattleTextToScreenSpace._originalScale = _UIBattleTextToScreenSpace._rectForText.localScale;
			_battleText.text = "";
			_textBattleDisplayTimer = new Timer();
		}
		private string GetFrameAnim(UIFrameAnimID id) => _UIframeAnimStr[(int)id];

		private void Awake() => InitializeUI();

		private void UIPlayerGoldScoreEvent()
		{
			//Used for communicating player currency count.PlayerCollision.cs->ProcessCollisionWithGoldCoin() ect.
			_playerStats = UIPlayerManager.GetPlayerStats(_playerID); //because we store kill count here.

			_currencyText.text = "" + _playerStats.Currency.ToString();
		}
		private void UIPlayerKillScoreEvent()
		{
			//Used for communicating player kill count.Bandit.cs->UpdateStats()
			_playerStats = UIPlayerManager.GetPlayerStats(_playerID); //because we store kill count here.
			_playerStats.IncrementKillCount(1);
			UIPlayerManager.SetPlayerStats(ref _playerStats, _playerID);

			_killCountText.text = ""+_playerStats.GetKillCount().ToString();
		}
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
			QueueItemToTextProcess(false);
		}
		private void UIPlayerStatsEvent() 
		{

			ProcessHealth();
			ProcessStamina();
			ProcessDefense();

			UIHealthUpdate((float)_playerStats.Health);
			UIStaminaUpdate((float)_playerStats.Stamina);
			UIDefenseUpdate((float)_playerStats.Defence);
		}
		private void ProcessHealth()
		{
			//Then this indicates health has decreased.
			if (_prevPlayerStats.Health > UIPlayerManager.GetPlayerStats(_playerID).Health)
			{
				//if (!_playerStats.IsArmored()) return;
				_UIAnimFlag = UIAnimationFlag.UIFrame;
				PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Hit));//("P" + (_playerID + 1).ToString() + "_Hit"); //anim length 1.017 sec
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Health = _playerStats.Health;
				_timer.StartTimer(1.0f); //We don't loop P1_Hit anim.
				_hitFrameAnimFlag = true;
			}
			//Then this indicates health has increased.
			if (_prevPlayerStats.Health < UIPlayerManager.GetPlayerStats(_playerID).Health)
			{
				//if (!_playerStats.IsArmored()) return;
				_UIAnimFlag = UIAnimationFlag.UIFrame;
				PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Heal));//("P" + (_playerID + 1).ToString() + "_Hit"); //anim length 1.017 sec
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Health = _playerStats.Health;
				_timer.StartTimer(1.0f); //We don't loop P1_Hit anim.
				_hitFrameAnimFlag = true;

				QueueItemToTextProcess(true);
			}
			//Then this indicates health has hit zero and death is upon thee.
			if (UIPlayerManager.GetPlayerStats(_playerID).Health <= 0)//(_playerStats.Health <= 0f)
			{
				SetUIColour(UIFrameAnimID.Dead);
				_UIAnimFlag = UIAnimationFlag.UIFrame;
				PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Dead));
				_hitFrameAnimFlag = false;
			}
		}
		private void ProcessStamina()
		{
			//Then this indicates stamina has lowered.
			if (_prevPlayerStats.Stamina > UIPlayerManager.GetPlayerStats(_playerID).Stamina)
			{
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Stamina = _playerStats.Stamina;
			}
		}
		private void ProcessDefense()
		{
			//Then this indicates defense has lowered.
			if (_prevPlayerStats.Defence > UIPlayerManager.GetPlayerStats(_playerID).Defence && _playerStats.IsArmored())
			{
				_playerStats = UIPlayerManager.GetPlayerStats(_playerID);
				_prevPlayerStats.Defence = _playerStats.Defence;
			}
			if (_playerStats.Defence <= 0f) _torsoEquipAnim.Play("nothing");
		}
		private void UIHealthUpdate(float value) => _healthUIImage.fillAmount = value;
		private void UIStaminaUpdate(float value) => _staminaUIImage.fillAmount = value;
		private void UIDefenseUpdate(float value) => _defenseUIImage.fillAmount = value;

		private void OnEnable()
		{
			UIPlayerManager.Subscribe("ReportUIPlayerStatEvent", _actionListener[0]);
			UIPlayerManager.Subscribe("ReportUIPlayerEquipEvent", _actionListener[1]);
			UIPlayerManager.Subscribe("ReportUIPlayerKillScoreEvent", _actionListener[2]);
			UIPlayerManager.Subscribe("ReportUIPlayerGoldscoreEvent", _actionListener[3]);
		}
		private void OnDisable()
		{
			UIPlayerManager.Unsubscribe("ReportUIPlayerStatEvent", _actionListener[0]);
			UIPlayerManager.Unsubscribe("ReportUIPlayerEquipEvent", _actionListener[1]);
			UIPlayerManager.Unsubscribe("ReportUIPlayerKillScoreEvent", _actionListener[2]);
			UIPlayerManager.Unsubscribe("ReportUIPlayerGoldscoreEvent", _actionListener[3]);
		}

		void Start() { }
		private void Update()
		{
			UpdatePlayerUI();
			UpdateUIItemTextToScreenSpace();
			UpdateUIBattleTextToScreenSpace();
		}
		private void UpdatePlayerUI()
		{
			if (!_hitFrameAnimFlag) return;

			if (_timer.HasTimerFinished())
			{
				_hitFrameAnimFlag = false;
				PlayUIAnimation(GetFrameAnim(UIFrameAnimID.Idle));
			}
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

		private void SetUIColour(UIFrameAnimID uIFrameAnimID)
		{
			_UIFrameImage.color = _UIcolours[(int)uIFrameAnimID];

			_healthUIImage.color = _UIcolours[(int)uIFrameAnimID];
			_staminaUIImage.color = _UIcolours[(int)uIFrameAnimID];
			_defenseUIImage.color = _UIcolours[(int)uIFrameAnimID];

			_leftHandEquipImage.color = _UIcolours[(int)uIFrameAnimID];
			_rightHandEquipImage.color = _UIcolours[(int)uIFrameAnimID];
			_torsoEquipImage.color = _UIcolours[(int)uIFrameAnimID];

			_currencyImage.color = _UIcolours[(int)uIFrameAnimID];
			_killCountImage.color = _UIcolours[(int)uIFrameAnimID];

			_usernameText.color = _UIcolours[(int)uIFrameAnimID];
			_currencyText.color = _UIcolours[(int)uIFrameAnimID];
			_killCountText.color = _UIcolours[(int)uIFrameAnimID];
		}

		private void QueueItemToTextProcess(bool forHealth)
		{
			_UIItemTextToScreenSpace._forHealthFlag = forHealth;
			_UIItemTextToScreenSpace._displayFlag = true;

			if (!forHealth) _textItemDisplayTimer.StartTimer(1.0f);
			if (forHealth) _textItemDisplayTimer.StartTimer(0.5f);
		}
		private void UpdateUIItemTextToScreenSpace()
		{
			if (!_UIItemTextToScreenSpace._displayFlag) return;

			if(_textItemDisplayTimer.HasTimerFinished())
			{
				_itemText.text = "";
				_UIItemTextToScreenSpace._displayFlag = false;
				_UIItemTextToScreenSpace._getPosition = false;
				_UIItemTextToScreenSpace._getStringForTextFlag = false;
				return;
			}

			if(!_UIItemTextToScreenSpace._forHealthFlag)
			{
				if (!_UIItemTextToScreenSpace._getStringForTextFlag)
				{
					IPlayerEquipment playerEquipment = UIPlayerManager.GetPlayerEquipment(_playerID);
					ItemBase itemBase = playerEquipment.GetMostRecentEquipment();

					if (itemBase.GetType() == typeof(Sword))
					{
						Sword weaponDervived = itemBase as Sword;
						string weaponType = "";
						string weaponMaterial = "";

						if (weaponDervived.GetSwordType() == EquipmentWeaponType.Broadsword) weaponType = "BROADSWORD";
						if (weaponDervived.GetSwordType() == EquipmentWeaponType.Longsword) weaponType = "LONGSWORD";

						if (weaponDervived.GetMaterialType() == EquipmentMaterialType.Bronze) weaponMaterial = "BRONZE ";
						if (weaponDervived.GetMaterialType() == EquipmentMaterialType.Iron) weaponMaterial = "IRON ";
						if (weaponDervived.GetMaterialType() == EquipmentMaterialType.Steel) weaponMaterial = "STEEL ";
						if (weaponDervived.GetMaterialType() == EquipmentMaterialType.Ebony) weaponMaterial = "EBONY ";

						_itemText.text = weaponMaterial + weaponType;
					}
					if (itemBase.GetType() == typeof(Shield))
					{
						Shield shieldDervived = itemBase as Shield;
						string shieldType = "";
						string shieldMaterial = "";

						shieldType = "SHIELD";

						if (shieldDervived.GetMaterialType() == EquipmentMaterialType.Bronze) shieldMaterial = "BRONZE ";
						if (shieldDervived.GetMaterialType() == EquipmentMaterialType.Iron) shieldMaterial = "IRON ";
						if (shieldDervived.GetMaterialType() == EquipmentMaterialType.Steel) shieldMaterial = "STEEL ";
						if (shieldDervived.GetMaterialType() == EquipmentMaterialType.Ebony) shieldMaterial = "EBONY ";

						_itemText.text = shieldMaterial + shieldType;
					}
					if (itemBase.GetType() == typeof(Breastplate))
					{
						Breastplate armorDervived = itemBase as Breastplate;
						string armorType = "";
						string armorMaterial = "";

						armorType = "BREASTPLATE";

						if (armorDervived.GetMaterialType() == EquipmentMaterialType.Bronze) armorMaterial = "BRONZE ";
						if (armorDervived.GetMaterialType() == EquipmentMaterialType.Iron) armorMaterial = "IRON ";
						if (armorDervived.GetMaterialType() == EquipmentMaterialType.Steel) armorMaterial = "STEEL ";
						if (armorDervived.GetMaterialType() == EquipmentMaterialType.Ebony) armorMaterial = "EBONY ";

						_itemText.text = armorMaterial + armorType;
					}

					_UIItemTextToScreenSpace._getStringForTextFlag = true;
				}
			}
			if (_UIItemTextToScreenSpace._forHealthFlag)
			{
				if (_playerStats.GetRecentHealItemType() == HealingItemType.None) _itemText.text = "";
				if (_playerStats.GetRecentHealItemType() == HealingItemType.Apple) _itemText.text = "JUICY APPLE";
				if (_playerStats.GetRecentHealItemType() == HealingItemType.Chicken) _itemText.text = "TASTY CHICKEN";
				if (_playerStats.GetRecentHealItemType() == HealingItemType.Fish) _itemText.text = "DELICIOUS FISH";
			}

			if (!_UIItemTextToScreenSpace._getPosition)
			{
				if (Camera.main != null)
					_UIItemTextToScreenSpace._positionOnScreen = Camera.main.WorldToScreenPoint(_playerTransform.position);

				_UIItemTextToScreenSpace._scaleFactor = _parentCanvas.scaleFactor;
				_UIItemTextToScreenSpace._finalPosition.x = _UIItemTextToScreenSpace._positionOnScreen.x;
				_UIItemTextToScreenSpace._finalPosition.y = _UIItemTextToScreenSpace._positionOnScreen.y;
				_UIItemTextToScreenSpace._rectForText.position = _UIItemTextToScreenSpace._finalPosition;
				_UIItemTextToScreenSpace._getPosition = true;
			}

		}

		private void QueueBattleToTextProcess()
		{
			_UIBattleTextToScreenSpace._displayFlag = true;
			_textBattleDisplayTimer.StartTimer(0.5f);
		}
		private void UpdateUIBattleTextToScreenSpace()
		{
			if (!_UIBattleTextToScreenSpace._displayFlag) return;

			if (_textBattleDisplayTimer.HasTimerFinished())
			{
				_battleText.text = "";
				_UIBattleTextToScreenSpace._displayFlag = false;
				_UIBattleTextToScreenSpace._getPosition = false;
				_UIBattleTextToScreenSpace._getStringForTextFlag = false;
				return;
			}

			if (!_UIBattleTextToScreenSpace._getStringForTextFlag)
			{


				_UIBattleTextToScreenSpace._getStringForTextFlag = true;
			}

			if (!_UIBattleTextToScreenSpace._getPosition)
			{
				if (Camera.main != null)
					_UIBattleTextToScreenSpace._positionOnScreen = Camera.main.WorldToScreenPoint(_playerTransform.position);

				_UIBattleTextToScreenSpace._scaleFactor = _parentCanvas.scaleFactor;
				_UIBattleTextToScreenSpace._finalPosition.x = _UIBattleTextToScreenSpace._positionOnScreen.x;
				_UIBattleTextToScreenSpace._finalPosition.y = _UIBattleTextToScreenSpace._positionOnScreen.y;
				_UIBattleTextToScreenSpace._rectForText.position = _UIBattleTextToScreenSpace._finalPosition;
				_UIBattleTextToScreenSpace._getPosition = true;
			}

		}
	}
}
