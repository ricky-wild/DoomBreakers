using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum PlayerEquip
	{
        Empty_None = 0,
        IsBroadsword = 1,
        IsLongsword = 2,
        IsShield = 3,
        IsBreastPlate = 4
	};
    public enum PlayerEquipType
	{
        Empty_None = 0,

        Broadsword_Bronze = 1,
        Broadsword_Iron = 2,
        Broadsword_Steel = 3,
        Broadsword_Ebony = 4,
        Longsword_Bronze = 5,
        Longsword_Iron = 6,
        Longsword_Steel = 7,
        Longsword_Ebony = 8,

        Shield_Bronze = 9,
        Shield_Iron = 10,
        Shield_Steel = 11,
        Shield_Ebony = 12,

        BreastPlate_Bronze = 13,
        BreastPlate_Iron = 14,
        BreastPlate_Steel = 15,
        BreastPlate_Ebony = 16
    };
    public enum EquipHand
	{
        Left_Hand = 1,
        Right_Hand = 2
	};
    public class PlayerEquipment : IPlayerEquipment//MonoBehaviour
    {
        private PlayerEquipType _torsoEquipment;
        private PlayerEquipType _leftHandEquip;
        private PlayerEquipType _rightHandEquip;

        public PlayerEquipment(PlayerEquipType torsoEquipment, PlayerEquipType leftHandEquip, PlayerEquipType rightHandEquip)
		{
            _torsoEquipment = torsoEquipment;
            _leftHandEquip = leftHandEquip;
            _rightHandEquip = rightHandEquip;

        }

        public void ApplySword(PlayerEquipType playerEquip, PlayerEquip equip)
		{
            //Now determine where we apply this equipment.

            if (!IsEquipSword(equip))
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
        public void ApplyShield(PlayerEquipType playerEquip, PlayerEquip equip)
		{
            if (!IsEquipShield(equip))
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
                _leftHandEquip = playerEquip;
                return;
            }
        }
        public void ApplyArmor(PlayerEquipType playerEquip, PlayerEquip equip)
		{
            if (!IsEquipArmor(equip))
                return;

            if(!IsArmor())
			{
                _torsoEquipment = playerEquip;
                return;
			}
        }
        private bool IsEquipSword(PlayerEquip equip)
		{
            if (equip == PlayerEquip.IsBroadsword)
                return true;
            if (equip == PlayerEquip.IsLongsword)
                return true;
            return false;
        }
        private bool IsEquipShield(PlayerEquip equip)
        {
            if (equip == PlayerEquip.IsShield)
                return true;
            return false;
        }
        private bool IsEquipArmor(PlayerEquip equip)
        {
            if (equip == PlayerEquip.IsBreastPlate)
                return true;
            return false;
        }

        public PlayerEquipType GetTorsoEquip()
		{
            return _torsoEquipment;
		}
        public PlayerEquipType GetLeftHandEquip()
        {
            return _leftHandEquip;
        }
        public PlayerEquipType GetRightHandEquip()
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

