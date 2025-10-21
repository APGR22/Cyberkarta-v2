using UnityEngine;

public class SoundSFXMain : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;

    [Header("Player")]
    public AudioClip[] footsteps;

    [Header("Dialogue")]
    public AudioClip[] humanDialogueVoice;
    public AudioClip[] robotDialogueVoice;

    public void Play(AudioClip sfx)
    {
        soundManagerLogic.SFXAudioSource.PlayOneShot(sfx);
    }

    public void PlayRandomOnRange(AudioClip[] sfxArray,
        float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        int randomIndex = Random.Range(0, sfxArray.Length);
        AudioClip sfx = sfxArray[randomIndex];

        //float randomPitch = Random.Range(minPitch, maxPitch);
        //soundManagerLogic.SFXAudioSource.pitch = randomPitch;

        soundManagerLogic.SFXAudioSource.PlayOneShot(sfx);
    }

    public void Stop()
    {
        soundManagerLogic.SFXAudioSource.Stop();
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
