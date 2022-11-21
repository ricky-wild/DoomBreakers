

namespace DoomBreakers
{
    public interface IPlayerEquipment
	{
		//void ApplyEquipment(PlayerEquipType playerEquip, PlayerItem equip);
		bool NewEquipmentGained();
		void NewEquipmentGained(bool flag);
		void ApplySword(ItemBase playerEquip);
		void ApplyShield(ItemBase playerEquip);
		void ApplyArmor(ItemBase playerEquip);
		ItemBase GetTorsoEquip();
		ItemBase GetLeftHandEquip();
		ItemBase GetRightHandEquip();

		EquipmentMaterialType GetArmorMaterialType();
		EquipmentMaterialType GetSwordMaterialType();
		EquipmentMaterialType GetShieldMaterialType();

		bool IsBroadsword(EquipHand equipHand);//bool isLeftHand);
		bool IsLongsword(EquipHand equipHand);//bool isLeftHand);
		bool IsShield(EquipHand equipHand);//bool isLeftHand);
		bool IsArmor();
		bool IsEmptyHanded(EquipHand equipHand);//bool isLeftHand);
	}
}
