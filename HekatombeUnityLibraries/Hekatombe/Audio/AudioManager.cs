using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hekatombe.Base;
using Hekatombe.Utils;

namespace Hekatombe.Audio
{
	public class AudioManager : MonoBehaviour
	{
		private static AudioManager _instance;
		private AudioSource _sourceMusic;
		private AudioSource _sourceSound;

		private float _volumeSound = 1.0f;
		private float _volumeMusic = 1.0f;

		public const bool IS_SOUND_ENABLED = true;
		public const bool IS_MUSIC_ENABLED = true;

		public const string kPPVolumeSound = "PPVolumeSound";
		public const string kPPVolumeMusic = "PPVolumeMusic";

		public static void Create()
		{
			if (_instance != null)
			{
				return;
			}
			//Create GameObject AudioManager
			GameObject go = new GameObject();
			_instance = go.AddComponent<AudioManager>();
			_instance.Init();
			DontDestroyOnLoad(_instance.gameObject);
		}

		private void Init ()
		{
			_instance = this;
			_volumeSound = PlayerPrefs.GetFloat(kPPVolumeSound, 1);
			_volumeMusic = PlayerPrefs.GetFloat(kPPVolumeMusic, 1);
			//Create MusicSource & SoundSource
			_sourceMusic = gameObject.AddComponent<AudioSource>();
			GameObject go = new GameObject();
			go.name = "SoundSource";
			go.transform.SetParent(transform);
			_sourceSound = go.AddComponent<AudioSource>();
		}

		/******
		 * Public members
		 */ 
		public static void PlayOnce (string name)
		{
			_instance.PlayOnceMe(name, 1);
		}

		public static void PlayOnce (string name, float volume)
		{
			_instance.PlayOnceMe(name, volume);
		}

		public static void PlayMusic (string name)
		{
			_instance.PlayMusicMe(name);
		}

		public static float VolumeSound
		{
			get {
				return _instance._volumeSound;
			}
			set {
				_instance._volumeSound = value;
				_instance._sourceSound.volume = value;
				PlayerPrefs.SetFloat(kPPVolumeSound, value);
			}
		}

		public static float VolumeMusic
		{
			get {
				return _instance._volumeMusic;
			}
			set {
				_instance._volumeMusic = value;
				_instance._sourceMusic.volume = value;
				PlayerPrefs.SetFloat(kPPVolumeMusic, value);
			}
		}

		/******
		 * Private Members
		 */ 
		private void PlayOnceMe(string name, float volume)
		{
			if (!IS_SOUND_ENABLED || name.IsEmpty() || _volumeSound <= 0 || volume <= 0) {
				return;
			}
			AudioClip clip = Resources.Load<AudioClip>("Audio/Sound/"+name);
			if (clip == null) {
				Debug.LogError ("Audio not found in audioClips on PlayOnce: " + name);
				return;
			}
			_sourceSound.PlayOneShot (clip, volume * _volumeSound);
		}


		private void PlayMusicMe (string name)
		{
			if (!IS_MUSIC_ENABLED || name.IsEmpty()) {
				return;
			}
			AudioClip clip = Resources.Load<AudioClip>("Audio/Music/"+name);
			if (clip == null) {
				Debug.LogError ("Audio not found in audioClips on PlayMusic: " + name);
				return;
			}

			//Comprova que no s'estigui executant ja aquesta musica
			if (_sourceMusic.clip != null && _sourceMusic.clip.name == name) {
				return;
			}
			_sourceMusic.clip = clip;
			_sourceMusic.Stop ();
			_sourceMusic.loop = true;
			_sourceMusic.Play ();
			_sourceMusic.volume = _volumeMusic;
		}
	}
}