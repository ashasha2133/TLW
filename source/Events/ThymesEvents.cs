using CustomAudio;
using UnityEngine;
using Utility;

namespace Events;

public static class ThymesEvents
{
	public static void OnBombKillPlayAudio(string eventName)
	{
		if (AudioListener.volume > 0f)
		{
			float x = Mathf.Clamp(IN_GAME_MAIN_CAMERA.main_objectR.velocity.magnitude / 100f, 0f, 8f);
			float pitch = ThymesUtils.Map(x, 0f, 8f, 1f, 2f);
			float volume = ThymesUtils.Map(x, 0f, 8f, 0.2f, 0.6f);
			CustomAudioManager.PlayDynamic(CustomAudioManager.AudioFile.BombKill, pitch, volume);
		}
	}

	public static void OnBombShotPlayAudio(string eventName)
	{
		if (AudioListener.volume > 0f)
		{
			CustomAudioManager.Play(CustomAudioManager.AudioFile.BombFlying);
		}
	}
}
