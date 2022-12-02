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


		private Action[] _actionListener = new Action[0];

		private void InitializeUI()
		{
			_usernameText.text = "Joe Bloggs";//MenuManager._instance.GetUserName(_id + 1);

			_currencyText.text = "0";//GameManager.GetPlayerStatData(_id).Currency.ToString();
			_killCountText.text = "0";//GameManager.GetPlayerStatData(_id).KillCount.ToString();

			_UIAnimFlag = UIAnimationFlag.None;
			_leftHandEquipAnim.Play("nothing");//_leftHandEquipAnim.gameObject.SetActive(false);
			_rightHandEquipAnim.Play("nothing");//_rightHandEquipAnim.gameObject.SetActive(false);
			_torsoEquipAnim.Play("nothing");//_torsoEquipAnim.gameObject.SetActive(false);



			_actionListener[0] = new Action(UIPlayerEvent);//UIPlayerEvent()
		}

		private void Awake() => InitializeUI();

		private void UIPlayerEvent() { }

		private void OnEnable() => UIPlayerManager.Subscribe("ReportUIPlayerEvent", _actionListener[0]);
		private void OnDisable() => UIPlayerManager.Unsubscribe("ReportUIPlayerEvent", _actionListener[0]);

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
