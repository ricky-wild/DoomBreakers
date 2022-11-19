using UnityEngine;
namespace DoomBreakers
{
	public interface IPlayerSprite
	{
		void Setup(SpriteRenderer spriteRenderer, int playerID);
		int GetSpriteDirection();
		WeaponChargeHold GetWeaponTexChargeFlag();
		void FlipSprite();
		void SetupTexture2DColorSwap(string texName, int texId);
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
		void SetBehaviourTextureFlash(float time, Color colour);
		void SetWeaponChargeTextureFXFlag(bool b);
		void SetNewEquipmemtTextureColorFlag(bool b, IPlayerEquipment playerEquipment);
	}
}

