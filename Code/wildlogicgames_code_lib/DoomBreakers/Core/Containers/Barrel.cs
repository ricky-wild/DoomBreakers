using UnityEngine;
using wildlogicgames;

namespace DoomBreakers
{
	public class Barrel : ContainerBase
	{
		private bool _destroyFlag;
		public override void Awake()
		{
			Initialize(this.GetComponent<SpriteRenderer>(), this.GetComponent<Animator>(), ContainerAnimatorController.Container_Barrel);
		}
		public override void Initialize(SpriteRenderer spriteRenderer, Animator animator, ContainerAnimatorController animController)
		{
			base.Initialize(spriteRenderer, animator, animController);

			//Resources/ContainerAnimControllers/Barrel/Barrel.controller
			_containerAnimator = new ContainerAnimator(animator, "ContainerAnimControllers", "Barrel", "Barrel");
			_timer = this.gameObject.AddComponent<Timer>();
			_timer.Setup("Barrel Timer");
			_destroyFlag = false;
		}

		public void IsHit()// => _containerAnimator.PlayAnimation(ContainerAnimationState.Container_Hit);
		{
			_containerHealth -= 1;

			if(_containerHealth > 0)
			{
				_containerBehaviour.IsHit();
				AudioEventManager.PlayPropSFX(PropSFXID.PropHitSFX);
				_containerAnimator.PlayAnimation(ContainerAnimationState.Container_Hit);
				_timer.StartTimer(0.2f);
			}
			else
			{
				if (_destroyFlag) return;
				AudioEventManager.PlayPropSFX(PropSFXID.PropDestroySFX);
				_containerAnimator.PlayAnimation(ContainerAnimationState.Container_Destroy);
				_timer.Reset();
				_timer.StartTimer(1.0f);
				_destroyFlag = true;
			}

		}

		public override void Update()// => base.Update();
		{
			base.Update();

			_containerBehaviour.UpdateContainerMovement();

			if(_destroyFlag) _containerAnimator.PlayAnimation(ContainerAnimationState.Container_Destroy);

			if (_timer.HasTimerFinished())
			{
				if (_containerHealth > 0 && !_destroyFlag) _containerAnimator.PlayAnimation(ContainerAnimationState.Container_Idle);
				else
				{
					Destroy();
				}
			}
		}
	}
}
