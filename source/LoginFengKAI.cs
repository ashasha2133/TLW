using UnityEngine;

public class LoginFengKAI : MonoBehaviour
{
	public static PlayerInfoPHOTON player;

	private static string playerGUILDName = string.Empty;

	private static string playerName = string.Empty;

	private void Start()
	{
		if (player == null)
		{
			player = new PlayerInfoPHOTON();
			player.initAsGuest();
		}
	}
}
