using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundGameObject : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;
    public SoundMainData sound;
    public SoundMainDataType type;
    public bool loopSFX = false;

    private AudioSource speaker;

    void Awake()
    {
        this.speaker = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        switch (this.type)
        {
            case SoundMainDataType.SFX:
                this.speaker.volume = this.soundManagerLogic.GetSFXVolume(this.sound.volume);
                if (this.loopSFX)
                {
                    this.speaker.clip = this.sound.sound;
                    this.speaker.Play();
                }
                else
                {
                    this.speaker.PlayOneShot(this.sound.sound);
                }
                break;
            case SoundMainDataType.BGM:
                this.speaker.volume = this.soundManagerLogic.GetBGMVolume(this.sound.volume);
                this.speaker.clip = this.sound.sound;
                this.speaker.Play();
                break;
            //dan seterusnya
        }
    }

    void OnDisable()
    {
        this.speaker.Stop();
    }
}
