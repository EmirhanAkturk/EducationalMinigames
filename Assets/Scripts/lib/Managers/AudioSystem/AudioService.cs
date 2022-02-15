using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public static class AudioService
{
	private static AudioCollection audioCollection;
	private const string COLLECTION_PATH = "Configurations/AudioCollection";

	private static GameObject audioPlayerContext;

	private static Dictionary<AudioType, GAudio> audioByType = new Dictionary<AudioType, GAudio>();
	private static Dictionary<AudioType, AudioSource> sourceByType = new Dictionary<AudioType, AudioSource>();

	//TODO Enes: Make use of MixerGroup from runtime side aswell
	//MixerGroups for changing extra layer of control
	private static Dictionary<AudioType, AudioMixerGroup> mixerGroupByType = new Dictionary<AudioType, AudioMixerGroup>();

	//Handles Loading and Initialization
	static AudioService()
	{
		if (!Application.isPlaying) return;

		audioPlayerContext = new GameObject("AudioPlayerContext");
		Object.DontDestroyOnLoad(audioPlayerContext);

		audioCollection = Resources.Load<AudioCollection>(COLLECTION_PATH);

		foreach (var audio in audioCollection.Audio)
		{
			if (audioByType.ContainsKey(audio.Type))
			{
				Debug.Log("Duplicate Audio, Audio Type: " + audio.Type + " Please check the collection and remove any duplicates.");
				continue;
			}
			audioByType.Add(audio.Type, audio);
			sourceByType.Add(audio.Type, GetAudioSource(audioPlayerContext, audio));
			mixerGroupByType.Add(audio.Type, audio.MixerGroup);
		}

		LoadStates();
	}
	private static AudioSource GetAudioSource(GameObject audioContext, GAudio audio)
	{
		AudioSource audioSource = audioPlayerContext.AddComponent<AudioSource>();
		audioSource.clip = audio.Clip;
		audioSource.outputAudioMixerGroup = audio.MixerGroup;
		audioSource.volume = audio.Volume;
		audioSource.pitch = audio.Pitch;
		audioSource.playOnAwake = audio.PlayOnAwake;
		if (audioSource.playOnAwake) audioSource.Play();
		audioSource.loop = audio.Loop;
		return audioSource;
	}

	public static void Play(AudioType audioType)
	{
		sourceByType[audioType].Play();
	}
	public static void PlayOneShot(AudioType audioType)
	{
		sourceByType[audioType].PlayOneShot(audioByType[audioType].Clip);
	}

	public static void Mute(AudioType audioType) => sourceByType[audioType].mute = true;
	public static void MuteAll()
	{
		foreach (var audioSource in sourceByType.Values)
			audioSource.mute = true;

		muted = 1;
		SaveStates();
	}
	public static void UnMute(AudioType audioType) => sourceByType[audioType].mute = false;
	public static void UnMuteAll()
	{
		foreach (var audioSource in sourceByType.Values)
			audioSource.mute = false;

		muted = 0;
		SaveStates();
	}

	private static int muted;
	public static int Muted => muted;
	private static Dictionary<SettingType, int> states;
	private static void LoadStates()
	{
		// TODO: will remove in future versions
		var tmpstate = DataService.GetData<Dictionary<StateType, int>>(DataType.STATE);
		if (tmpstate.ContainsKey(StateType.MuteAudio))
        {
			tmpstate.Remove(StateType.MuteAudio);
			DataService.SetData(DataType.SETTING, tmpstate);
		}
		
		states = DataService.GetData<Dictionary<SettingType, int>>(DataType.SETTING);
		if (states == null)
		{
			states = new Dictionary<SettingType, int>();
			DataService.SetData(DataType.SETTING, states);
		}
		if (!states.ContainsKey(SettingType.MuteAudio))
			states.Add(SettingType.MuteAudio, 0);
		muted = states[SettingType.MuteAudio];
		if (muted == 1) MuteAll();
	}
	private static void SaveStates()
	{
		states = DataService.GetData<Dictionary<SettingType, int>>(DataType.SETTING);
		states[SettingType.MuteAudio] = muted;
		DataService.SetData(DataType.SETTING, states);
	}
}
