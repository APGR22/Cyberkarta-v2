using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel1 : LevelDirectorMain
{
    private List<LevelDirectorData> eventsRegistered = new();
    private Dictionary<string, LevelDirectorData> checklist;

    protected override List<LevelDirectorData> GetEventsRegistered() => eventsRegistered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.checklist = this.GetEmptyChecklistEventsRegistered(this.eventsRegistered);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateChecklistEventsRegistered(this.checklist);
    }
}
