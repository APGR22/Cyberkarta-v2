using UnityEngine;

[System.Serializable]
public class LevelDirectorData
{
    /// <summary>
    /// Example: "OnPlayerEnterZone", "OnBossDefeated"
    /// </summary>
    public string eventName;
    public GameObject gameObject;
    public System.Type classTypeCaller;
    public string methodNameToCall;
    public StatsController playerStatsController;
    public StatsController enemyStatsController;

    public LevelDirectorData(
        string eventName,
        GameObject gameObject = null,
        System.Type classTypeCaller = null,
        string methodNameToCall = null,
        StatsController playerStatsController = null,
        StatsController enemyStatsController = null
    )
    {
        this.eventName = eventName;
        this.gameObject = gameObject;
        this.classTypeCaller = classTypeCaller;
        this.methodNameToCall = methodNameToCall;
        this.playerStatsController = playerStatsController;
        this.enemyStatsController = enemyStatsController;
    }

    public LevelDirectorData() { }
}
