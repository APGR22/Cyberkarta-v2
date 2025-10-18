using UnityEngine;

public class SceneDirector : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalTags cbkta_globaltags;

    private bool run = false;
    private bool exit = false;

    private int sceneIndex = 0;
    private Vector3 playerPositionStayedOn = Vector3.zero;

    //dari informasi luar
    private SceneData[] listSceneData = null;

    //dari penekanan trigger
    private bool pressE = false;

    /// <summary>
    /// untuk dialogue
    /// </summary>
    private int dialogueIndex = 0;

    void FreezePlayer()
    {
        if (this.playerPositionStayedOn == Vector3.zero)
        {
            this.playerPositionStayedOn = this.cbkta_globalobjects.player.transform.position;
        }

        this.cbkta_globalobjects.player.transform.position = this.playerPositionStayedOn;
    }

    void UnfreezePLayer()
    {
        this.playerPositionStayedOn = Vector3.zero;
    }

    void EnterScene()
    {
        this.cbkta_globalui.cinemachineCamera.enabled = false;

        Vector3 playerPosition = this.cbkta_globalobjects.player.transform.position;
        Vector3 objectPosition = this.cbkta_globalobjects.playerTriggeredWithObject.transform.position;
        Vector3 camPosition = this.cbkta_globalui.cam.transform.position;

        float xCenter = objectPosition.x - ((objectPosition.x - playerPosition.x) / 2);

        Vector3 camNewPosition = new Vector3(xCenter, camPosition.y, camPosition.z);

        this.cbkta_globalui.cam.transform.position = camNewPosition;

        this.cbkta_globalui.controls.Player.Disable();

        this.FreezePlayer();
    }

    void ExitScene()
    {
        this.cbkta_globalui.cinemachineCamera.enabled = true;

        this.cbkta_globalui.controls.Player.Enable();

        this.UnfreezePLayer();
    }

    void EnterDialogueScene()
    {
        this.cbkta_globalui.dialog.SetActive(true);
        this.cbkta_globalui.dialog.GetComponent<Dialogue>().dialogueIndex = this.dialogueIndex;

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
        this.cbkta_globalui.fight.SetActive(true);
    }

    void ExitFightScene()
    {
        this.cbkta_globalui.fight.SetActive(false);
    }

    bool CompareWithSceneTags(GameObject obj)
    {
        if (obj.CompareTag("DialogTrigger")) return true;
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
        if (this.playerPositionStayedOn != Vector3.zero) this.FreezePlayer();

        //---

        //deteksi jika men-trigger sesuatu sesuai scene tags
        bool isSomethingTriggered = false;
        if (this.cbkta_globalobjects.playerTriggeredWithObject != null)
        {
            GameObject obj = this.cbkta_globalobjects.playerTriggeredWithObject; //sebagai singkatan sementara
            SceneDataTemplate objSceneDataTemplate = obj.GetComponent<SceneDataTemplate>();

            isSomethingTriggered = this.CompareWithSceneTags(obj) && !objSceneDataTemplate.hasScene;
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
            this.listSceneData = this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<SceneDataTemplate>().scenes;
            this.sceneIndex = 0;

            switch (this.listSceneData[this.sceneIndex])
            {
                case SceneData.Dialog:
                    //jika dialog dan belum interaksi, skip ke berikut saja
                    if (!this.cbkta_globalstates.isInteractionTrigger) return;

                    this.EnterDialogueScene();
                    break;
                case SceneData.Fight:
                    this.EnterFightScene();
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
        if (run && !exit)
        {
            if (this.cbkta_globalstates.isDialogDone || this.cbkta_globalstates.isFightDone)
            {
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

            this.dialogueIndex = 0;

            this.ExitScene();

            this.listSceneData = null;
            this.sceneIndex = 0;

            this.run = false;

            this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<SceneDataTemplate>().hasScene = true;
            return;
        }

        ///*Deteksi trigger*/

        //bool isSomethingTriggered = this.cbkta_globalobjects.playerTriggeredWithObject != null;

        //bool isPlayerTriggedDialogue = false;
        //if (isSomethingTriggered)
        //{
        //    isPlayerTriggedDialogue =
        //        (
        //            this.cbkta_globalobjects.playerTriggeredWithObject.CompareTag("DialogTrigger")
        //            ||
        //            this.cbkta_globalobjects.playerTriggeredWithObject.CompareTag("Enemy")
        //        );

        //    if (isPlayerTriggedDialogue)
        //    {
        //        this.cbkta_globalobjects.playerTriggeredWithObject.GetComponent<SceneDataTemplate>();
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
