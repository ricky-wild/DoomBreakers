using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Diamond : ItemBase
	{

		private int _currencyValue;

		public int Amount() => _currencyValue;
		public Diamond() { }
		public override void Awake() => Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), CurrencyItemType.Diamond);

		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, CurrencyItemType currencyItemType)
		{
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Currency", "Currency", CurrencyItemType.Diamond);

			_currencyValue = 10;
		}
		public override void Start() => base.Start();
		public override void Update() => base.Update();
	}
}
