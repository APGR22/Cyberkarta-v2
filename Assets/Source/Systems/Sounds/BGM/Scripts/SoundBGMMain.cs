using UnityEngine;

public class SoundBGMMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    [Header("Downtown")]
    public AudioClip downtownBGM;

    [Header("BrainD Realm")]
    public AudioClip brainDRealmBGM;

public void Play(AudioClip bgm)
    {
        soundManagerLogic.BGMAudioSource.clip = bgm;
        soundManagerLogic.BGMAudioSource.Play();
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
