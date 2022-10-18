
using System.Collections.Generic;
using UnityEngine;
using static DoomBreakers.PlayerInput;

namespace DoomBreakers
{
	interface IPlayerInput  //: MonoBehaviour
	{
		//void Setup(int playerID);
		void ResetInput();
		void UpdateInput();
		inputState GetInputState();
		Vector2 GetInputVector2();
		//Dictionary<inputState, bool> GetInputStates();
		//bool IsJumpPressed();
	}
}

