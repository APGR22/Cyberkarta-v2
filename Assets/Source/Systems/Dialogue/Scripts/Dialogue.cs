using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalObjects cbkta_globalobjects;
    public TextMeshProUGUI tmpui;
    public Image icon;

    /// <summary>
    /// Informasi oleh SceneDirector
    /// </summary>
    [HideInInspector] public int dialogueIndex = 0;

    private bool hasInit = false;
    private DialogueTemplate dialogue = null;

    //informasi animasi dialog
    public int visibleText = 0;

    private int nextText = 0;
    private float textAnimationDuration = 0.025f;
    private float textAnimationTime = 0;
    private int totalChars = 0;
    private bool isDialogEnded = true;

    void Init()
    {
        this.cbkta_globalui.controls.UI.DialogNext.performed += ctx => //tombol ditekan
        {
            this.NextDialog();
        };
    }

    void ResetDialog()
    {
        this.visibleText = 0;
        this.tmpui.maxVisibleCharacters = 0;
    }

    void TypeText(string text)
    {
        this.tmpui.text = text;
        this.tmpui.ForceMeshUpdate(); // Perlu kah?
    }

    void DialogAnimationNext()
    {
        //hanya jika dialog aktif
        if (!gameObject.activeInHierarchy) return;

        //menganimasikan pengetikan teks
        if (this.textAnimationTime < this.textAnimationDuration)
        {
            this.textAnimationTime += Time.deltaTime;
            return;
        }

        //reset
        this.textAnimationTime = 0;

        //increment
        this.visibleText++;

        //pastikan dan paksa hanya di dalam range panjang data asli
        this.visibleText = Mathf.Clamp(this.visibleText, 0, this.totalChars);

        //update
        tmpui.maxVisibleCharacters = this.visibleText;
    }
    public void NextDialog(bool skip = true)
    {
        //ketika enter, kalau sudah selesai dialognya, maka dinyatakan selesai
        if (this.isDialogEnded)
        {
            this.cbkta_globalstates.isDialogDone = true;
            return;
        }

        //kalau belum selesai, langsung tampilkan semuanya, kalau mau skip
        if (this.visibleText < this.totalChars && skip)
        {
            this.visibleText = this.totalChars;
            return;
        }
        this.visibleText = 0; //reset karena ini data berikutnya

        bool isEnded = false;
        DialogueData data = this.dialogue.GetText(this.dialogueIndex, this.nextText, out isEnded);

        this.TypeText(data.text);
        this.icon.sprite = data.icon;

        this.nextText++;

        if (isEnded)
        {
            this.nextText = 0;
            this.isDialogEnded = true;
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.totalChars = this.tmpui.textInfo.characterCount;
        this.DialogAnimationNext();
    }

    void OnEnable()
    {
        if (!hasInit)
        {
            this.Init();

            this.hasInit = true;
        }

        this.cbkta_globalui.controls.UI.DialogNext.Enable();

        GameObject obj = this.cbkta_globalobjects.playerTriggeredWithObject;
        this.dialogue = obj.GetComponent<DialogueTemplate>();

        //ada bug dimana kalau terlalu cepat pergantian enable dan disable, maka tidak bekerja
        //jika sudah di bagian akhir tapi belum enter lagi, maka akan terulang ketika masuk kembali
        if (this.isDialogEnded)
        {
            //mengulang dan memulai ulang dialog
            this.ResetDialog();
            this.isDialogEnded = false;
            this.NextDialog(false);
        }
    }

    void OnDisable()
    {
        //this.ResetDialog();
        //this.NextDialog(false);

        this.cbkta_globalui.controls.UI.DialogNext.Disable();
    }
}

//class Dialogue_1 : MonoBehaviour
//{
//    public cbkta_GlobalUI cbkta_globalui;
//    public cbkta_GlobalStates cbkta_globalstates;
//    public cbkta_GlobalObjects cbkta_globalobjects;
//    public TextMeshProUGUI tmpui;

//    private bool hasInit = false;
//    private DialogueTemplate dialogue = null;

//    //informasi animasi dialog
//    public int visibleText = 0;

//    private int nextText = 0;
//    private float textAnimationDuration = 0.025f;
//    private float textAnimationTime = 0;
//    private int totalChars = 0;
//    private bool isDialogEnded = true;

//    void Init()
//    {
//        this.cbkta_globalui.controls.UI.DialogNext.performed += ctx => //tombol ditekan
//        {
//            this.NextDialog();
//        };
//    }

//    void NextDialog(bool skip = true)
//    {
//        this.totalChars = this.tmpui.textInfo.characterCount;

//        //kalau belum selesai, tunggu sudah tampilkan semuanya
//        if (this.visibleText < this.totalChars)
//        {
//            //jika mau skip meski belum selesai
//            if (skip)
//            {
//                this.visibleText = this.totalChars;
//            }

//            return;
//        }

//        this.visibleText = 0; //reset karena dialog selanjutnya

//        bool isEnded = false; //cek apakah index sudah berada di akhir
//        string data = this.dialogue.GetText(nextText, out isEnded);

//        this.tmpui.data = data;
//        this.tmpui.ForceMeshUpdate(); // Perlu kah?

//        this.nextText++;
//    }

//    void Start()
//    {
        
//    }

//    void Update()
//    {
//        //menganimasikan pengetikan teks
//        if (this.textAnimationTime < this.textAnimationDuration)
//        {
//            this.textAnimationTime += Time.deltaTime;
//            return;
//        }

//        //reset
//        this.textAnimationTime = 0;

//        //increment
//        this.visibleText++;

//        //pastikan dan paksa hanya di dalam range panjang data asli
//        this.visibleText = Mathf.Clamp(this.visibleText, 0, this.totalChars);

//        //update
//        tmpui.maxVisibleCharacters = this.visibleText;
//    }

//    void OnEnable()
//    {
//        if (!hasInit) this.Init();

//        this.cbkta_globalui.controls.UI.DialogNext.Enable();
//    }

//    void OnDisable()
//    {
//        this.cbkta_globalui.controls.UI.DialogNext.Disable();
//    }
//}