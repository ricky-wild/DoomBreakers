using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Chicken : ItemBase
	{

		private double _healingValue;

		public double Health() => _healingValue;
		public Chicken() { }
		public override void Awake() => Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), HealingItemType.Chicken);

		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, HealingItemType healingItemType)
		{
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "HealthItems", "Health", HealingItemType.Chicken);

			_healingValue = 0.1;
		}
		public override void Start() => base.Start();
		public override void Update() => base.Update();
	}
}
