using UnityEngine;

public class SoundEventMusicMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    [Header("Fight")]
    public SoundMainData fightWin;
    public SoundMainData fightLose;

    public void Play(SoundMainData sound)
    {
        //setup
        AudioSource speaker = this.soundManagerLogic.EventMusicAudioSource;

        //settings
        speaker.volume = this.soundManagerLogic.GetEventSoundVolume(sound.volume);
        speaker.PlayOneShot(sound.sound);
    }

    public void PlayRandomOnRange(SoundMainData2D soundArray,
        float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        //setup
        int randomIndex = Random.Range(0, soundArray.sounds.Count);
        AudioClip sound = soundArray.sounds[randomIndex];
        AudioSource speaker = this.soundManagerLogic.EventMusicAudioSource;

        //float randomPitch = Random.Range(minPitch, maxPitch);
        //soundManagerLogic.EventMusicAudioSource.pitch = randomPitch;

        //settings
        speaker.volume = this.soundManagerLogic.GetEventSoundVolume(soundArray.volume);
        speaker.PlayOneShot(sound);
    }

    public void Stop()
    {
        this.soundManagerLogic.EventMusicAudioSource.Stop();
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
