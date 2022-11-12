using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public enum PlayerEquip
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
        private PlayerEquip _torsoEquipment;
        private PlayerEquip _leftHandEquip;
        private PlayerEquip _rightHandEquip;

        public PlayerEquipment(PlayerEquip torsoEquipment, PlayerEquip leftHandEquip, PlayerEquip rightHandEquip)
		{
            _torsoEquipment = torsoEquipment;
            _leftHandEquip = leftHandEquip;
            _rightHandEquip = rightHandEquip;

        }

        public PlayerEquip GetTorsoEquip()
		{
            return _torsoEquipment;
		}
        public PlayerEquip GetLeftHandEquip()
        {
            return _leftHandEquip;
        }
        public PlayerEquip GetRightHandEquip()
        {
            return _rightHandEquip;
        }
        public bool IsBroadsword(EquipHand equipHand)
		{
            if(equipHand == EquipHand.Left_Hand)
			{
                if (_leftHandEquip == PlayerEquip.Broadsword_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquip.Broadsword_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquip.Broadsword_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquip.Broadsword_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquip.Broadsword_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquip.Broadsword_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquip.Broadsword_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquip.Broadsword_Ebony)
                    return true;
            }

            return false;
		}
        public bool IsLongsword(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquip.Longsword_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquip.Longsword_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquip.Longsword_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquip.Longsword_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquip.Longsword_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquip.Longsword_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquip.Longsword_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquip.Longsword_Ebony)
                    return true;
            }
            return false;
        }
        public bool IsShield(EquipHand equipHand)
        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquip.Shield_Bronze)
                    return true;
                if (_leftHandEquip == PlayerEquip.Shield_Iron)
                    return true;
                if (_leftHandEquip == PlayerEquip.Shield_Steel)
                    return true;
                if (_leftHandEquip == PlayerEquip.Shield_Ebony)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquip.Shield_Bronze)
                    return true;
                if (_rightHandEquip == PlayerEquip.Shield_Iron)
                    return true;
                if (_rightHandEquip == PlayerEquip.Shield_Steel)
                    return true;
                if (_rightHandEquip == PlayerEquip.Shield_Ebony)
                    return true;
            }
            return false;
        }
        public bool IsArmor()
        {
            if (_torsoEquipment == PlayerEquip.BreastPlate_Bronze)
                return true;
            if (_torsoEquipment == PlayerEquip.BreastPlate_Iron)
                return true;
            if (_torsoEquipment == PlayerEquip.BreastPlate_Steel)
                return true;
            if (_torsoEquipment == PlayerEquip.BreastPlate_Ebony)
                return true;
            return false;
        }
        public bool IsEmptyHanded(EquipHand equipHand)//bool isLeftHand)

        {
            if (equipHand == EquipHand.Left_Hand)
            {
                if (_leftHandEquip == PlayerEquip.Empty_None)// && _rightHandEquip == PlayerEquip.Empty_None)
                    return true;
            }
            if (equipHand == EquipHand.Right_Hand)
            {
                if (_rightHandEquip == PlayerEquip.Empty_None)
                    return true;
            }

            return false;
		}
    }
}

