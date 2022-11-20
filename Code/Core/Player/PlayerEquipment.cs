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
        IsShield = 3,
        IsBreastPlate = 4
	};

    public enum EquipHand
	{
        Left_Hand = 1,
        Right_Hand = 2
	};
    public class PlayerEquipment : IPlayerEquipment//MonoBehaviour
    {
        private ItemBase _torsoEquipment;
        private ItemBase _leftHandEquip;
        private ItemBase _rightHandEquip;

        public PlayerEquipment(ItemBase torsoEquipment, ItemBase leftHandEquip, ItemBase rightHandEquip)
		{

            _torsoEquipment = torsoEquipment;
            _leftHandEquip = leftHandEquip;
            _rightHandEquip = rightHandEquip;

        }

        public void ApplySword(ItemBase playerEquip)
		{
            //Now determine where we apply this equipment.

            if (!IsEquipSword(playerEquip))
                return;


            if (IsBroadsword(EquipHand.Left_Hand))          //No Dual Weapons
                return;
            if (IsBroadsword(EquipHand.Right_Hand))         //No Dual Weapons
                return;
            if (IsLongsword(EquipHand.Left_Hand))           //No Dual Weapons
                return;
            if (IsLongsword(EquipHand.Right_Hand))          //No Dual Weapons
                return;

            if (IsEmptyHanded(EquipHand.Left_Hand) && IsEmptyHanded(EquipHand.Right_Hand))
            {
                _leftHandEquip = playerEquip;
                return;
            }
            if (IsShield(EquipHand.Left_Hand))
			{
                _rightHandEquip = playerEquip;
                return;
			}
            if (IsShield(EquipHand.Right_Hand))
            {
                _leftHandEquip = playerEquip;
                return;
            }

            //if (IsEquipShield(equip)) { }
            //if (IsEquipArmor(equip)) { }
        }
        public void ApplyShield(ItemBase playerEquip)
		{
            if (!IsEquipShield(playerEquip))
                return;

            if (IsShield(EquipHand.Left_Hand))          //No Dual Shields
                return;
            if (IsShield(EquipHand.Right_Hand))         //No Dual Shields
                return;

            if (IsEmptyHanded(EquipHand.Left_Hand) && IsEmptyHanded(EquipHand.Right_Hand))
            {
                _leftHandEquip = playerEquip;
                return;
            }

            if (IsEmptyHanded(EquipHand.Left_Hand) && !IsShield(EquipHand.Right_Hand))
            {
                _leftHandEquip = playerEquip;
                return;
            }
            if (IsEmptyHanded(EquipHand.Right_Hand) && !IsShield(EquipHand.Left_Hand))
            {
                _rightHandEquip = playerEquip;
                return;
            }
        }
        public void ApplyArmor(ItemBase playerEquip)
		{
            if (!IsEquipArmor(playerEquip))
                return;

            if(!IsArmor())
			{
                _torsoEquipment = playerEquip;
                return;
			}
        }
        private bool IsEquipSword(ItemBase equip)
		{
            if (equip.GetType() == typeof(Sword))
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
                if (_leftHandEquip.GetType() == typeof(ItemBase))
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip.GetType() == typeof(ItemBase))
                    return true;
            }

            return false;
		}
    }
}

