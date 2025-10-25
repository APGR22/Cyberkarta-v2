using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel3 : LevelDirectorMain
{
    public GameObject drone;
    public GameObject nextLevelEntry;

    private List<LevelDirectorData> eventsRegistered = new List<LevelDirectorData>
    {
        new LevelDirectorData("EnemyCyberkartaFightDone"),
        new LevelDirectorData("InteractionDone")
    };

    private Dictionary<string, LevelDirectorData> checklist;

    protected override List<LevelDirectorData> GetEventsRegistered()
    {
        return this.eventsRegistered;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.checklist = this.GetEmptyChecklistEventsRegistered(this.eventsRegistered);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateChecklistEventsRegistered(this.checklist);

        if (this.checklist["EnemyCyberkartaFightDone"] != null)
        {
            this.drone.SetActive(true);
        }

        if (this.checklist["InteractionDone"] != null)
        {
            this.nextLevelEntry.SetActive(true);
        }
    }
}
