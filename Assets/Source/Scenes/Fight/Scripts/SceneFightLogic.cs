using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFightLogic : MonoBehaviour
{
    public cbkta_GlobalTags cbkta_globaltags;
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;
    public cbkta_GlobalScenes cbkta_globalscenes;
    public cbkta_GlobalStates cbkta_globalstates; // added to observe dialog/fight completion

    [Header("Main")]
    public SceneDataTemplate sceneDataTemplate;
    public FightArrowController fightArrowController;
    public SceneFightDialogDataTemplate fightDialogDataTemplate;

    //[Header("Will be filled on run")]
    private SceneFightFightDataTemplate fightFightDataTemplate;

    private cbkta_GlobalSharingInformation cbkta_globalsharinginformation;

    // director state (mirrors SceneDirector behaviour)
    private SceneData[] listSceneData = null;
    private int sceneIndex = 0;
    private bool run = false;
    private bool exit = false;

    // local controllers inside this fight scene
    private Dialogue localDialogueController;
    private Fight localFightController;

    void Init()
    {
        GameObject temp = GameObject.FindWithTag(this.cbkta_globaltags.globalSharingInformation);
        this.cbkta_globalsharinginformation = temp.GetComponent<cbkta_GlobalSharingInformation>();

        SceneFightDialogDataTemplate SFDDTData;
        this.cbkta_globalsharinginformation.GetInformationForFightScene(out SFDDTData, out this.fightFightDataTemplate);

        //GameObject enemy = this.cbkta_globallogichelper.FindGameObjectWithTagInScene(cbkta_globaltags.enemy, SceneManager.GetSceneByName(this.cbkta_globalscenes.fightInSubconcious));

        if (this.fightFightDataTemplate.easyMode)
        {
            this.fightArrowController.minArrowCounts = 3;
            this.fightArrowController.maxArrowCounts = 5;
        }
        else
        {
            this.fightArrowController.minArrowCounts = 8;
            this.fightArrowController.maxArrowCounts = 15;
        }

        // copy dialog data from shared info into this scene's dialog template so Dialogue controller reads it
        if (SFDDTData != null && this.fightDialogDataTemplate != null)
        {
            this.fightDialogDataTemplate.text = SFDDTData.text;
        }

        // cache local controllers that belong to the same scene as this logic
        this.localDialogueController = FindObjectsByType<Dialogue>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(d => d.gameObject.scene == this.gameObject.scene);
        this.localFightController = FindObjectsByType<Fight>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(f => f.gameObject.scene == this.gameObject.scene);

        // prepare director list from assigned template (if any)
        if (this.sceneDataTemplate != null && this.sceneDataTemplate.scenes != null && this.sceneDataTemplate.scenes.Length > 0)
        {
            this.listSceneData = this.sceneDataTemplate.scenes;
            this.sceneIndex = 0;
        }
    }

    void EnterDialogue()
    {
        if (this.localDialogueController != null)
        {
            this.localDialogueController.gameObject.SetActive(true);
        }

        if (this.cbkta_globalstates != null)
        {
            this.cbkta_globalstates.isEnterDialog = true;
        }
    }

    void ExitDialogue()
    {
        if (this.localDialogueController != null)
        {
            this.localDialogueController.gameObject.SetActive(false);
        }

        if (this.cbkta_globalstates != null)
        {
            this.cbkta_globalstates.isEnterDialog = false;
        }
    }

    void EnterFight()
    {
        if (this.localFightController != null)
        {
            this.localFightController.gameObject.SetActive(true);
        }
    }

    void ExitFight()
    {
        if (this.localFightController != null)
        {
            this.localFightController.gameObject.SetActive(false);
        }
    }

    void StartSequenceIfNeeded()
    {
        if (this.run) return;
        if (this.listSceneData == null || this.listSceneData.Length == 0) return;

        // start first scene
        switch (this.listSceneData[this.sceneIndex])
        {
            case SceneData.Dialog:
                this.EnterDialogue();
                break;
            case SceneData.Fight:
                this.EnterFight();
                break;
            case SceneData.Subconscious:
                // Subconscious behaviour in fight scene not implemented here; treat like Fight
                this.EnterFight();
                break;
            default:
                break;
        }

        this.run = true;
        this.exit = false;
    }

    void UpdateSequence()
    {
        if (!this.run || this.exit) return;
        if (this.listSceneData == null) return;

        // wait for dialogue or fight to signal completion via global states
        bool finishedCurrent = false;
        if (this.cbkta_globalstates != null)
        {
            if (this.listSceneData[this.sceneIndex] == SceneData.Dialog && this.cbkta_globalstates.isDialogDone) finishedCurrent = true;
            if (this.listSceneData[this.sceneIndex] == SceneData.Fight && this.cbkta_globalstates.isFightDone) finishedCurrent = true;
        }

        if (!finishedCurrent) return;

        // exit current
        switch (this.listSceneData[this.sceneIndex])
        {
            case SceneData.Dialog:
                this.ExitDialogue();
                if (this.cbkta_globalstates != null) this.cbkta_globalstates.isDialogDone = false;
                break;
            case SceneData.Fight:
            case SceneData.Subconscious:
                this.ExitFight();
                if (this.cbkta_globalstates != null) this.cbkta_globalstates.isFightDone = false;
                break;
            default:
                break;
        }

        // advance
        this.sceneIndex++;

        // if finished all
        if (this.sceneIndex >= this.listSceneData.Length)
        {
            this.exit = true;
            return;
        }

        // enter next
        switch (this.listSceneData[this.sceneIndex])
        {
            case SceneData.Dialog:
                this.EnterDialogue();
                break;
            case SceneData.Fight:
                this.EnterFight();
                break;
            case SceneData.Subconscious:
                this.EnterFight();
                break;
            default:
                break;
        }
    }

    void ExitSequence()
    {
        // final cleanup: mark that this template has been consumed and notify states
        if (this.sceneDataTemplate != null)
        {
            this.sceneDataTemplate.hasScene = true;
        }

        if (this.cbkta_globalstates != null)
        {
            this.cbkta_globalstates.isSceneFightDone = true;
        }

        // optionally disable this whole fight scene root so SceneDirector (or other) can resume / unload
        // Keep minimal: disable this gameObject to trigger OnDisable on components in scene (Fight.OnDisable sets isFightDone).
        this.gameObject.SetActive(false);
    }

    // Unity lifecycle
    void Start()
    {
        // nothing here
    }

    void Update()
    {
        // sequence manager
        if (!this.run && !this.exit)
        {
            this.StartSequenceIfNeeded();
        }

        if (this.run && !this.exit)
        {
            this.UpdateSequence();
        }

        if (this.run && this.exit)
        {
            this.ExitSequence();
        }
    }

    void OnEnable()
    {
        this.Init();
        // ensure states reset for fresh start
        if (this.cbkta_globalstates != null)
        {
            this.cbkta_globalstates.isDialogDone = false;
            this.cbkta_globalstates.isFightDone = false;
            this.cbkta_globalstates.isSceneFightDone = false;
        }

        // if data provided via SceneDataTemplate assign list and begin
        if (this.sceneDataTemplate != null && this.sceneDataTemplate.scenes != null && this.sceneDataTemplate.scenes.Length > 0)
        {
            this.listSceneData = this.sceneDataTemplate.scenes;
            this.sceneIndex = 0;
        }
    }
}
