using UnityEngine;

public class SoundSFXMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    [Header("Player")]
    public SoundMainData2D playerGroundFootsteps;
    public SoundMainData2D playerFloorFootsteps;
    public SoundMainData2D playerConcreteFootsteps;

    [Header("Dialogue")]
    public SoundMainData2D humanDialogueVoice;
    public SoundMainData2D robotDialogueVoice;

    [Header("Fight")]
    public SoundMainData fightArrowUp;
    public SoundMainData fightArrowDown;
    public SoundMainData fightArrowLeft;
    public SoundMainData fightArrowRight;
    public SoundMainData2D fightMissClick;
    public SoundMainData2D fightAttack;

    public void Play(SoundMainData sfx)
    {
        //setup
        AudioSource speaker = this.soundManagerLogic.SFXAudioSource;

        //settings
        speaker.volume = this.soundManagerLogic.GetSFXVolume(sfx.volume);
        speaker.PlayOneShot(sfx.sound);
    }

    public void PlayRandomOnRange(SoundMainData2D sfxArray,
        float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        //setup
        int randomIndex = Random.Range(0, sfxArray.sounds.Count);
        AudioClip sfx = sfxArray.sounds[randomIndex];
        AudioSource speaker = soundManagerLogic.SFXAudioSource;

        //float randomPitch = Random.Range(minPitch, maxPitch);
        //soundManagerLogic.SFXAudioSource.pitch = randomPitch;

        //settings
        speaker.volume = this.soundManagerLogic.GetSFXVolume(sfxArray.volume);
        speaker.PlayOneShot(sfx);
    }

    public void Stop()
    {
        this.soundManagerLogic.SFXAudioSource.Stop();
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
