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
    }
}

