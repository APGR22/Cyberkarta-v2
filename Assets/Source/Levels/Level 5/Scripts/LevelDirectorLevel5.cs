using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel5 : LevelDirectorMain
{
    public GameObject blackScreen;
    public SpriteRenderer ranuBefore;
    public GameObject ranuAfter;

    private List<LevelDirectorData> eventsRegistered = new List<LevelDirectorData>
    {
        new LevelDirectorData("FightDone"),
    };

    private Dictionary<string, LevelDirectorData> checklist;

    protected override List<LevelDirectorData> GetEventsRegistered()
    {
        return this.eventsRegistered;
    }

    private int FightDoneCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         this.checklist = this.GetEmptyChecklistEventsRegistered(this.eventsRegistered);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateChecklistEventsRegistered(this.checklist);

        if (this.checklist["FightDone"] != null)
        {
            this.FightDoneCount++;
            this.RemoveEvent("FightDone");
            this.checklist["FightDone"] = null;

            this.ranuBefore.enabled = false;
            this.ranuAfter.SetActive(true);
        }

        if (this.FightDoneCount >= 2)
        {
            //print("Hitamkan lalu pindah ke cutscene");
            this.blackScreen.SetActive(true);
        }
    }
}
