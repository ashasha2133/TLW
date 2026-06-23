using UnityEngine;

namespace CustomAudio;

internal class CustomAlternatingAudio : ICustomAudio
{
	public ICustomAudio _sourceA;

	public ICustomAudio _sourceB;

	protected bool _playingA;

	public CustomAlternatingAudio(ICustomAudio sourceA, ICustomAudio sourceB)
	{
		_sourceA = sourceA;
		_sourceB = sourceB;
		_playingA = false;
	}

	public bool IsPlaying()
	{
		if (!_sourceA.IsPlaying())
		{
			return _sourceB.IsPlaying();
		}
		return true;
	}

	public void Modify(float pitch, float volume)
	{
		_sourceA.Modify(pitch, volume);
		_sourceB.Modify(pitch, volume);
	}

	public void Modify(Vector3 location)
	{
		_sourceA.Modify(location);
		_sourceB.Modify(location);
	}

	public void Play()
	{
		if (_playingA)
		{
			_sourceA.Play();
		}
		else
		{
			_sourceB.Play();
		}
		_playingA = !_playingA;
	}

	public void Stop()
	{
		if (_sourceA.IsPlaying())
		{
			_sourceA.Stop();
		}
		if (_sourceB.IsPlaying())
		{
			_sourceB.Stop();
		}
	}
}
