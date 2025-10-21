using UnityEngine;

public class Level1Main : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalLogic cbkta_globallogic;

    void Init()
    {
        //setup
        FadeController fadeController = this.cbkta_globalui.fadeController;

        //settings
        this.cbkta_globallogic.FreezePlayer();

        // memastikan
        fadeController.gameObject.SetActive(true);
        fadeController.value = 1;

        fadeController.FadeOut(() =>
        {
            this.cbkta_globallogic.UnfreezePlayer();
        });
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
