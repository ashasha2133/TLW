using System.Collections.Generic;
using UnityEngine;

namespace CustomAudio
{
	public static class CustomAudioManager
	{
		public enum AudioFile
		{
			None,
			BombExplosion,
			BombKill,
			BombFlying,
			HookTitan,
			HookObject,
			GasBurst,
			BladesBroken,
			BladesReload,
			GunsShot,
			GunsReload,
			Refill,
			Flare,
			Walk,
			Gas,
			Slide
		}

		private static Dictionary<AudioFile, ICustomAudio> _playableAudio = new Dictionary<AudioFile, ICustomAudio>();

		public static AudioSource BladesReload;

		public static AudioSource Walk;

		public static void LoadAllSoundsForRelease(string myEventName)
		{
			AudioSource audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("bomb_explosion");
			audioSource.volume = 0.5f;
			LoadSound(AudioFile.BombExplosion, audioSource);
			AudioSource audioSource2 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource2.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("bomb_kill");
			LoadSound(AudioFile.BombKill, audioSource2);
			AudioSource audioSource3 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource3.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("bomb_flying");
			LoadSound(AudioFile.BombFlying, audioSource3);
			AudioSource audioSource4 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource4.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("hook_titan");
			LoadSound(AudioFile.HookTitan, audioSource4);
			AudioSource audioSource5 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource5.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("hook_object");
			audioSource5.pitch = 2f;
			LoadSound(AudioFile.HookObject, audioSource5);
			AudioSource audioSource6 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource6.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("gas_burst");
			LoadSound(AudioFile.GasBurst, audioSource6);
			AudioSource audioSource7 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource7.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("blade_broken");
			LoadSound(AudioFile.BladesBroken, audioSource7);
			BladesReload = Camera.main.gameObject.AddComponent<AudioSource>();
			BladesReload.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("blade_reload");
			LoadSound(AudioFile.BladesReload, BladesReload);
			AudioSource audioSource8 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource8.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("guns_shoot");
			LoadSound(AudioFile.GunsShot, audioSource8);
			AudioSource audioSource9 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource9.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("guns_reload");
			LoadSound(AudioFile.GunsReload, audioSource9);
			AudioSource audioSource10 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource10.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("refill");
			LoadSound(AudioFile.Refill, audioSource10);
			AudioSource audioSource11 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource11.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("flare");
			LoadSound(AudioFile.Flare, audioSource11);
			Walk = Camera.main.gameObject.AddComponent<AudioSource>();
			Walk.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("walk");
			LoadSound(AudioFile.Walk, Walk);
			AudioSource audioSource12 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource12.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("gas");
			LoadSound(AudioFile.Gas, audioSource12);
			AudioSource audioSource13 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource13.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("slide");
			audioSource13.loop = true;
			LoadSound(AudioFile.Slide, audioSource13);
		}

		public static void LoadSound(AudioFile name, AudioSource sound)
		{
			if (_playableAudio.ContainsKey(name))
			{
				_playableAudio.Remove(name);
			}
			_playableAudio.Add(name, new CustomAudio(sound));
		}

		public static void LoadAlternatingSound(AudioFile name, string soundPathA, string soundPathB)
		{
			if (_playableAudio.ContainsKey(name))
			{
				_playableAudio.Remove(name);
			}
			AudioSource audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource.clip = new WWW(soundPathA).GetAudioClip(threeD: true);
			audioSource.volume = 0.3f;
			ICustomAudio sourceA = new CustomAudio(audioSource);
			AudioSource audioSource2 = Camera.main.gameObject.AddComponent<AudioSource>();
			audioSource2.clip = new WWW(soundPathB).GetAudioClip(threeD: true);
			audioSource2.volume = 0.3f;
			ICustomAudio sourceB = new CustomAudio(audioSource2);
			_playableAudio.Add(name, new CustomAlternatingAudio(sourceA, sourceB));
		}

		public static void Play(AudioFile name)
		{
			//_playableAudio[name].Play();
		}

		public static void PlayDynamic(AudioFile name, float pitch, float volume)
		{
			//_playableAudio[name].Modify(pitch, volume);
			//_playableAudio[name].Play();
		}

		public static bool AudioIsPlaying(AudioFile name)
		{
			return _playableAudio[name].IsPlaying();
		}

		public static void Stop(AudioFile name)
		{
			_playableAudio[name].Stop();
		}
	}
}
