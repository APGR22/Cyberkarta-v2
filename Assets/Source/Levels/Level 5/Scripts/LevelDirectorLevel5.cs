using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel5 : LevelDirectorMain
{
    public cbkta_GlobalLogic cbkta_globallogic;
    public GameObject blackScreen;
    public SpriteRenderer ranuBefore;
    public GameObject ranuAfter;
    public SoundManagerLogic soundManagerLogic;

    private List<LevelDirectorData> eventsRegistered = new List<LevelDirectorData>
    {
        new LevelDirectorData("FightDone"),
        new LevelDirectorData("LastBossDone")
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
            //Hitamkan dan mute dulu, agar latar belakang tidak tampil ketika selesai mengalahkan brain sekaligus suaranya
            this.blackScreen.SetActive(true);
            this.soundManagerLogic.soundBGMMain.Stop();
        }

        if (this.checklist["LastBossDone"] != null)
        {
            this.cbkta_globallogic.NextScene();
        }
    }
}
