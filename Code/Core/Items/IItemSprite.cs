using UnityEngine;
namespace DoomBreakers
{
	public interface IItemSprite
	{
		void Setup(SpriteRenderer spriteRenderer, int itemID);
		void SetupTexture2DColorSwap(int texId);
		void ResetTexture2DColor();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
	}
}