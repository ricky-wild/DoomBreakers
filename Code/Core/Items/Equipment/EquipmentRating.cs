using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
	//<Summary>
	//Class purpose is to assist witht he PlayerEquipment.cs by taking in various Equipment objs
	//and comparing their MaterialTypes() against one another, to return a bool that indicates
	//if one is better than the other.
	//We want to be able to only apply Equipment the Player collides with, that is better than
	//thier current. Otherwise ignore it.
	//</Summary>
	public class EquipmentRating
	{


		public EquipmentRating() { }

		public bool CompareSwords(ref Sword isThisSwordBetter, ref Sword thanThisSword)
		{
			if (isThisSwordBetter.GetMaterialType() == EquipmentMaterialType.Ebony &&
				thanThisSword.GetMaterialType() != EquipmentMaterialType.Ebony)
				return true;

			if (isThisSwordBetter.GetMaterialType() == EquipmentMaterialType.Steel &&
					thanThisSword.GetMaterialType() == EquipmentMaterialType.Iron ||
					thanThisSword.GetMaterialType() == EquipmentMaterialType.Bronze)
				return true;

			if (isThisSwordBetter.GetMaterialType() == EquipmentMaterialType.Iron &&
				thanThisSword.GetMaterialType() == EquipmentMaterialType.Bronze)
				return true;


			return false; //Return false will allow the player to aquire the sword.
		}

		public bool CompareShields(ref Shield isThisShieldBetter, ref Shield thanThisShield)
		{
			if (isThisShieldBetter.GetMaterialType() == EquipmentMaterialType.Ebony &&
				thanThisShield.GetMaterialType() != EquipmentMaterialType.Ebony)
				return true;

			if (isThisShieldBetter.GetMaterialType() == EquipmentMaterialType.Steel &&
					thanThisShield.GetMaterialType() == EquipmentMaterialType.Iron ||
					thanThisShield.GetMaterialType() == EquipmentMaterialType.Bronze)
				return true;

			if (isThisShieldBetter.GetMaterialType() == EquipmentMaterialType.Iron &&
				thanThisShield.GetMaterialType() == EquipmentMaterialType.Bronze)
				return true;


			return false; //Return false will allow the player to aquire the sword.
		}
	}
}
