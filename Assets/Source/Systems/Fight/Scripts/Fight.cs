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
    public GameObject panel;
    public GameObject prefabKeyCombination;
    public GameObject parentForKeyCombination;
    public Slider timer;

    private System.Random randomSystem;

    private int lastRandomValue = -1;
    private int sameRandomValueCount = 0;
    private int limitRandomValueCount = 0;

    private bool stopTimer = false;

    private int fightTimes = 0;
    private int fightTimeCount = 1; //hitungannya segera bertambah setelah melakukan, jadi 1 kali

    private bool run = false;
    private bool isAnimationEnded = false;

    private List<FightArrowController> fightArrowControllerObjects = new();
    private int fightArrowControllerObjectsIndex = 0;

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

    void SpawnObjects(int minObjectCounts = 3, int maxObjectCounts = 5)
    {
        //mempersiapkan data
        int objCount = this.randomSystem.Next(minObjectCounts, maxObjectCounts+1); //min - max-1

        //munculkan objek dengan jumlah yang sudah ditentukan secara random
        for (int i = 0; i < objCount; i++)
        {
            GameObject obj = Instantiate(this.prefabKeyCombination, this.parentForKeyCombination.transform);
            FightArrowController fightArrowController = obj.GetComponent<FightArrowController>();

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

    void OnShaken(GameObject obj)
    {
        this.panel.GetComponent<Image>().color = new Color(255, 0, 0);
    }

    void ExitShaken(GameObject obj)
    {
        this.panel.GetComponent<Image>().color = new Color(255, 255, 255);
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
                }
            }
            else if (isKeyPressedWrongly)
            {
                //player gagal menyerang dan berakhir diserang musuh

                StatsController enemy_statscontroller = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<StatsController>();
                enemy_statscontroller.Attack(this.cbkta_globalobjects.player);

                //getar
                this.cbkta_globallogichelper.ShakeGameObject(this.panel, 10, .3f, this.OnShaken, this.ExitShaken);
                this.cbkta_globallogichelper.ShakeGameObject(this.cbkta_globalui.cam, 1);

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
                MainStats enemy_stats = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<MainStats>();
                StatsController player_statscontroller = this.cbkta_globalobjects.player.GetComponent<StatsController>();

                //player berhasil menyerang musuh
                player_statscontroller.Attack(this.cbkta_globalobjects.playerTriggeredWithObject);

                //reset
                this.timer.value = this.timer.maxValue;

                //jika nyawa musuh habis
                if (enemy_stats.healthPoint == 0)
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

        MainStats player_stats = this.cbkta_globalobjects.player.GetComponent<MainStats>();

        //jika health player sudah habis duluan
        if (player_stats.healthPoint == 0)
        {
            //nonaktifkan Fight
            gameObject.SetActive(false);
        }

        if (this.timer.value == 0)
        {
            //player gagal menyerang dan berakhir diserang musuh

            StatsController enemy_statscontroller = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<StatsController>();
            enemy_statscontroller.Attack(this.cbkta_globalobjects.player);

            //getar
            this.cbkta_globallogichelper.ShakeGameObject(this.panel, 10, .3f, this.OnShaken, this.ExitShaken);
            this.cbkta_globallogichelper.ShakeGameObject(this.cbkta_globalui.cam, 1);

            //reset
            this.Restart();
        }
    }

    void OnEnable()
    {
        //setup
        this.fightTimes = this.randomSystem.Next(5, 8+1);
        this.fightTimeCount = 1;
        FightArrowController obj = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<FightArrowController>();

        this.SpawnObjects(obj.minArrowCounts, obj.maxArrowCounts);

        this.run = true;
    }

    void OnDisable()
    {
        this.DestroyObjects();
        this.run = false;
        this.cbkta_globalstates.isFightDone = true;
    }
}
