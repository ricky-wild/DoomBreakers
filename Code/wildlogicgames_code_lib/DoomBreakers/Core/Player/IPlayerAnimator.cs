
namespace DoomBreakers
{
	interface IPlayerAnimator //: MonoBehaviour
	{
		void SetAnimatorController(ref IPlayerEquipment playerEquipment);
		void PlayIndicatorAnimation(IndicatorAnimID indicatorAnim);
	}
}

