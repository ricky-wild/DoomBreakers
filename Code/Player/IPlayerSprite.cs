using UnityEngine;
namespace DoomBreakers
{
	interface IPlayerSprite
	{
		void SetupTexture2DColorSwap();
		void ApplyCustomTexture2DColours();
		void SetTexture2DColor(Color color);
	}
}

