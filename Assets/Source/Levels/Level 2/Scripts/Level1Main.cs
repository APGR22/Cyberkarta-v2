using UnityEngine;

public class Level1Main : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;

    void Init()
    {
        FadeController fadeController = this.cbkta_globalui.fadeController;

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundBGMMain soundBGMMain = soundManagerLogic.soundBGMMain;

        fadeController.gameObject.SetActive(true);
        fadeController.value = 1;
        fadeController.FadeOut();

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
