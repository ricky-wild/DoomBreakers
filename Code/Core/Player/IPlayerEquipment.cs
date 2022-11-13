

namespace DoomBreakers
{
    public interface IPlayerEquipment
	{
		//void ApplyEquipment(PlayerEquipType playerEquip, PlayerItem equip);
		void ApplySword(PlayerEquipType playerEquip, PlayerItem equip);
		void ApplyShield(PlayerEquipType playerEquip, PlayerItem equip);
		void ApplyArmor(PlayerEquipType playerEquip, PlayerItem equip);
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
