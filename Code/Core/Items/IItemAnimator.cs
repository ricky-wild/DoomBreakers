using System.Collections;

namespace DoomBreakers
{
    public interface IItemAnimator //: MonoBehaviour
    {
		void UpdateAnimator();
		void SetAnimationState(AnimationState animationState);
		void SetAnimatorController(AnimatorController animatorController);
	}
}

