using UnityEngine;

public class LevelTriggerGas : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (!(other.gameObject.tag == "Player"))
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			other.gameObject.GetComponent<HERO>().fillGas();
			Object.Destroy(base.gameObject);
		}
		else if (other.gameObject.GetComponent<HERO>().photonView.isMine)
		{
			other.gameObject.GetComponent<HERO>().fillGas();
			Object.Destroy(base.gameObject);
			if (!InRoomChat.ChatInstanse.IsVisible)
			{
				InRoomChat.ChatInstanse.IsVisible = true;
			}
			InRoomChat.ChatInstanse.timeOfFade.Reset();
			InRoomChat.ChatInstanse.timeOfFade.Start();
		}
	}

	private void Start()
	{
	}
}
