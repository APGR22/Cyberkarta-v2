using UnityEngine;

public class SoundBGMMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    public SoundMainData mainMenuBGM = new();
    public SoundMainData downtownBGM = new();
    public SoundMainData brainDRealmBGM = new();
    public SoundMainData cyberkartaBGM = new();

    public void Play(SoundMainData bgm)
    {
        //setup
        AudioSource speaker = this.soundManagerLogic.BGMAudioSource;

        //settings
        speaker.clip = bgm.sound;
        speaker.volume = this.soundManagerLogic.GetBGMVolume(bgm.volume);

        speaker.Play();
    }

    public void Stop()
    {
        soundManagerLogic.BGMAudioSource.Stop();
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
