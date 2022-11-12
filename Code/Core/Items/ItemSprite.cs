
using UnityEngine;

namespace DoomBreakers
{
    public class ItemSprite : Sprite, IItemSprite
    {
        public void Setup(SpriteRenderer spriteRenderer, int itemID)
		{

		}
        public override void SetupTexture2DColorSwap(int texId)
        {
            base.SetupTexture2DColorSwap(texId);

        }
        public override void ResetTexture2DColor()
		{

		}
        public void ApplyCustomTexture2DColours()
		{

		}
        public override void SetTexture2DColor(Color color)
		{

		}

        void Start() { }

        void Update() { }
    }
}


