using UnityEngine;
using UnityEngine.UI;

public class FightStatus : MonoBehaviour
{
    public GameObject background;
    public Slider successSlider;
    public Slider failedSlider;
    public GameObject centerBarObj;

    public void Win()
    {
        int index = 0;

        this.background.transform.SetSiblingIndex(index++);
        this.failedSlider.transform.SetSiblingIndex(index++);
        this.successSlider.transform.SetSiblingIndex(index++);
        this.centerBarObj.transform.SetSiblingIndex(index++);
    }

    public void Lose()
    {
        int index = 0;

        this.background.transform.SetSiblingIndex(index++);
        this.successSlider.transform.SetSiblingIndex(index++);
        this.failedSlider.transform.SetSiblingIndex(index++);
        this.centerBarObj.transform.SetSiblingIndex(index++);
    }

    void ResetValues()
    {
        this.successSlider.value = 0;
        this.failedSlider.value = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        this.ResetValues();
    }

    void OnDisable()
    {
        this.ResetValues();
    }
}
