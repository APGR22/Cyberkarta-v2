using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Digunakan hanya satu object pada setiap level
/// </summary>
/// <remarks>
/// Template member untuk nama event yang akan dikirim ke LevelDirectorMain.
/// <code>
/// [Tooltip("(Opsional) Mengirim sinyal event ke pusat ketika {suatu event tercapai}")]
/// public string eventNameForLevelDirectorMain = "";
/// </code>
/// Misalnya:
/// <code>
/// [Tooltip("(Opsional) Mengirim sinyal event ke pusat ketika dialog selesai")]
/// public string eventNameForLevelDirectorMain = "";
/// </code>
/// <br/>
/// Untuk komentar dalam mengirim sinyal event, ini template-nya.
/// <code>
/// //kirim sinyal event
/// </code>
/// </remarks>
public abstract class LevelDirectorMain : MonoBehaviour
{
    protected List<LevelDirectorData> events = new();

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// On parameter <paramref name="classTypeCaller"/>, use <c>this.GetType()</c> to avoid typos.<br/>
    /// On parameter <paramref name="methodCaller"/>, use <c>System.Reflection.MethodBase.GetCurrentMethod().Name</c> to avoid typos.
    /// </remarks>
    /// <param name="eventName"></param>
    /// <param name="gameObject"></param>
    /// <param name="classTypeCaller">use <c>this.GetType()</c></param>
    /// <param name="methodCaller">use <c>System.Reflection.MethodBase.GetCurrentMethod().Name</c></param>
    /// <param name="playerStatsController"></param>
    /// <param name="enemyStatsController"></param>
    public void SendEvent(
        string eventName,
        GameObject gameObject,
        System.Type classTypeCaller,
        string methodCaller,
        StatsController playerStatsController = null,
        StatsController enemyStatsController = null
    )
    {
        this.events.Add(
            new LevelDirectorData
            {
                eventName = eventName,
                gameObject = gameObject,
                classTypeCaller = classTypeCaller,
                methodNameToCall = methodCaller,
                playerStatsController = playerStatsController,
                enemyStatsController = enemyStatsController
            }
        );
    }

    public Dictionary<string, LevelDirectorData> GetChecklistEventsRegistered()
    {
        List<LevelDirectorData> eventsRegistered = this.GetEventsRegistered();
        Dictionary<string, LevelDirectorData> checklist = this.GetEmptyChecklistEventsRegistered(eventsRegistered);

        this.UpdateChecklistEventsRegistered(checklist);

        return checklist;
    }

    public void UpdateChecklistEventsRegistered(Dictionary<string, LevelDirectorData> checklist)
    {
        foreach (LevelDirectorData eventTrigged in this.events)
        {
            if (checklist.ContainsKey(eventTrigged.eventName))
            {
                checklist[eventTrigged.eventName] = eventTrigged;
            }
        }
    }

    public void RemoveEvent(string key)
    {
        for (int i = 0; i < this.events.Count; i++)
        {
            if (this.events[i].eventName != key) continue;

            this.events.RemoveAt(i);
        }
    }

    /// <summary>
    /// Check if all events registered have been trigged.
    /// </summary>
    /// <remarks>
    /// Will allocate memory on each call. Better to call this method once.
    /// </remarks>
    /// <returns></returns>
    public bool IsAllEventsHasTriggered()
    {
        return this.IsAllEventsHasTriggered(this.GetChecklistEventsRegistered());
    }

    /// <summary>
    /// Check if all events registered have been trigged.
    /// </summary>
    /// <remarks>
    /// Reduce memory allocation by passing checklistEnterCyberkartaPortal from outside.
    /// </remarks>>
    /// <param name="checklist"></param>
    /// <returns></returns>
    public bool IsAllEventsHasTriggered(Dictionary<string, LevelDirectorData> checklist)
    {
        foreach (KeyValuePair<string, LevelDirectorData> item in checklist)
        {
            if (item.Value == null)
            {
                return false;
            }
        }

        return true;
    }

    protected Dictionary<string, LevelDirectorData> GetEmptyChecklistEventsRegistered(List<LevelDirectorData> eventsRegistered)
    {
        Dictionary<string, LevelDirectorData> checklist = new();

        foreach (LevelDirectorData eventRegistered in eventsRegistered)
        {
            checklist[eventRegistered.eventName] = null;
        }

        return checklist;
    }

    protected abstract List<LevelDirectorData> GetEventsRegistered();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
