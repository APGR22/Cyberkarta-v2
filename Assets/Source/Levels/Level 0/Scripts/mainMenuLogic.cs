using UnityEngine;
using UnityEngine.Rendering.Universal;

public class mainMenuLogic : MonoBehaviour
{
    public GameObject objectTracking;
    public SoundManagerLogic soundManagerLogic;

    void Move()
    {
        this.objectTracking.transform.Translate(Vector2.left * 5f * Time.deltaTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundBGMMain soundBGMMain = this.soundManagerLogic.soundBGMMain;
        soundBGMMain.Play(soundBGMMain.mainMenuBGM);
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
    }
}
