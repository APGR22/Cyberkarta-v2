using UnityEngine;

public class Level1Main : LevelMain
{
    public GameObject dialogueTrigger;
    public SoundManagerLogic soundManager;

    void Init()
    {
        //play bgm
        SoundEnvironmentsMain soundEnvironmentsMain = this.soundManager.soundEnvironmentsMain;
        soundEnvironmentsMain.Play(soundEnvironmentsMain.level1);

        this.dialogueTrigger.SetActive(true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ParentInit(this.Init);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
