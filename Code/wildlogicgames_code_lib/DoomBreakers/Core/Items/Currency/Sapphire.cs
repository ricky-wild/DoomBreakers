using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Sapphire : ItemBase
	{

		private int _currencyValue;

		public int Amount() => _currencyValue;
		public Sapphire() { }
		public override void Awake() => Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), CurrencyItemType.Sapphire);

		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, CurrencyItemType currencyItemType)
		{
			_itemBehaviour = this.gameObject.AddComponent<ItemBehaviour>();
			_itemBehaviour.Setup(this.transform, this.GetComponent<CharacterController2D>(), this.GetComponent<BoxCollider2D>());

			_itemAnimator = new ItemAnimator(animator, "ItemAnimControllers", "Currency", "Currency", CurrencyItemType.Sapphire);

			_currencyValue = 5;
		}
		public override void Start() => base.Start();
		public override void Update() => base.Update();
	}
}
