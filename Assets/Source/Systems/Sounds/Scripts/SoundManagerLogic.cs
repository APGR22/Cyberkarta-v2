using UnityEngine;

public class SoundManagerLogic : MonoBehaviour
{
    public SoundSFXMain soundSFXMain;
    public SoundBGMMain soundBGMMain;
    public SoundEventMusicMain soundEventMusicMain;
    public SoundEnvironmentsMain soundEnvironmentsMain;

    [HideInInspector] public AudioSource SFXAudioSource;
    [HideInInspector] public AudioSource BGMAudioSource;
    [HideInInspector] public AudioSource EventMusicAudioSource;
    [HideInInspector] public AudioSource EnvironmentAudioSource;

    /// <summary>
    /// 0 - 1
    /// </summary>
    [Range(0, 1)] public float masterVolume = 1;
    /// <summary>
    /// 0 - 1
    /// </summary>
    [Range(0, 1)] public float sfxVolume = 1;
    /// <summary>
    /// 0 - 1
    /// </summary>
    [Range(0, 1)] public float bgmVolume = 1;
    /// <summary>
    /// 0 - 1
    /// </summary>
    [Range(0, 1)] public float environmentVolume = 1;
    /// <summary>
    /// 0 - 1
    /// </summary>
    [Range(0, 1)] public float eventSoundVolume = 1;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <paramref name="clipVolume"/>: May to pass bigger than 1
    /// </remarks>
    /// <param name="clipVolume">May to pass bigger than 1</param>
    /// <returns></returns>
    public float GetSFXVolume(float clipVolume) => Mathf.Clamp01(this.masterVolume * this.sfxVolume * clipVolume);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <paramref name="clipVolume"/>: May to pass bigger than 1
    /// </remarks>
    /// <param name="clipVolume">May to pass bigger than 1</param>
    /// <returns></returns>
    public float GetBGMVolume(float clipVolume) => Mathf.Clamp01(this.masterVolume * this.bgmVolume * clipVolume);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <paramref name="clipVolume"/>: May to pass bigger than 1
    /// </remarks>
    /// <param name="clipVolume">May to pass bigger than 1</param>
    /// <returns></returns>
    public float GetEventSoundVolume(float clipVolume) => Mathf.Clamp01(this.masterVolume * this.eventSoundVolume * clipVolume);

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <paramref name="clipVolume"/>: May to pass bigger than 1
    /// </remarks>
    /// <param name="clipVolume">May to pass bigger than 1</param>
    /// <returns></returns>
    public float GetEnvironmentVolume(float clipVolume) => Mathf.Clamp01(this.masterVolume * this.environmentVolume * clipVolume);

    public float GetCurrentSFXClipVolume() => this.SFXAudioSource.volume / this.masterVolume / this.sfxVolume;
    public float GetCurrentBGMClipVolume() => this.BGMAudioSource.volume / this.masterVolume / this.bgmVolume;
    public float GetCurrentEventMusicClipVolume() => this.EventMusicAudioSource.volume / this.masterVolume / this.eventSoundVolume;

    void Awake()
    {
        AudioSource[] audioSource = GetComponents<AudioSource>();

        this.SFXAudioSource = audioSource[0];
        this.BGMAudioSource = audioSource[1];
        this.EventMusicAudioSource = audioSource[2];
        this.EnvironmentAudioSource = audioSource[3];

        this.SFXAudioSource.playOnAwake = false;
        this.BGMAudioSource.playOnAwake = false;
        this.EventMusicAudioSource.playOnAwake = false;
        this.EnvironmentAudioSource.playOnAwake = false;

        this.BGMAudioSource.loop = true;
        this.EnvironmentAudioSource.loop = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
