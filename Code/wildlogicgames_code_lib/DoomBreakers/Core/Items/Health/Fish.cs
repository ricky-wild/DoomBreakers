using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Fish : ItemBase
	{

		private double _healingValue;

		public double Health() => _healingValue;
		public Fish() { }
		public override void Awake() => Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), HealingItemType.Fish);

		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, HealingItemType healingItemType)
		{
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "HealthItems", "Health", HealingItemType.Fish);

			_healingValue = 0.125;
		}
		public override void Start() => base.Start();
		public override void Update() => base.Update();
	}
}
