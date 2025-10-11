using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalStates cbkta_globalstates;
    public GameObject prefabKeyCombination;
    public GameObject parentForKeyCombination;

    private System.Random randomSystem;

    private int lastRandomValue = -1;
    private int sameRandomValueCount = 0;
    private int limitRandomValueCount = 0;

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

    void Awake()
    {
        this.randomSystem = new System.Random(System.Guid.NewGuid().GetHashCode());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.run) return;

        if (this.fightArrowControllerObjectsIndex < this.fightArrowControllerObjects.Count)
        {
            bool isKeyPressedCorrectly = this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].IsKeyPressedCorrectly();

            if (isKeyPressedCorrectly)
            {
                this.fightArrowControllerObjects[this.fightArrowControllerObjectsIndex].Explosion();
                this.fightArrowControllerObjectsIndex++;
            }
        }
        else
        {
            if (this.fightTimeCount == this.fightTimes && this.fightArrowControllerObjects.Last().isAnimationEnded)
            {
                //buat untuk exit dari fight

                //trigger ke fungsi OnDisable()
                gameObject.SetActive(false);
            }
            else if (this.fightArrowControllerObjects.Last().isAnimationEnded)
            {
                //buat untuk melanjutkan ke sesi fight berikutnya

                this.DestroyObjects();
                this.SpawnObjects();

                this.fightTimeCount++;
            }

            //setup untuk berhenti atau lanjut ke berikutnya, bergantung terhadap nilai berapa banyak kali
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
