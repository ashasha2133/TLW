using UnityEngine;

namespace CustomAudio
{
	internal interface ICustomAudio
	{
		void Modify(float pitch, float volume);

		void Modify(Vector3 location);

		void Play();

		void Stop();

		bool IsPlaying();
	}
}
