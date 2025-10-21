using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel1 : LevelDirectorMain
{
    //jika satu atau lebih event sudah selesai, maka akan trigger event baru
    //misalnya yang direncanakan: jika interaksi NPC selesai, maka muncul portal ke level berikutnya

    public LevelEntryPortal nextScenePortalToCyberkarta;

    private List<LevelDirectorData> eventsRegisteredEnterCyberkartaPortal =
    new List<LevelDirectorData>{
        new("NPCInteractionCompleted"),
    };

    private Dictionary<string, LevelDirectorData> checklistEnterCyberkartaPortal;

    protected override List<LevelDirectorData> GetEventsRegistered() => eventsRegisteredEnterCyberkartaPortal;

    void Init()
    {
        this.checklistEnterCyberkartaPortal = this.GetEmptyChecklistEventsRegistered(this.eventsRegisteredEnterCyberkartaPortal);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Setelah semua Update() selesai dipanggil
    void LateUpdate()
    {
        this.UpdateChecklistEventsRegistered(this.checklistEnterCyberkartaPortal);

        if (this.IsAllEventsHasTriggered(this.checklistEnterCyberkartaPortal))
        {
            this.nextScenePortalToCyberkarta.gameObject.SetActive(true);
        }
    }
}
