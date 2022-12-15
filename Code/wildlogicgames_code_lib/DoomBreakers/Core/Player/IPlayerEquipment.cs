

namespace DoomBreakers
{
    public interface IPlayerEquipment
	{
		//void ApplyEquipment(PlayerEquipType playerEquip, PlayerItem equip);
		void RemoveArmor();
		bool NewEquipmentGained();
		void NewEquipmentGained(bool flag);
		bool ApplySword(ItemBase playerEquip);
		bool ApplyShield(ItemBase playerEquip);
		bool ApplyArmor(ItemBase playerEquip);
		ItemBase GetTorsoEquip();
		ItemBase GetLeftHandEquip();
		ItemBase GetRightHandEquip();
		ItemBase GetWeapon();
		ItemBase GetMostRecentEquipment();

		EquipmentMaterialType GetArmorMaterialType();
		EquipmentMaterialType GetSwordMaterialType();
		EquipmentMaterialType GetShieldMaterialType();

		bool IsBroadsword(EquipHand equipHand);//bool isLeftHand);
		bool IsLongsword(EquipHand equipHand);//bool isLeftHand);
		bool IsShield(EquipHand equipHand);//bool isLeftHand);
		bool IsArmor();
		bool IsEmptyHanded(EquipHand equipHand);//bool isLeftHand);
		//bool IsSwordBetterThanCurrent(Sword playerEquip);
	}
}
