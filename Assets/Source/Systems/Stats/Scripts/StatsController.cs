using Unity.Services.Apis.Friends;
using UnityEngine;

[RequireComponent(typeof(MainStats))]
public class StatsController : MonoBehaviour
{
    private MainStats stats;

    void Awake()
    {
        this.stats = GetComponent<MainStats>();
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

        print($"{obj}: {obj_stats.healthPoint}");
    }

    private int NormalizeStatsValue(int value, int maxValue = 100)
    {
        return Mathf.Clamp(value, 0, maxValue);
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
