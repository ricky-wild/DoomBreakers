using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Barrel : ContainerBase
	{
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), ContainerAnimatorController.Container_Barrel);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, ContainerAnimatorController animController)
		{
			base.Initialize(spriteRenderer, animator, animController);

			//Resources/ContainerAnimControllers/Barrel/Barrel.controller
			_containerAnimator = new ContainerAnimator(animator, "ContainerAnimControllers", "Barrel", "Barrel");
		}

		public void IsHit() => _containerAnimator.PlayAnimation(ContainerAnimationState.Container_Hit);

		public override void Update() => base.Update();
	}
}
