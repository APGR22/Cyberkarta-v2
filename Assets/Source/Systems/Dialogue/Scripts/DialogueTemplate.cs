using UnityEngine;

[RequireComponent(typeof(SceneDataTemplate))]
public class DialogueTemplate : MonoBehaviour
{
    //data field filled on Unity Inspector
    public string[] text;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isEnded">Mendapatkan true jika num-nya merupakan index terakhir</param>
    /// <returns>Mengembalikan "" jika sudah diluar jangkauan</returns>
    public string GetText(int num, out bool isEnded)
    {
        isEnded = num == this.text.Length - 1;

        if (num >= this.text.Length)
        {
            return "";
        }

        return this.text[num];
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
