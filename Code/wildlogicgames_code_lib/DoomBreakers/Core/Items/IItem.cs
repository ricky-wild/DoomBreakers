using System.Collections;
using UnityEngine;

namespace DoomBreakers
{
    public interface IItem //: MonoBehaviour
    {

        void Initialize(SpriteRenderer spriteRenderer, Animator animator, AnimatorController animController,
                                       AnimationState animationState, PlayerItem itemType, EquipmentMaterialType equipMaterialType);

        void Destroy();

        //void Start() { }

        //void Update() { }
    }
}

