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
        public bool IsBroadsword(EquipHand equipHand)
		{
            if(equipHand == EquipHand.Left_Hand)
			{
                if (_leftHandEquip == PlayerEquipType.Broadsword_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Broadsword_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Broadsword_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Broadsword_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquipType.Broadsword_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Broadsword_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Broadsword_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Broadsword_Ebony)
                    return true;
            }

            return false;
		}
        public bool IsLongsword(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquipType.Longsword_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Longsword_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Longsword_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Longsword_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquipType.Longsword_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Longsword_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Longsword_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Longsword_Ebony)
                    return true;
            }
            return false;
        }
        public bool IsShield(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquipType.Shield_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Shield_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Shield_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquipType.Shield_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquipType.Shield_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Shield_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Shield_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquipType.Shield_Ebony)
                    return true;
            }
            return false;
        }
        public bool IsArmor()
        {
            if (_torsoEquipment == PlayerEquipType.BreastPlate_Bronze)
                return true;
            if (_torsoEquipment == PlayerEquipType.BreastPlate_Iron)
                return true;
            if (_torsoEquipment == PlayerEquipType.BreastPlate_Steel)
                return true;
            if (_torsoEquipment == PlayerEquipType.BreastPlate_Ebony)
                return true;
            return false;
        }
        public bool IsEmptyHanded(EquipHand equipHand)//bool isLeftHand)

        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquipType.Empty_None)// && _rightHandEquip == PlayerEquipType.Empty_None)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquipType.Empty_None)
                    return true;
            }

            return false;
		}
    }
}

