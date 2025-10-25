using UnityEngine;

public class SoundEnvironmentsMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    public SoundMainData level1 = new();
    public SoundMainData level2 = new();

    public void Play(SoundMainData environment)
    {
        //setup
        AudioSource speaker = this.soundManagerLogic.EnvironmentAudioSource;

        //settings
        speaker.clip = environment.sound;
        speaker.volume = this.soundManagerLogic.GetEnvironmentVolume(environment.volume);

        speaker.Play();
    }

    public void Stop()
    {
        this.soundManagerLogic.EnvironmentAudioSource.Stop();
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
