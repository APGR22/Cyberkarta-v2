using UnityEngine;

public class GameObjectBGM : MonoBehaviour
{
    public SoundManagerLogic soundManagerLogic;
    public SoundMainData bgm = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.bgm != null) this.soundManagerLogic.soundBGMMain.Play(this.bgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
