using System;
using UnityEngine;

public class LevelMain : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalLogic cbkta_globallogic;

    protected void ParentInit(Action funcEnd = null)
    {
        //setup
        FadeController fadeController = this.cbkta_globalui.fadeController;

        //settings
        this.cbkta_globallogic.FreezePlayer();

        // memastikan
        fadeController.value = 1;

        fadeController.FadeOut(() =>
        {
            this.cbkta_globallogic.UnfreezePlayer();

            if (funcEnd != null) funcEnd();
        });
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
