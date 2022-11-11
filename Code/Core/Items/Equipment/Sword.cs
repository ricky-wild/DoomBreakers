using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoomBreakers
{
    public class Sword : ItemBase
    {
		public override void Awake()
		{
			Initialize(this.GetComponent<Animator>(), AnimatorController.Weapon_equipment_to_pickup, AnimationState.IdleSword);
		}
		public override void Initialize(Animator animator, AnimatorController animController, AnimationState animationState)
		{
			base.Initialize(animator, animController, animationState);
		}
		public override void Start()
		{
			base.Start();
		}
		public override void Update()
		{
			base.Update();
		}

    }
}

