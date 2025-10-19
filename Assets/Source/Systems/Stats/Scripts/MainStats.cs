using UnityEngine;

public class MainStats : MonoBehaviour
{
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;

    public MainStatsData[] data;

    void Awake()
    {
        foreach (MainStatsData stats in data)
        {
            if (stats.useRandom)
            {
                System.Random randomSystem = this.cbkta_globallogichelper.GenerateRandomSystem();

                stats.healthPoint = randomSystem.Next(stats.minHealthPoint, stats.maxHealthPoint + 1);
                stats.damagePower = randomSystem.Next(stats.minDamagePower, stats.maxDamagePower + 1);
            }
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
}
