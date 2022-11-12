

namespace DoomBreakers
{
    public interface IPlayerEquipment
	{
		//void ApplyEquipment(PlayerEquipType playerEquip, PlayerEquip equip);
		void ApplySword(PlayerEquipType playerEquip, PlayerEquip equip);
		void ApplyShield(PlayerEquipType playerEquip, PlayerEquip equip);
		void ApplyArmor(PlayerEquipType playerEquip, PlayerEquip equip);
		PlayerEquipType GetTorsoEquip();
		PlayerEquipType GetLeftHandEquip();
		PlayerEquipType GetRightHandEquip();

		bool IsBroadsword(EquipHand equipHand);//bool isLeftHand);
		bool IsLongsword(EquipHand equipHand);//bool isLeftHand);
		bool IsShield(EquipHand equipHand);//bool isLeftHand);
		bool IsArmor();
		bool IsEmptyHanded(EquipHand equipHand);//bool isLeftHand);
	}
}
