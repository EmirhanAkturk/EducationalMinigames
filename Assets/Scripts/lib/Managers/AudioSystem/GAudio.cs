using UnityEngine;
using System;
using UnityEngine.Audio;


[Serializable]
public class GAudio
{
	[SerializeField] private AudioType type;
	[SerializeField] private AudioClip clip;
	[SerializeField] [Range(0, 1f)] private float volume = 1f;
	[SerializeField] [Range(-3, 3f)] private float pitch = 1f;
	[SerializeField] private bool playOnAwake;
	[SerializeField] private bool loop;
	[SerializeField] private AudioMixerGroup mixerGroup;

	public AudioType Type => type;
	public AudioClip Clip => clip;
	public float Volume => volume;
	public float Pitch => pitch;
	public bool PlayOnAwake => playOnAwake;
	public bool Loop => loop;
	public AudioMixerGroup MixerGroup => mixerGroup;
}