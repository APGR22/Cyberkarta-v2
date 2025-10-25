using UnityEngine;

public class Level2Main : LevelMain
{
    void Init()
    {
        //setup

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundBGMMain soundBGMMain = soundManagerLogic.soundBGMMain;
        SoundEnvironmentsMain soundEnvironmentsMain = soundManagerLogic.soundEnvironmentsMain;

        //settings
        soundBGMMain.Play(soundBGMMain.downtownBGM);
        soundEnvironmentsMain.Play(soundEnvironmentsMain.level2);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ParentInit();
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
