using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainStats))]
public class StatsController : MonoBehaviour
{
    private MainStats stats;

    private List<Action> listOfFuncForAttack = new();

    public void AddFuncOnAttack(Action func)
    {
        listOfFuncForAttack.Add(func);
    }

    public void RemoveFuncOnAttack(Action func)
    {
        listOfFuncForAttack.Remove(func);
    }

    public void Attack(GameObject obj)
    {
        MainStats obj_stats = obj.GetComponent<MainStats>();
        if (obj_stats == null)
        {
            Debug.LogError($"The object ({obj}) has no MainStats");
            return;
        }

        obj_stats.healthPoint -= this.stats.damagePower;
        obj_stats.healthPoint = this.NormalizeStatsValue(obj_stats.healthPoint);

        foreach (Action func in listOfFuncForAttack)
        {
            func();
        }

        print($"{obj}: {obj_stats.healthPoint}");
    }

    private int NormalizeStatsValue(int value, int maxValue = 100)
    {
        return Mathf.Clamp(value, 0, maxValue);
    }

    void Awake()
    {
        this.stats = GetComponent<MainStats>();
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
