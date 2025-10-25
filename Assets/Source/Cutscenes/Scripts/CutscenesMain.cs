using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenesMain : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalLogic cbkta_globallogic;
    public Image cutsceneRenderer;
    public List<CutsceneData> cutscenes = new();

    private FadeController fadeController;

    private int cutscenesIndex = 0;

    void Init()
    {
        //setup
        this.fadeController = this.cbkta_globalui.fadeController;

        //mempersiapkan
        this.fadeController.value = 1;

        //mulai
        this.Show();
    }

    void Show()
    {
        //setup
        CutsceneData data = this.cutscenes[this.cutscenesIndex];

        //settings

        // set sprite
        this.cutsceneRenderer.sprite = data.cutscene;
        // fade out untuk menampilkan
        this.fadeController.fadeTime = data.fadeOutDurationInSeconds;

        //print(("Show", this.cutscenesIndex));

        this.fadeController.FadeOut(() =>
        {
            //mulai menghitung waktu
            StartCoroutine(this.TimeCounts(data.durationInSeconds, data.fadeInDurationInSeconds));
        });
    }

    IEnumerator TimeCounts(float durationInSeconds, float fadeInDurationInSeconds)
    {
        //time counts

        float timeCounts = 0;
        while (timeCounts < durationInSeconds)
        {
            timeCounts += Time.deltaTime;
            yield return null;
        }

        //waktu sudah habis

        this.cutscenesIndex++;

        //kalau masih ada cutscene, maka akan terus lanjut hingga cutscene habis
        if (this.cutscenesIndex < this.cutscenes.Count)
        {
            //fade in dulu sebelum mulai cutscene berikutnya
            this.fadeController.fadeTime = fadeInDurationInSeconds;

            this.fadeController.FadeIn(this.Show);
        }
        //sudah tidak ada cutscene berikutnya
        else
        {
            this.Terminate();
        }
    }

    /// <summary>
    /// Menyelesaikan dan tutup
    /// </summary>
    /// <remarks>
    /// <a href="https://softwareengineering.stackexchange.com/questions/163004/what-is-the-opposite-of-initialize-or-init">Sumber informasi untuk penamaan</a>
    /// </remarks>
    void Terminate()
    {
        //settings

        this.fadeController.gameObject.SetActive(true);
        this.fadeController.value = 0;

        //fade in lalu menuju ke scene berikutnya
        this.fadeController.FadeIn(this.cbkta_globallogic.NextScene);
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
}
