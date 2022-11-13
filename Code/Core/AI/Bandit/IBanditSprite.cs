using UnityEngine;
namespace DoomBreakers
{
	public interface IBanditSprite
	{
		void Setup(SpriteRenderer spriteRenderer, int playerID);
		int GetSpriteDirection();
		void FlipSprite();
		void SetupTexture2DColorSwap(string texName, int texId);
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
		void SetWeaponChargeTextureFXFlag(bool b);
	}
}

