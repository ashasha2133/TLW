using System;
using UnityEngine;

namespace CustomAudio;

internal class CustomAudio : ICustomAudio
{
	public AudioSource _source;

	public CustomAudio(AudioSource audio)
	{
		_source = audio;
	}

	public bool IsPlaying()
	{
		if (_source != null)
		{
			return _source.isPlaying;
		}
		return false;
	}

	public void Modify(float pitch, float volume)
	{
		if (_source != null)
		{
			_source.pitch = pitch;
			_source.volume = volume;
		}
	}

	public void Modify(Vector3 location)
	{
		throw new NotImplementedException();
	}

	public void Play()
	{
		if (_source != null)
		{
			_source.Play();
		}
	}

	public void Stop()
	{
		if (_source != null)
		{
			_source.Stop();
		}
	}
}
