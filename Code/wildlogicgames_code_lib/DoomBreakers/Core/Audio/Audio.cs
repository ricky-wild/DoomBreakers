using System;
using UnityEngine;

namespace DoomBreakers
{
	public class Audio : MonoBehaviour
	{
		private GameObject _gameObject;
		private AudioSource _audioSource;
		private AudioClip _audioClip;
		private float _volume, _timeStamp, _waitTime, _length;
		

		public Audio(string fileName, float volume, bool loop, float pitch, Transform parentTransform)
		{
			_gameObject = new GameObject();
			_gameObject.transform.parent = parentTransform;
			_gameObject.name = fileName + "_AudioEventManager_SFX";

			_audioSource = (AudioSource)_gameObject.AddComponent(typeof(AudioSource));
			_audioClip = (AudioClip)Resources.Load(fileName, typeof(AudioClip));

			_audioSource.clip = _audioClip;
			_audioSource.loop = loop;
			_volume = volume;
			_audioSource.volume = _volume;
			_audioSource.pitch = pitch;

			_length = _audioSource.time;//_audioClip.length;


		}


		public void PlaySound()
		{
			if (!_audioSource.isPlaying)
				_audioSource.Play();
			else
			{
				StopSound();
				PlaySound();
			}
		}

		public void StopSound() => _audioSource.Stop(); //_audioSource.Pause();

	}
}
