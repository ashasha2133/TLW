using UnityEngine;

namespace CustomAudio
{
	internal class Custom3DAudio : CustomAudio, ICustomAudio
	{
		public Vector3 _location;

		public Custom3DAudio(AudioSource audio, Vector3 location)
			: base(audio)
		{
			_location = location;
		}

		public void Modify(float pitch, float volume, Vector3 location)
		{
			Modify(pitch, volume);
			_location = location;
		}

		public new void Modify(Vector3 location)
		{
			_location = location;
		}

		public new void Play()
		{
			AudioSource.PlayClipAtPoint(_source.clip, _location);
		}
	}
}
