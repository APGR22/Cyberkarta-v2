using UnityEngine;

public class Level3Main : LevelMain
{
    public SoundManagerLogic soundManagerLogic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ParentInit(() =>
        {
            SoundBGMMain soundBGMMain = this.soundManagerLogic.soundBGMMain;
            soundBGMMain.Play(soundBGMMain.cyberkartaBGM);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
