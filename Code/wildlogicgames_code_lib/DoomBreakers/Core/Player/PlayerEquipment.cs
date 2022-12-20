using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum PlayerItem
	{
        Empty_None = 0,
        IsBroadsword = 1,
        IsLongsword = 2,
        IsMace = 3,
        IsShield = 9,
        IsBreastPlate = 10
	};

    public enum EquipHand
	{
        Left_Hand = 1,
        Right_Hand = 2
	};
    public class PlayerEquipment : IPlayerEquipment//MonoBehaviour
    {
        private int _playerID;
        private ItemBase _torsoEquipment;
        private ItemBase _leftHandEquip;
        private ItemBase _rightHandEquip;
        
        private bool _equipmentGainedFlag;


        private ItemBase _mostRecentEquipGained;
        private EquipmentRating _equipmentChecker;

        public PlayerEquipment(int playerId)
		{
            _playerID = playerId;
            if (_torsoEquipment == null)
                _torsoEquipment = new EmptyHand(EquipmentWeaponType.None, EquipmentMaterialType.None);
            if (_leftHandEquip == null)
                _leftHandEquip = new EmptyHand(EquipmentWeaponType.None, EquipmentMaterialType.None);
            if (_rightHandEquip == null)
                _rightHandEquip = new EmptyHand(EquipmentWeaponType.None, EquipmentMaterialType.None);
            _equipmentChecker = new EquipmentRating();
        }
        public PlayerEquipment(ItemBase torsoEquipment, ItemBase leftHandEquip, ItemBase rightHandEquip)
        {
            _torsoEquipment = torsoEquipment;
            _leftHandEquip = leftHandEquip;
            _rightHandEquip = rightHandEquip;
        }
        public void RemoveArmor() => _torsoEquipment = new EmptyHand(EquipmentWeaponType.None, EquipmentMaterialType.None);
        public bool NewEquipmentGained()
		{
            return _equipmentGainedFlag;

        }
        public void NewEquipmentGained(bool flag)
		{
            _equipmentGainedFlag = flag;
		}

        public ItemBase GetMostRecentEquipment() => _mostRecentEquipGained;




        public bool ApplyEquipment(ItemBase playerEquip)
		{
            if (IsEquipMace(playerEquip))
			{
                //NO EQUIPMENT APPLY SWORD.
                if (IsEmptyHanded(EquipHand.Left_Hand) && IsEmptyHanded(EquipHand.Right_Hand))
                {
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandMace, _playerID);
                    return true;
                }

                //SHIELD IN LEFT HAND, EQUIP SWORD IN RIGHT HAND.
                if (IsShield(EquipHand.Left_Hand))
                {
                    if (IsEmptyHanded(EquipHand.Right_Hand))
                    {
                    }
                    _mostRecentEquipGained = _rightHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandMace, _playerID);
                    return true;
                }

                ////SHIELD IN RIGHT HAND, EQUIP SWORD IN LEFT HAND.
                if (IsShield(EquipHand.Right_Hand))
                {
                    if (IsEmptyHanded(EquipHand.Left_Hand))
                    {
                    }
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandMace, _playerID);
                    return true;
                }

                if (ApplyMaceForLeftHand(playerEquip)) return true;
                if (ApplyMaceForRightHand(playerEquip)) return true;
            }

            if (IsEquipSword(playerEquip))
			{
                //NO EQUIPMENT APPLY SWORD.
                if (IsEmptyHanded(EquipHand.Left_Hand) && IsEmptyHanded(EquipHand.Right_Hand))
                {
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandSword, _playerID);
                    return true;
                }

                //SHIELD IN LEFT HAND, EQUIP SWORD IN RIGHT HAND.
                if (IsShield(EquipHand.Left_Hand))
                {
                    if (IsEmptyHanded(EquipHand.Right_Hand))
                    {
                    }
                    _mostRecentEquipGained = _rightHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandSword, _playerID);
                    return true;
                }

                //SHIELD IN RIGHT HAND, EQUIP SWORD IN LEFT HAND.
                if (IsShield(EquipHand.Right_Hand))
                {
                    if (IsEmptyHanded(EquipHand.Left_Hand))
                    {
                    }
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandSword, _playerID);
                    return true;
                }

				if (ApplySwordForLeftHand(playerEquip)) return true;
				if (ApplySwordForRightHand(playerEquip)) return true;
			}

            if (IsEquipShield(playerEquip))
            {
                if (IsEmptyHanded(EquipHand.Left_Hand) && IsEmptyHanded(EquipHand.Right_Hand))
                {
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandShield, _playerID);
                    return true;
                }

                if (IsEmptyHanded(EquipHand.Left_Hand) && !IsShield(EquipHand.Right_Hand))
                {
                    _mostRecentEquipGained = _leftHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandShield, _playerID);
                    return true;
                }
                if (IsEmptyHanded(EquipHand.Right_Hand) && !IsShield(EquipHand.Left_Hand))
                {
                    _mostRecentEquipGained = _rightHandEquip = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandShield, _playerID);

                }
                if (ApplyShieldForLeftHand(playerEquip)) return true;
                if (ApplyShieldForRightHand(playerEquip)) return true;
            }

            if (IsEquipArmor(playerEquip))
			{
                if (!IsArmor())
                {
                    _mostRecentEquipGained = _torsoEquipment = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UITorsoEquip, _playerID);
                    return true;
                }
                else
                {
                    //if(IsArmorBetterThanCurrent(playerEquip))
                    _mostRecentEquipGained = _torsoEquipment = playerEquip;
                    UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UITorsoEquip, _playerID);
                    return true;
                }
            }

            return false;
        }

        private bool ApplySwordForLeftHand(ItemBase playerEquip)
		{
            if (IsSword(EquipHand.Left_Hand))
            {
            }
            _mostRecentEquipGained = _leftHandEquip = playerEquip;
            UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandSword, _playerID);
            return true;
            //return false;
        }
        private bool ApplySwordForRightHand(ItemBase playerEquip)
        {
            if (IsSword(EquipHand.Right_Hand))
            {
            }
            _mostRecentEquipGained = _rightHandEquip = playerEquip;
            UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandSword, _playerID);
            return true;
            //return false;
        }
        

        private bool ApplyShieldForLeftHand(ItemBase playerEquip)
        {
            if (IsShield(EquipHand.Left_Hand))
            {
                //if (IsShieldBetterThanCurrent(playerEquip, EquipHand.Left_Hand))
                _mostRecentEquipGained = _leftHandEquip = playerEquip;
                UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandShield, _playerID);
                return true;
            }
            return false;
        }
        private bool ApplyShieldForRightHand(ItemBase playerEquip)
        {
            if (IsShield(EquipHand.Right_Hand))
            {
                //if (IsShieldBetterThanCurrent(playerEquip, EquipHand.Right_Hand))
                _mostRecentEquipGained = _rightHandEquip = playerEquip;
                UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandShield, _playerID);
                return true;
            }
            return false;
        }


        private bool ApplyMaceForLeftHand(ItemBase playerEquip)
        {
            if (IsMace(EquipHand.Left_Hand))
            {
            }
            _mostRecentEquipGained = _leftHandEquip = playerEquip;
            UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UILeftHandMace, _playerID);
            return true;
            //return false;
        }
        private bool ApplyMaceForRightHand(ItemBase playerEquip)
        {
            if (IsMace(EquipHand.Right_Hand))
            {
            }
            _mostRecentEquipGained = _rightHandEquip = playerEquip;
            UIPlayerManager.TriggerEvent("ReportUIPlayerEquipEvent", UIAnimationFlag.UIRightHandMace, _playerID);
            return true;
            //return false;
        }




        private bool IsEquipSword(ItemBase equip)
		{
            if (equip.GetType() == typeof(Sword))
                return true;

            return false;
        }
        private bool IsEquipMace(ItemBase equip)
        {
            if (equip.GetType() == typeof(Mace))
                return true;

            return false;
        }
        private bool IsEquipShield(ItemBase equip)
        {
            if (equip.GetType() == typeof(Shield))
                return true;
            return false;
        }
        private bool IsEquipArmor(ItemBase equip)
        {
            if (equip.GetType() == typeof(Breastplate))
                return true;
            return false;
        }

        
        public ItemBase GetTorsoEquip()
		{
            return _torsoEquipment;
		}
        public ItemBase GetLeftHandEquip()
        {
            return _leftHandEquip;
        }
        public ItemBase GetRightHandEquip()
        {
            return _rightHandEquip;
        }
        public ItemBase GetWeapon()
		{
            if (_leftHandEquip.GetType() == typeof(Sword) ||
                _leftHandEquip.GetType() == typeof(Mace))
                return _leftHandEquip;
            if (_rightHandEquip.GetType() == typeof(Sword) ||
                _rightHandEquip.GetType() == typeof(Mace))
                return _rightHandEquip;

            return null;// new EmptyHand(EquipmentWeaponType.None, EquipmentMaterialType.None);
        }

        public EquipmentMaterialType GetArmorMaterialType()
		{
            if (_torsoEquipment.GetType() == typeof(Breastplate))
            {
                Breastplate armor = (Breastplate)_torsoEquipment;
                return armor.GetMaterialType();
            }
            return EquipmentMaterialType.None;
        }
        public EquipmentMaterialType GetShieldMaterialType()
        {
            if (_leftHandEquip.GetType() == typeof(Shield))
            {
                Shield shield = (Shield)_leftHandEquip;
                return shield.GetMaterialType();
            }
            if (_rightHandEquip.GetType() == typeof(Shield))
            {
                Shield shield = (Shield)_rightHandEquip;
                return shield.GetMaterialType();
            }
            return EquipmentMaterialType.None;
        }
        public EquipmentMaterialType GetSwordMaterialType()
        {
            if (_leftHandEquip.GetType() == typeof(Sword))
            {
                Sword sword = (Sword)_leftHandEquip;
                return sword.GetMaterialType();
            }
            if (_rightHandEquip.GetType() == typeof(Sword))
            {
                Sword sword = (Sword)_rightHandEquip;
                return sword.GetMaterialType();
            }
            return EquipmentMaterialType.None;
        }
        public EquipmentMaterialType GetMaceMaterialType()
        {
            if (_leftHandEquip.GetType() == typeof(Mace))
            {
                Mace Mace = (Mace)_leftHandEquip;
                return Mace.GetMaterialType();
            }
            if (_rightHandEquip.GetType() == typeof(Mace))
            {
                Mace Mace = (Mace)_rightHandEquip;
                return Mace.GetMaterialType();
            }
            return EquipmentMaterialType.None;
        }

        public bool IsSword(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip.GetType() == typeof(Sword))
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Sword))
                    return true;
            }

            return false;
        }
        public bool IsMace(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip.GetType() == typeof(Mace))
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Mace))
                    return true;
            }

            return false;
        }
        public bool IsBroadsword(EquipHand equipHand)
		{
            if(equipHand == EquipHand.Left_Hand)
			{
                
                if (_leftHandEquip.GetType() == typeof(Sword))
				{
                    ISword sword = (Sword)_leftHandEquip;
                    if(sword.GetSwordType() == EquipmentWeaponType.Broadsword)
                        return true;
                }

            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Sword))
                {
                    ISword sword = (Sword)_rightHandEquip;
                    if (sword.GetSwordType() == EquipmentWeaponType.Broadsword)
                        return true;
                }
            }

            return false;
		}
        public bool IsMorningstarMace(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {

                if (_leftHandEquip.GetType() == typeof(Mace))
                {
                    Mace Mace = (Mace)_leftHandEquip;
                    if (Mace.GetMaceType() == EquipmentWeaponType.MorningstarMace)
                        return true;
                }

            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Mace))
                {
                    Mace Mace = (Mace)_rightHandEquip;
                    if (Mace.GetMaceType() == EquipmentWeaponType.MorningstarMace)
                        return true;
                }
            }

            return false;
        }
        public bool IsLongsword(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip.GetType() == typeof(Sword))
                {
                    ISword sword = (Sword)_leftHandEquip;
                    if (sword.GetSwordType() == EquipmentWeaponType.Longsword)
                        return true;
                }
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Sword))
                {
                    ISword sword = (Sword)_rightHandEquip;
                    if (sword.GetSwordType() == EquipmentWeaponType.Longsword)
                        return true;
                }
            }
            return false;
        }
        public bool IsShield(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip.GetType() == typeof(Shield))
                {
                    Shield shield = (Shield)_leftHandEquip;
                    if (shield.GetShieldType() == EquipmentArmorType.Shield)
                        return true;
                }
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(Shield))
                {
                    Shield shield = (Shield)_rightHandEquip;
                    if (shield.GetShieldType() == EquipmentArmorType.Shield)
                        return true;
                }
            }
            return false;
        }
        public bool IsArmor()
        {
            if (_torsoEquipment.GetType() == typeof(Breastplate))
			{
                Breastplate armor = (Breastplate)_torsoEquipment;
                if (armor.GetArmorType() == EquipmentArmorType.Breastplate)
                    return true;
			}

            return false;
        }
        public bool IsEmptyHanded(EquipHand equipHand)//bool isLeftHand)

        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == null)//.GetType() == typeof(ItemBase))
                    return true;
                if (_leftHandEquip.GetType() == typeof(EmptyHand))
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == null)//.GetType() == typeof(ItemBase))
                    return true;
                if (_rightHandEquip.GetType() == typeof(EmptyHand))
                    return true;
            }

            return false;
		}

        public bool IsSwordBetterThanCurrent(ItemBase equipToApply, EquipHand equipHand)
		{
            Sword swordToApply = (Sword)equipToApply;
            Sword currentSword;

            if (equipHand == EquipHand.Left_Hand)
			{
                currentSword = (Sword)_leftHandEquip;
                return _equipmentChecker.CompareSwords(ref swordToApply, ref currentSword);
            }
            if (equipHand == EquipHand.Right_Hand)
			{
                currentSword = (Sword)_rightHandEquip;
                return _equipmentChecker.CompareSwords(ref swordToApply, ref currentSword);
            }

            return false;
		}
        public bool IsShieldBetterThanCurrent(ItemBase equipToApply, EquipHand equipHand)
        {
            Shield shieldToApply = (Shield)equipToApply;
            Shield currentShield;

            if (equipHand == EquipHand.Left_Hand)
            {
                currentShield = (Shield)_leftHandEquip;
                return _equipmentChecker.CompareShields(ref shieldToApply, ref currentShield);
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                currentShield = (Shield)_rightHandEquip;
                return _equipmentChecker.CompareShields(ref shieldToApply, ref currentShield);
            }

            return false;
        }
        public bool IsArmorBetterThanCurrent(ItemBase equipToApply)
		{
            Breastplate armorToApply = (Breastplate)equipToApply;
            Breastplate currentArmor = (Breastplate)_torsoEquipment;

            return _equipmentChecker.CompareArmors(ref armorToApply, ref currentArmor);
		}

    }
}

