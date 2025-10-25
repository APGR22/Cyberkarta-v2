using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;
    public cbkta_GlobalLogic cbkta_globallogic;

    [Header("Fight System")]
    public VisualShake visualShake;
    public GameObject panel;
    public GameObject prefabKeyCombination;
    public GameObject parentForKeyCombination;
    public Slider timer;
    public FightStatus fightStatus;

    /// <summary>
    /// Shared and set by <see cref="SceneDirector"/>
    /// </summary>
    public int fightIndex = 0;

    [Header("Determinator")]

    private System.Random randomSystem;

    private int lastRandomValue = -1;

    private bool stopTimer = false;

    private int totalChancePoint;
    private int totalPlayerSuccessPoint;
    private int totalPlayerFailedPoint;

    private bool run = false;
    private bool isShaken = false;

    private List<FightArrowController> fightArrowControllerObjects = new();
    private int fightArrowControllerObjectsIndex = 0;

    /// <summary>
    /// Defined on OnEnable(), destroyed on OnDisable()
    /// </summary>
    private FightData fightDataCache;

    //events
    private List<Action> listOfFuncEventOnPlayerAttackEnemy = new();
    private List<Action> listOfFuncEventOnEnemyAttackPlayer = new();

    public void AddFuncEventOnPlayerAttackEnemy(Action func)
    {
        this.listOfFuncEventOnPlayerAttackEnemy.Add(func);
    }

    public void RemoveFuncEventOnPlayerAttackEnemy(Action func)
    {
        this.listOfFuncEventOnPlayerAttackEnemy.Remove(func);
    }

    public bool ContainsFuncEventOnPlayerAttackEnemy(Action func)
    {
        return this.listOfFuncEventOnPlayerAttackEnemy.Contains(func);
    }

    public void AddFuncEventOnEnemyAttackPlayer(Action func)
    {
        this.listOfFuncEventOnEnemyAttackPlayer.Add(func);
    }

    public void RemoveFuncEventOnEnemyAttackPlayer(Action func)
    {
        this.listOfFuncEventOnEnemyAttackPlayer.Remove(func);
    }

    public bool ContainsFuncEventOnEnemyAttackPlayer(Action func)
    {
        return this.listOfFuncEventOnEnemyAttackPlayer.Contains(func);
    }

    int GetRandomValue()
    {
        int fightDataTypesCount = System.Enum.GetValues(typeof(FightDataType)).Length;

        int randomValue = 0; //default

        //ulangi terus jika nilainya sama terus selama 2 kali
        do
        {
            randomValue = this.randomSystem.Next(0, fightDataTypesCount); //0 - max-1
        }
        while (randomValue == this.lastRandomValue);

        this.lastRandomValue = randomValue;

        return randomValue;
    }

    void SpawnObjects()
    {
        //mempersiapkan events
        int minObjectCounts = this.fightDataCache.minArrowCounts;
        int maxObjectCounts = this.fightDataCache.maxArrowCounts;

        int objCount = this.randomSystem.Next(minObjectCounts, maxObjectCounts+1); //min - max-1

        //munculkan objek dengan jumlah yang sudah ditentukan secara random
        for (int i = 0; i < objCount; i++)
        {
            //mengkloning objek dan mendapatkan komponennya
            GameObject obj = Instantiate(this.prefabKeyCombination, this.parentForKeyCombination.transform);
            FightArrowController fightArrowController = obj.GetComponent<FightArrowController>();

            //inisialisasi
            fightArrowController.Init(GetRandomValue());
            this.fightArrowControllerObjects.Add(fightArrowController);
        }
    }

    void DestroyObjects()
    {
        foreach (FightArrowController obj in this.fightArrowControllerObjects)
        {
            Destroy(obj.gameObject);
        }

        this.fightArrowControllerObjects.Clear();
        this.fightArrowControllerObjectsIndex = 0;
    }

    void Restart()
    {
        this.DestroyObjects();
        this.SpawnObjects();

        this.timer.value = this.timer.maxValue;
        this.stopTimer = false;
    }

    void OnShaken(GameObject obj = null)
    {
        this.isShaken = true;
    }

    void ExitShaken(GameObject obj = null)
    {
        this.isShaken = false;
    }

    void Awake()
    {
        this.randomSystem = this.cbkta_globallogichelper.GenerateRandomSystem();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.run) return;

        if (this.timer.value != 0 && !this.stopTimer)
        {
            this.timer.value -= Time.deltaTime;
        }

        //setup

        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundSFXMain soundSFXMain = soundManagerLogic.soundSFXMain;
        SoundEventMusicMain soundEventMusicMain = soundManagerLogic.soundEventMusicMain;

        //mulai

            // masih ada panah dan timer belum habis
        if (this.fightArrowControllerObjectsIndex < this.fightArrowControllerObjects.Count && this.timer.value > 0)
        {
            //setup
            FightArrowController fightArrowController = this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex];
            SoundMainData fightArrowSFX = new();

            //mulai

            bool isKeyPressedCorrectly = fightArrowController.IsKeyPressedCorrectly();
            bool isKeyPressedWrongly = fightArrowController.IsKeyPressedWrongly();

            if (isKeyPressedCorrectly)
            {
                this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].Explosion();
                switch (fightArrowController.arrowType)
                {
                    case FightDataType.arrowUp:
                        fightArrowSFX = soundSFXMain.fightArrowUp;
                        break;
                    case FightDataType.arrowDown:
                        fightArrowSFX = soundSFXMain.fightArrowDown;
                        break;
                    case FightDataType.arrowLeft:
                        fightArrowSFX = soundSFXMain.fightArrowLeft;
                        break;
                    case FightDataType.arrowRight:
                        fightArrowSFX = soundSFXMain.fightArrowRight;
                        break;
                }
                soundSFXMain.Play(fightArrowSFX);

                fightArrowController.Explosion();
                this.fightArrowControllerObjectsIndex++;

                //jika di index terakhir
                if (this.fightArrowControllerObjectsIndex == this.fightArrowControllerObjects.Count)
                {
                    this.stopTimer = true;

                    foreach (Action func in this.listOfFuncEventOnPlayerAttackEnemy)
                    {
                        func();
                    }
                }
            }
            else if (isKeyPressedWrongly)
            {
                //player gagal menyerang dan berakhir diserang musuh
                //enemy_statscontroller.Attack(this.cbkta_globalobjects.player);
                this.totalPlayerFailedPoint++;

                soundSFXMain.PlayRandomOnRange(soundSFXMain.fightMissClick);

                //getar
                if (!this.isShaken)
                {
                    if (this.fightDataCache.shakePanel)
                    {
                        this.visualShake.ShakeGameObject(this.panel, 10, .3f,
                        (GameObject obj) =>
                        {
                            this.panel.GetComponent<Image>().color = new Color(255, 0, 0);
                        },
                        (obj) =>
                        {
                            this.panel.GetComponent<Image>().color = new Color(255, 255, 255);
                            this.ExitShaken(obj);
                        }
                        );
                    }

                    if (this.fightDataCache.shakeCamera)
                    {
                        this.visualShake.ShakeGameObject(this.cbkta_globalui.cam, 1, .3f, null, (obj) =>
                        {
                            this.ExitShaken(obj);
                        });
                    }

                    this.OnShaken();
                }

                //ulangi
                this.Restart();
            }

        }
        // jika panah berhasil ditekan semua dan timer belum habis, atau semuanya sudah selesai
        else
        {
            //if (this.fightTimeCount == this.fightTimes && this.fightArrowControllerObjects.Last().isAnimationEnded)

            //jika sesi dan animasi fight sudah selesai
            if (this.fightArrowControllerObjects.Last().isAnimationEnded)
            {
                //player berhasil menyerang musuh
                //player_statscontroller.Attack(this.cbkta_globalobjects.playerTriggeredWithObject);
                this.totalPlayerSuccessPoint++;

                //reset
                this.timer.value = this.timer.maxValue;

                //lanjut
                this.Restart();
            }

            //setup untuk berhenti atau lanjut ke berikutnya, bergantung terhadap nilai berapa banyak kali
        }

        //hasil


        //jika timer habis
        if (this.timer.value == 0)
        {
            //player gagal menyerang dan berakhir diserang musuh
            //enemy_statscontroller.Attack(this.cbkta_globalobjects.player);
            this.totalPlayerFailedPoint++;

            soundSFXMain.PlayRandomOnRange(soundSFXMain.fightMissClick);

            //getar
            if (!this.isShaken)
            {
                if (this.fightDataCache.shakePanel) this.visualShake.ShakeGameObject(this.panel, 10, .3f,
                    (GameObject obj) =>
                    {
                        this.panel.GetComponent<Image>().color = new Color(255, 0, 0);
                    },
                    (obj) =>
                    {
                        this.panel.GetComponent<Image>().color = new Color(255, 255, 255);
                        this.ExitShaken(obj);
                    }
                );

                if (this.fightDataCache.shakeCamera) this.visualShake.ShakeGameObject(this.cbkta_globalui.cam, 1, .3f, null, this.ExitShaken);

                this.OnShaken();
            }

            //reset
            this.Restart();
        }

        this.fightStatus.successSlider.value = (float) this.totalPlayerSuccessPoint / (float) this.totalChancePoint;
        this.fightStatus.failedSlider.value = (float) this.totalPlayerFailedPoint / (float) this.totalChancePoint;

        //cek keberhasilan
        if (this.totalPlayerSuccessPoint > (this.totalChancePoint/2))
        {
            this.fightStatus.Win();

            this.run = false;

            this.cbkta_globallogic.ExecuteFuncOnSeconds(() =>
            {
                this.gameObject.SetActive(false);
            }, .5f);
        }
        if (this.totalPlayerFailedPoint > (this.totalChancePoint/2))
        {
            this.fightStatus.Lose();

            foreach (FightArrowController objFightArrowController in this.fightArrowControllerObjects)
            {
                objFightArrowController.SetError();
            }

            this.run = false;

            this.cbkta_globallogic.ExecuteFuncOnSeconds(this.cbkta_globallogic.RestartScene, .1f);

            //this.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        //setup

        GameObject enemy = this.cbkta_globalobjects.playerTriggeredWithObject;

        FightDataTemplate enemyFightDataTemplate = enemy.GetComponent<FightDataTemplate>();

        StatsController enemyStatsController = enemy.GetComponent<StatsController>();
        MainStatsData enemyStatsData = enemyStatsController.GetMainStats();

        //settings
        this.totalChancePoint = enemyStatsData.totalChance;

        //reset
        this.totalPlayerSuccessPoint = 0;
        this.totalPlayerFailedPoint = 0;

        //cache

        this.fightDataCache = enemyFightDataTemplate.data[this.fightIndex];

        this.SpawnObjects();

        this.run = true;
    }

    void OnDisable()
    {
        //setup

        GameObject player = this.cbkta_globalobjects.player;
        GameObject obj = this.cbkta_globalobjects.playerTriggeredWithObject;

        StatsController playerStatsController = player.GetComponent<StatsController>();
        StatsController objStatsController = obj.GetComponent<StatsController>();

        FightDataTemplate objFightDataTemplate = obj.GetComponent<FightDataTemplate>();

        //kirim sinyal event
        this.cbkta_globalui.levelDirectorMain.SendEvent(
            objFightDataTemplate.eventNameForLevelDirectorMain,
            this.gameObject,
            this.GetType(),
            System.Reflection.MethodBase.GetCurrentMethod().Name,
            playerStatsController,
            objStatsController
        );

        //setting

        this.DestroyObjects();

        this.run = false;
        this.cbkta_globalstates.isFightDone = true;
        this.stopTimer = false;
        // destroy
        this.fightDataCache = null;
    }
}
