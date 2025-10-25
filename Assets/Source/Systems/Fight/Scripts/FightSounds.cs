using UnityEngine;

public class FightSounds : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;

    public void SFXPlayFightAttackEnemy()
    {
        SoundManagerLogic soundManagerLogic = this.cbkta_globalui.soundManagerLogic;
        SoundSFXMain soundSFXMain = soundManagerLogic.soundSFXMain;

        soundSFXMain.PlayRandomOnRange(soundSFXMain.fightAttack);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
