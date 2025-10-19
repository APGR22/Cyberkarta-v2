using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
//[ExecuteAlways]
public class FadeController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float value = 0f;
    public float fadeTime = 1f;
    public bool startFadeOutWhenAwake = false;

    private SpriteRenderer spriteRenderer;

    private bool isInFade = false;

    [ContextMenu("Fade In")]
    public void FadeIn(Action funcEnd = null)
    {
        if (this.isInFade) return;
        if (this.value == 1) return;

        //Karena awalnya tidak terlihat, jadi diaktifkan dulu (dan harus karena StartCoroutine())
        this.gameObject.SetActive(true);

        StartCoroutine(this.Fade(false, funcEnd));
        this.isInFade = true;
    }

    [ContextMenu("Fade Out")]
    public void FadeOut(Action funcEnd = null)
    {
        if (this.isInFade) return;
        if (this.value == 0) return;

        StartCoroutine(this.Fade(true, () =>
        {
            if (funcEnd != null) funcEnd();

            //Karena tidak terlihat, jadi dinonaktifkan saja
            this.gameObject.SetActive(false);
        }));
        this.isInFade = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fadeout">True, fade out. Otherwise, fade in</param>
    IEnumerator Fade(bool fadeout = false, Action funcEnd = null)
    {
        float fadeTimeCount = 0;
        float fadeValue = 0;
        float fadeTimeCache = this.fadeTime;

        while (fadeTimeCount < this.fadeTime)
        {
            fadeTimeCount += Time.deltaTime;

            fadeValue = fadeTimeCount / fadeTime;
            this.value = (fadeout ? 1 - fadeValue : fadeValue);

            yield return null;
        }

        this.value = fadeout ? 0 : 1;
        this.isInFade = false;

        if (funcEnd != null) funcEnd();
    }

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.startFadeOutWhenAwake) this.FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        this.value = Mathf.Clamp01(value);

        Color color = this.spriteRenderer.color;
        this.spriteRenderer.color = new(color.r, color.g, color.b, this.value);
    }
}
