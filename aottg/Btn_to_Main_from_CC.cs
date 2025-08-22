using BRM;
using UnityEngine;

public class Btn_to_Main_from_CC : MonoBehaviour
{
	private void OnClick()
	{
		PhotonNetwork.Disconnect();
		Screen.lockCursor = false;
		Screen.showCursor = true;
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		FengGameManagerMKII.gameStart = false;
		CacheGameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;
		Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
		Application.LoadLevel("menu");
	}
}
