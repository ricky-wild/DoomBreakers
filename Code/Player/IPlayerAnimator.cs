
namespace DoomBreakers
{
	interface IPlayerAnimator //: MonoBehaviour
	{
		void UpdateAnimator();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(AnimatorController animatorController, bool withArmor);
		//void SetAnimationState(string animationState);
	}
}

