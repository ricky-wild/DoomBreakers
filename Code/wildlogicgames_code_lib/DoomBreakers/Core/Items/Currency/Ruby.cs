using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Ruby : ItemBase
	{

		private int _currencyValue;

		public int Amount() => _currencyValue;
		public Ruby() { }
		public override void Awake() => Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), CurrencyItemType.Ruby);

		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, CurrencyItemType currencyItemType)
		{
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Currency", "Currency", CurrencyItemType.Ruby);

			_currencyValue = 3;
		}
		public override void Start() => base.Start();
		public override void Update() => base.Update();
	}
}
