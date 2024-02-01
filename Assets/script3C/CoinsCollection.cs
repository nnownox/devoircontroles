using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsCollection : MonoBehaviour
{
    private int Coin = 0;

    public TextMeshProUGUI coinText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "coin")
        {
            Coin++;
            coinText.text = "Coin: " + Coin.ToString();
            Debug.Log(Coin);
            Destroy(other.gameObject);
        }
    }
}
