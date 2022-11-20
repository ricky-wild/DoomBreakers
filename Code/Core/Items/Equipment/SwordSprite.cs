
using UnityEngine;

namespace DoomBreakers
{
    public class SwordSprite : Sprite, IEquipmentSprite
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

            if (itemType != PlayerItem.IsBroadsword || itemType != PlayerItem.IsLongsword)
                return;

            ApplySwordColours(playerEquipType);

            //_colorSwapTexture2D.Apply();
        }
        private void ApplySwordColours(PlayerEquipType playerEquipType)
        {
            switch (playerEquipType)
            {
                case PlayerEquipType.Broadsword_Bronze:
                case PlayerEquipType.Longsword_Bronze:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xe4e6c9));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xd3d6ab));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0xcbd092));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0xb5b977));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x9ca060));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x61512f));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x6c5a34));
                    break;
                case PlayerEquipType.Broadsword_Iron:
                case PlayerEquipType.Longsword_Iron:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xafafaf));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xa3a2a2));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0x939191));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0x8b8b8b));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x7c7979));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x144c18));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x1a5a1f));
                    break;
                case PlayerEquipType.Broadsword_Steel:
                case PlayerEquipType.Longsword_Steel:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0xd5d5d5));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0xcbc5c5));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0xb9b9b9));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0xb3afaf));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x9f9a9a));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x0f1c4e));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x182964));
                    break;
                case PlayerEquipType.Broadsword_Ebony:
                case PlayerEquipType.Longsword_Ebony:
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_a, ColorFromInt(0x565965));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_b, ColorFromInt(0x424141));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_c, ColorFromInt(0x383636));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_d, ColorFromInt(0x302d2d));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_e, ColorFromInt(0x201d1d));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_a, ColorFromInt(0x4c1212));
                    SwapTexture2DColor(SpriteColourIndex.Sword_Standard_handle_b, ColorFromInt(0x601c1c));
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


