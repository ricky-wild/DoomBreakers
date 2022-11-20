
using UnityEngine;

namespace DoomBreakers
{
    public class ShieldSprite : Sprite, IEquipmentSprite
    {
        private int _itemId;
        //private Texture2D _colorSwapTexture2D;
        public void Setup(ref SpriteRenderer spriteRenderer, int itemID, PlayerItem itemType, PlayerEquipType playerEquipType)
        {
            _spriteRenderer = spriteRenderer;
            //base.Setup(spriteRenderer, "_SwapTexBandit", _itemId);
            _itemId = itemID;
            ApplyCustomTexture2DColours(itemType, playerEquipType);
        }
        public override void SetupTexture2DColorSwap(string texName, int texId)
        {
            base.SetupTexture2DColorSwap(texName, texId);
        }
        public override void ResetTexture2DColor()
        {
            base.ResetTexture2DColor();
        }
        public void ApplyCustomTexture2DColours(PlayerItem itemType, PlayerEquipType playerEquipType)//IItem item)
        {
            SetupTexture2DColorSwap("_SwapTexBandit", _itemId);

            if (itemType != PlayerItem.IsShield)
                return;

            ApplyShieldColours(playerEquipType);

            //_colorSwapTexture2D.Apply();
        } 
        private void ApplyShieldColours(PlayerEquipType playerEquipType)
        {
            switch (playerEquipType)
            {
                case PlayerEquipType.Shield_Bronze:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xcbd092));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x9ca060));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x61512f));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x074f21));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x0e622c));
                    break;
                case PlayerEquipType.Shield_Iron:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xa8a8a8));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x7b7b7b));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x343434));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x4f1507));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x823c3d));
                    break;
                case PlayerEquipType.Shield_Steel:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0xcfc8c8));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0xb1a7a7));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x918686));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x212454));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x3c407e));
                    break;
                case PlayerEquipType.Shield_Ebony:
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_a, ColorFromInt(0x2f3146));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_b, ColorFromInt(0x272834));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_c, ColorFromInt(0x1e1717));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_d, ColorFromInt(0x121d28));
                    SwapTexture2DColor(SpriteColourIndex.Shield_Standard_e, ColorFromInt(0x3f464e));
                    break;
            }
            _colorSwapTexture2D.Apply();
        }
        public override void SetTexture2DColor(float time, Color color)
        {
            base.SetTexture2DColor(time, color);
        }

        public override void SwapTexture2DColor(SpriteColourIndex indexOfColourToSwap, Color replacementColor)
        {
            base.SwapTexture2DColor(indexOfColourToSwap, replacementColor);
        }

        public override Color ColorFromInt(int c, float alpha = 1.0f)
        {
            return base.ColorFromInt(c, alpha);
        }

        public override Color ColorFromIntRGB(int r, int g, int b)
        {
            return base.ColorFromIntRGB(r, g, b);
        }

        void Start() { }

        void Update() { }
    }
}


