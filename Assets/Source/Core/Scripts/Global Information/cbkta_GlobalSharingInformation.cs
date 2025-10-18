using UnityEngine;

public class cbkta_GlobalSharingInformation : MonoBehaviour
{
    public SceneFightDialogDataTemplate dialogDataTemplate;
    public SceneFightFightDataTemplate fightDataTemplate;

    public void SendInformationForFightScene(SceneFightDialogDataTemplate dialogueData, SceneFightFightDataTemplate fightData)
    {
        this.dialogDataTemplate = dialogueData;
        this.fightDataTemplate = fightData;
    }

    public void GetInformationForFightScene(out SceneFightDialogDataTemplate dialogueData, out SceneFightFightDataTemplate fightDataTemplate)
    {
        dialogueData = this.dialogDataTemplate;
        fightDataTemplate = this.fightDataTemplate;
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
