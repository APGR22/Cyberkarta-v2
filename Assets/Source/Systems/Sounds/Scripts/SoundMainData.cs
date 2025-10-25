using System.Collections.Generic;
using UnityEngine;

public enum SoundMainDataType
{
    None,
    SFX,
    BGM,
    EventMusic,
    Environment,
}

[System.Serializable]
public class SoundMainData
{
    public AudioClip sound;
    /// <remarks>
    /// 0 - 1
    /// </remarks>
    [Tooltip("1 = 100%")]
    [Range(0, 1)]
    public float volume;

    public SoundMainData()
    {
        this.sound = null;
        this.volume = 1;
    }

    public SoundMainData(AudioClip sound, float volume)
    {
        this.sound = sound;
        this.volume = volume;
    }
}

[System.Serializable]
public class SoundMainData2D
{
    public List<AudioClip> sounds;
    /// <remarks>
    /// 0 - 1
    /// </remarks>
    [Tooltip("1 = 100%")]
    [Range(0, 1)]
    public float volume;

    public SoundMainData2D()
    {
        this.sounds = new();
        this.volume = 1;
    }

    public SoundMainData2D(List<AudioClip> sounds, float volume)
    {
        this.sounds = sounds;
        this.volume = volume;
    }
}
