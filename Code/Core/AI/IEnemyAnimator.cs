
namespace DoomBreakers
{
	interface IEnemyAnimator //: MonoBehaviour
	{
		void UpdateAnimator();
		AnimationState GetAnimationState();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(BanditAnimatorController animatorController);

	}
}

