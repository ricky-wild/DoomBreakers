
namespace DoomBreakers
{
	public interface IEnemyStateMachine //: MonoBehaviour
	{

		void SetEnemyState(state setTheState);
		state GetEnemyState();

		//void UpdatePlayerStateBehaviours();
	}
}

