using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MainStats))]
public class StatsController : MonoBehaviour
{
    private MainStats stats;
    private int statsIndex = 0;

    private List<Action> listOfFuncForAttack = new();

    public int GetStatsIndex()
    {
        return this.statsIndex;
    }

    public void IncrementStatsIndex()
    {
        this.statsIndex++;
    }

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
        StatsController obj_statscontroller = obj.GetComponent<StatsController>();
        int obj_statsindex = obj_statscontroller.GetStatsIndex();

        obj_stats.data[obj_statsindex].healthPoint -= this.stats.data[this.statsIndex].damagePower;
        obj_stats.data[obj_statsindex].healthPoint = this.NormalizeStatsValue(obj_stats.data[obj_statsindex].healthPoint);

        foreach (Action func in listOfFuncForAttack)
        {
            func();
        }

        print($"{obj}: {obj_stats.data[obj_statsindex].healthPoint}");
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
