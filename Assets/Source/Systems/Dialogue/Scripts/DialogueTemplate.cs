using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SceneDataTemplate))]
public class DialogueTemplate : MonoBehaviour
{
    //data field filled on Unity Inspector
    public DialogueData2D[] data;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isEnded">Mendapatkan true jika num-nya merupakan index terakhir</param>
    /// <returns>Mengembalikan null jika sudah diluar jangkauan</returns>
    public DialogueData GetText(int index, int num, out bool isEnded)
    {
        isEnded = num == this.data[index].data.Length - 1;

        if (num >= this.data[index].data.Length)
        {
            return null;
        }

        return this.data[index].data[num];
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
