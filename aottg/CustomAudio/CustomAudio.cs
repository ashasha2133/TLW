using System;
using UnityEngine;

namespace CustomAudio
{
	internal class CustomAudio : ICustomAudio
	{
		public AudioSource _source;

		public CustomAudio(AudioSource audio)
		{
			_source = audio;
		}

		public bool IsPlaying()
		{
			return _source.isPlaying;
		}

		public void Modify(float pitch, float volume)
		{
			_source.pitch = pitch;
			_source.volume = volume;
		}

		public void Modify(Vector3 location)
		{
			throw new NotImplementedException();
		}

		public void Play()
		{
			_source.Play();
		}

		public void Stop()
		{
			_source.Stop();
		}
	}
}
