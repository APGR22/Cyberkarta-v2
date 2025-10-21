using UnityEngine;

public class Level2Main : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalLogic cbkta_globallogic;

    void Init()
    {
        //setup

        FadeController fadeController = this.cbkta_globalui.fadeController;

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundBGMMain soundBGMMain = soundManagerLogic.soundBGMMain;

        //settings
        this.cbkta_globallogic.FreezePlayer();

        // memastikan
        fadeController.gameObject.SetActive(true);
        fadeController.value = 1;

        fadeController.FadeOut(() =>
        {
            this.cbkta_globallogic.UnfreezePlayer();
        });

        soundBGMMain.Play(soundBGMMain.downtownBGM);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
