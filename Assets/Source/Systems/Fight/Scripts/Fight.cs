using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;
    public VisualShake visualShake;
    public GameObject panel;
    public GameObject prefabKeyCombination;
    public GameObject parentForKeyCombination;
    public Slider timer;

    /// <summary>
    /// Shared and set by <see cref="SceneDirector"/>
    /// </summary>
    public int fightIndex = 0;

    [Header("Determinator")]
    private bool shakeCamera = false;
    private bool shakePanel = true;

    private System.Random randomSystem;

    private int lastRandomValue = -1;
    private int sameRandomValueCount = 0;
    private int limitRandomValueCount = 0;

    private bool stopTimer = false;

    private int fightTimes = 0;
    private int fightTimeCount = 1; //hitungannya segera bertambah setelah melakukan, jadi 1 kali

    private bool run = false;
    private bool isAnimationEnded = false;
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

        if (randomValue == this.lastRandomValue)
        {
            this.sameRandomValueCount++;
        }
        else
        {
            this.sameRandomValueCount = 0;
        }

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

        //akhir
        this.isAnimationEnded = false;
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

        StatsController enemy_statscontroller = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<StatsController>();
        MainStatsData enemy_statsdata = enemy_statscontroller.GetMainStats();

        StatsController player_statscontroller = this.cbkta_globalobjects.player.GetComponent<StatsController>();
        MainStatsData player_statsdata = player_statscontroller.GetMainStats();

        //mulai

        if (this.fightArrowControllerObjectsIndex < this.fightArrowControllerObjects.Count && this.timer.value > 0)
        {
            bool isKeyPressedCorrectly = this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].IsKeyPressedCorrectly();
            bool isKeyPressedWrongly = this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].IsKeyPressedWrongly();

            if (isKeyPressedCorrectly)
            {
                this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].Explosion();
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
                enemy_statscontroller.Attack(this.cbkta_globalobjects.player);

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
        else
        {
            //if (this.fightTimeCount == this.fightTimes && this.fightArrowControllerObjects.Last().isAnimationEnded)

            //sesi dan animasi fight sudah selesai
            if (this.fightArrowControllerObjects.Last().isAnimationEnded)
            {
                //player berhasil menyerang musuh
                player_statscontroller.Attack(this.cbkta_globalobjects.playerTriggeredWithObject);

                //reset
                this.timer.value = this.timer.maxValue;

                //jika nyawa musuh habis
                if (enemy_statsdata.healthPoint == 0)
                {
                    //buat untuk exit dari fight

                    //trigger ke fungsi OnDisable()
                    gameObject.SetActive(false);
                }
                //lanjut saja
                else
                {
                    this.Restart();
                }
            }

            //setup untuk berhenti atau lanjut ke berikutnya, bergantung terhadap nilai berapa banyak kali
        }

        //hasil


        //jika health player sudah habis duluan
        if (player_statsdata.healthPoint == 0)
        {
            //nonaktifkan Fight
            gameObject.SetActive(false);
        }

        if (this.timer.value == 0)
        {
            //player gagal menyerang dan berakhir diserang musuh
            enemy_statscontroller.Attack(this.cbkta_globalobjects.player);

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
    }

    void OnEnable()
    {
        //setup
        this.fightTimes = this.randomSystem.Next(5, 8+1);
        this.fightTimeCount = 1;
        FightDataTemplate obj = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<FightDataTemplate>();
        this.fightDataCache = obj.data[this.fightIndex];

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
        this.fightDataCache = null; //destroy
    }
}
