
using UnityEngine;

namespace DoomBreakers
{
    public class BreastplateSprite : Sprite, IEquipmentSprite
    {
        private int _itemId;
        //private Texture2D _colorSwapTexture2D;
        public void Setup(ref SpriteRenderer spriteRenderer, int itemID, PlayerItem itemType, EquipmentMaterialType equipMaterialType)
        {
            _spriteRenderer = spriteRenderer;
            //base.Setup(spriteRenderer, "_SwapTexBandit", _itemId);
            _itemId = itemID;
            ApplyCustomTexture2DColours(itemType, equipMaterialType);
        }
        public override void SetupTexture2DColorSwap(string texName, int texId)
        {
            base.SetupTexture2DColorSwap(texName, texId);
        }
        public override void ResetTexture2DColor()
        {
            base.ResetTexture2DColor();
        }
        public void ApplyCustomTexture2DColours(PlayerItem itemType, EquipmentMaterialType equipMaterialType)//IItem item)
        {
            SetupTexture2DColorSwap("_SwapTexBandit", _itemId);

            if (itemType != PlayerItem.IsBreastPlate)
                return;

            ApplyArmorColours(equipMaterialType);

            //_colorSwapTexture2D.Apply();
        }
        private void ApplyArmorColours(EquipmentMaterialType equipMaterialType)
        {
            switch (equipMaterialType)
            {
                case EquipmentMaterialType.Bronze:
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0xbabf6e));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x9ca060));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x61512f));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x44371c));
                    break;
                case EquipmentMaterialType.Iron:
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0x858585));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x747474));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x535353));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x1e1e1e));
                    break;
                case EquipmentMaterialType.Steel:
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0xbebebe));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0xa8a8a8));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x7b7b7b));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x343434));
                    break;
                case EquipmentMaterialType.Ebony:
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_c, ColorFromInt(0x494e64));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_f, ColorFromInt(0x383636));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_g, ColorFromInt(0x302d2d));
                    SwapTexture2DColor(SpriteColourIndex.Armor_Standard_h, ColorFromInt(0x201d1d));
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


