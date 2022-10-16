
namespace DoomBreakers
{
	public interface IPlayerStateMachine //: MonoBehaviour
	{

		void SetPlayerState(state setTheState);
		state GetPlayerState();
		
		//void UpdatePlayerStateBehaviours();
	}
}

