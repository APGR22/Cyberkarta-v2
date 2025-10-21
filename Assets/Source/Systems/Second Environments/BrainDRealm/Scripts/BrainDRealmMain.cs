using System;
using System.Collections.Generic;
using UnityEngine;

public class BrainDRealmMain : MonoBehaviour
{
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalUI cbkta_globalui;
    public Animator secondPlayerAnimator;

    private bool hasInit = false;

    private List<Action> listFuncOnAttack = new();

    private Vector2 previousFightPosition;
    private AudioClip previousBGM;

    void Init()
    {
        //setup

        GameObject fight = this.cbkta_globalui.fight;
        Fight fightFight = fight.GetComponent<Fight>();

        //settings

        this.listFuncOnAttack.Add(() =>
        {
            this.secondPlayerAnimator.SetTrigger("Attack");
        });

        //apply

        foreach (Action func in this.listFuncOnAttack)
        {
            fightFight.AddFuncEventOnPlayerAttackEnemy(func);
        }
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
        if (!this.hasInit)
        {
            this.Init();
            this.hasInit = true;
        }

        //setup
        GameObject fight = this.cbkta_globalui.fight;
        RectTransform fightRectTransform = fight.GetComponent<RectTransform>();

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundBGMMain soundBGMMain = soundManagerLogic.soundBGMMain;

        //cache
        this.previousFightPosition = fightRectTransform.anchoredPosition;

        this.previousBGM = soundManagerLogic.BGMAudioSource.clip;

        //settings
        fightRectTransform.anchoredPosition = new(0, -117);

        soundBGMMain.Play(soundBGMMain.brainDRealmBGM);
    }

    void OnDisable()
    {
        //setup
        GameObject fight = this.cbkta_globalui.fight;
        RectTransform fightRectTransform = fight.GetComponent<RectTransform>();

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundBGMMain soundBGMMain = soundManagerLogic.soundBGMMain;

        //restore
        fightRectTransform.anchoredPosition = this.previousFightPosition;

        soundBGMMain.Play(this.previousBGM);
    }
}
