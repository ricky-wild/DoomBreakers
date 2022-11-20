using UnityEngine;
namespace DoomBreakers
{
	public interface IEquipmentSprite
	{
		void Setup(ref SpriteRenderer spriteRenderer, int itemID, PlayerItem itemType, EquipmentMaterialType equipmentMaterialType);
		void SetupTexture2DColorSwap(string texName, int texId);
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours(PlayerItem itemType, EquipmentMaterialType equipmentMaterialType);
		void SetTexture2DColor(float time, Color color);
	}
}