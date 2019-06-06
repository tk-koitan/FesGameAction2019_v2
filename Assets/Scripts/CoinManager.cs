using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coinNum;
    [SerializeField]
    private TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = "×" + coinNum.ToString();
    }
}
