﻿using System.Collections;
using UnityEngine;

namespace DoomBreakers
{
    public interface IItem //: MonoBehaviour
    {

        void Initialize(Animator animator, AnimatorController animController, AnimationState animationState);

        void Destroy();

        //void Start() { }

        //void Update() { }
    }
}
