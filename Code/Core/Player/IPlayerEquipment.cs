

namespace DoomBreakers
{
    public interface IPlayerEquipment
	{
		PlayerEquip GetTorsoEquip();
		PlayerEquip GetLeftHandEquip();
		PlayerEquip GetRightHandEquip();

		bool IsBroadsword(EquipHand equipHand);//bool isLeftHand);
		bool IsLongsword(EquipHand equipHand);//bool isLeftHand);
		bool IsShield(EquipHand equipHand);//bool isLeftHand);
		bool IsArmor();
		bool IsEmptyHanded(EquipHand equipHand);//bool isLeftHand);
	}
}
