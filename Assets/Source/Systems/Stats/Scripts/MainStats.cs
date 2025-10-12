using UnityEngine;

public class MainStats : MonoBehaviour
{
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;

    public int healthPoint = 5;
    public int damagePower = 1;

    public bool useRandom = false;
    public int minHealthPoint = 0;
    public int maxHealthPoint = 0;
    public int minDamagePower = 0;
    public int maxDamagePower = 0;

    void Awake()
    {
        if (this.useRandom)
        {
            System.Random randomSystem = this.cbkta_globallogichelper.GenerateRandomSystem();

            this.healthPoint = randomSystem.Next(this.minHealthPoint, this.maxHealthPoint+1);
            this.damagePower = randomSystem.Next(this.minDamagePower, this.maxDamagePower+1);
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
