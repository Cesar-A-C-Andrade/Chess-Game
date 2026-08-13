using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableUI : MonoBehaviour
{
    [SerializeField]
    Color32 blackHouseColor;

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Debug.Log(transform.GetChild(i * 7 + j).name );
                Debug.Log($"{i}, {j}");
                if ((i + j) % 2 != 0)
                {
                    transform.GetChild(i * 7 + j + i).GetComponent<Image>().color = blackHouseColor;
                }
            }
        }
    }

    
}
