using UnityEngine;

public class SoundManagerLogic : MonoBehaviour
{
    public SoundSFXMain soundSFXMain;
    public SoundBGMMain soundBGMMain;

    [HideInInspector] public AudioSource SFXAudioSource;
    [HideInInspector] public AudioSource BGMAudioSource;

    //kontrol volume SFX atau BGM

    void Awake()
    {
        AudioSource[] audioSource = GetComponents<AudioSource>();

        this.SFXAudioSource = audioSource[0];
        this.BGMAudioSource = audioSource[1];

        this.SFXAudioSource.playOnAwake = false;
        this.BGMAudioSource.playOnAwake = false;

        this.BGMAudioSource.loop = true;
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
