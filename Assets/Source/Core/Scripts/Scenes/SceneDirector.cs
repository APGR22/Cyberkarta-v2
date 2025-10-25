using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalTags cbkta_globaltags;
    public cbkta_GlobalLogic cbkta_globallogic;

    private bool run = false;
    private bool exit = false;

    private int sceneIndex = 0;
    private Vector3 playerPositionStayedOn = Vector3.zero;

    //dari informasi luar
    private SceneData[] listSceneData = null;

    /// <summary>
    /// untuk dialogue
    /// </summary>
    private int dialogueIndex = 0;
    /// <summary>
    /// untuk fight
    /// </summary>
    private int fightIndex = 0;
    /// <summary>
    /// untuk SecondEnvironment
    /// </summary>
    private int secondEnvironmentIndex = 0;

    private bool forceNextScene = false;

    void EnterScene()
    {
        this.cbkta_globalui.cinemachineCamera.enabled = false;

        Vector3 playerPosition = this.cbkta_globalobjects.player.transform.position;
        Vector3 objectPosition = this.cbkta_globalobjects.playerTriggeredWithObject.transform.position;
        Vector3 camPosition = this.cbkta_globalui.cam.transform.position;

        float xCenter = objectPosition.x - ((objectPosition.x - playerPosition.x) / 2);

        Vector3 camNewPosition = new Vector3(xCenter, camPosition.y, camPosition.z);

        this.cbkta_globalui.cam.transform.position = camNewPosition;

        this.cbkta_globallogic.FreezePlayer();
    }

    void ExitScene()
    {
        this.cbkta_globalui.cinemachineCamera.enabled = true;

        this.cbkta_globalui.controls.Player.Enable();

        this.cbkta_globallogic.UnfreezePlayer();
    }

    void EnterDialogueScene()
    {
        Dialogue dialog = this.cbkta_globalui.dialog.GetComponent<Dialogue>();
        dialog.dialogueIndex = this.dialogueIndex;

        this.cbkta_globalui.dialog.SetActive(true);

        this.cbkta_globalstates.isEnterDialog = true;
    }

    void ExitDialogueScene()
    {
        this.cbkta_globalui.dialog.SetActive(false);

        this.cbkta_globalstates.isEnterDialog = false;
        this.dialogueIndex++; //dialog berikutnya jika masih ada dialog lagi di daftar scene
    }

    void EnterFightScene()
    {
        Fight fight = this.cbkta_globalui.fight.GetComponent<Fight>();
        fight.fightIndex = this.fightIndex;

        this.cbkta_globalui.fight.SetActive(true);
    }

    void ExitFightScene()
    {
        StatsController enemy = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<StatsController>();

        this.cbkta_globalui.fight.SetActive(false);

        this.fightIndex++; //fight berikutnya jika masih ada fight lagi di daftar scene
        enemy.IncrementStatsIndex();
    }

    void EnterSecondEnvironmentScene()
    {
        //setup

        GameObject obj = this.cbkta_globalobjects.playerTriggeredWithObject;
        SecondEnvironmentsDataTemplate secondEnvironmentsDataTemplate = obj.GetComponent<SecondEnvironmentsDataTemplate>();

        GameObject secondEnvironments = this.cbkta_globalui.secondEnvironments;
        SecondEnvironmentsController secondEnvironmentsController = secondEnvironments.GetComponent<SecondEnvironmentsController>();

        //setting
        this.cbkta_globalstates.isSceneInterrupted = true;
        secondEnvironmentsController.environmentData = secondEnvironmentsDataTemplate.environmentData[this.secondEnvironmentIndex];

        //start
        this.cbkta_globalui.fadeController.FadeIn(() =>
        {
            this.cbkta_globalui.secondEnvironments.SetActive(true);
            this.cbkta_globalui.fadeController.FadeOut(() =>
            {
                //done
                this.cbkta_globalstates.isSceneInterrupted = false;
            });
        });

        this.secondEnvironmentIndex++; //untuk environment berikutnya jika ada
    }

    void ExitSecondEnvironmentScene()
    {
        //setup
        GameObject secondEnvironments = this.cbkta_globalui.secondEnvironments;

        //setting
        this.cbkta_globalstates.isSceneInterrupted = true;

        //start
        this.cbkta_globalui.fadeController.FadeIn(() =>
        {
            this.cbkta_globalui.secondEnvironments.SetActive(false);
            this.cbkta_globalui.fadeController.FadeOut(() =>
            {
                //done
                this.cbkta_globalstates.isSceneInterrupted = false;
            });
        });
    }

    bool CompareWithSceneTags(GameObject obj)
    {
        if (obj.CompareTag(this.cbkta_globaltags.dialogTrigger)) return true;
        if (obj.CompareTag(this.cbkta_globaltags.enemy)) return true;
        if (obj.CompareTag(this.cbkta_globaltags.npc)) return true;

        return false;
    }

    void Awake()
    {
        this.ExitDialogueScene();
        this.dialogueIndex = 0; //reset karena ditambah otomatis oleh ExitDialogueScene
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //setup

        GameObject player = this.cbkta_globalobjects.player;
        GameObject obj = this.cbkta_globalobjects.playerTriggeredWithObject;

        //deteksi jika men-trigger sesuatu sesuai scene tags
        bool isSomethingTriggered = false;
        if (obj != null)
        {
            SceneDataTemplate objSceneDataTemplate = obj.GetComponent<SceneDataTemplate>();

            if (objSceneDataTemplate != null) isSomethingTriggered = this.CompareWithSceneTags(obj) && !objSceneDataTemplate.hasScene;
        }

        //jika sesuatu sudah selesai dan sudah keluar dari trigger
        if (!isSomethingTriggered)
        {
            this.exit = false;
        }

        //jika sudah mencapai akhir
        if (this.listSceneData != null ? this.sceneIndex == this.listSceneData.Length : false)
        {
            this.exit = true;
        }

        //---

        //pertama kali
        if (!run && isSomethingTriggered && !exit)
        {
            this.listSceneData = obj.GetComponent<SceneDataTemplate>().scenes;
            this.sceneIndex = 0;

            switch (this.listSceneData[this.sceneIndex])
            {
                case SceneData.Dialog:
                    //(selain DialogTrigger) jika dialog dan belum interaksi, skip hingga ada interaksi.
                    if (
                        !obj.CompareTag(this.cbkta_globaltags.dialogTrigger)
                        &&
                        !this.cbkta_globalstates.isInteractionTrigger
                        )
                        return;

                    this.EnterDialogueScene();
                    break;
                case SceneData.Fight:
                    this.EnterFightScene();
                    break;
                case SceneData.EnterSecondEnvironment:
                    this.EnterSecondEnvironmentScene();
                    break;
                default:
                    break;
            }

            this.EnterScene();

            this.run = true;
            this.exit = false;
            return;
        }

        //---

        //hanya jika saat running, belum exit
        if (run && !exit && !this.cbkta_globalstates.isSceneInterrupted)
        {
            if (this.cbkta_globalstates.isDialogDone || this.cbkta_globalstates.isFightDone || this.forceNextScene)
            {
                this.forceNextScene = false; //langsung ubah ke false

                switch (this.listSceneData[this.sceneIndex])
                {
                    case SceneData.Dialog:
                        this.ExitDialogueScene();
                        this.cbkta_globalstates.isDialogDone = false;
                        break;
                    case SceneData.Fight:
                        this.ExitFightScene(); //meski sudah dilakukan secara internal, tetap pastikan
                        this.cbkta_globalstates.isFightDone = false;
                        break;
                    //tidak perlu exit untuk SecondEnvironment, karena hanya mengubah environment saja
                    default:
                        break;
                }

                this.sceneIndex++;

                //segera hentikan jika index berada di luar jangkauan
                if (this.sceneIndex >= this.listSceneData.Length) return;

                switch (this.listSceneData[this.sceneIndex])
                {
                    case SceneData.Dialog:
                        this.EnterDialogueScene();
                        break;
                    case SceneData.Fight:
                        this.EnterFightScene();
                        break;
                    case SceneData.EnterSecondEnvironment:
                        this.EnterSecondEnvironmentScene();
                        this.forceNextScene = true; //langsung lanjut ke scene berikutnya
                        break;
                    case SceneData.ExitSecondEnvironment: //balik ke environment awal
                        this.ExitSecondEnvironmentScene();
                        this.forceNextScene = true; //langsung lanjut ke scene berikutnya
                        break;
                    default:
                        break;
                }
            }
        }

        //---

        //mau exit, terakhir kali
        if (run && exit)
        {
            //switch (this.listSceneData[this.sceneIndex-1])
            //{
            //    case SceneData.Dialog:
            //        this.ExitDialogueScene();
            //        break;
            //    case SceneData.Fight:
            //        break;
            //    default:
            //        break;
            //}

            //setup

            SceneDataTemplate objSceneDataTemplate = obj.GetComponent<SceneDataTemplate>();

            StatsController playerStatsController = player.GetComponent<StatsController>();
            StatsController objStatsController = obj.GetComponent<StatsController>();

            //settings

            this.dialogueIndex = 0;
            this.fightIndex = 0;

            this.listSceneData = null;
            this.sceneIndex = 0;

            this.run = false;

            objSceneDataTemplate.hasScene = true;

            //kirim sinyal event
            this.cbkta_globalui.levelDirectorMain.SendEvent(
                objSceneDataTemplate.eventNameForLevelDirectorMain,
                this.gameObject,
                this.GetType(),
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                playerStatsController,
                objStatsController
            );

            //keluar

            this.ExitScene();
            return;
        }

        ///*Deteksi trigger*/

        //bool isSomethingTriggered = obj != null;

        //bool isPlayerTriggedDialogue = false;
        //if (isSomethingTriggered)
        //{
        //    isPlayerTriggedDialogue =
        //        (
        //            obj.CompareTag("DialogTrigger")
        //            ||
        //            obj.CompareTag("NonPlayer")
        //        );

        //    if (isPlayerTriggedDialogue)
        //    {
        //        obj.GetComponent<SceneDataTemplate>();
        //    }
        //}

        ////ke-trigger dan dialog belum selesai
        //if (isPlayerTriggedDialogue && !this.cbkta_globalstates.isDialogDone)
        //{
        //    this.cbkta_globalui.dialog.SetActive(true);
        //}
        ////tidak ke-trigger
        //else if (!isPlayerTriggedDialogue)
        //{
        //    this.cbkta_globalui.dialog.SetActive(false);
        //    this.cbkta_globalstates.isDialogDone = false;
        //}
        ////dialog selesai walau masih di dalam trigger
        //else if (this.cbkta_globalstates.isDialogDone)
        //{
        //    this.cbkta_globalui.dialog.SetActive(false);
        //}
    }
}
