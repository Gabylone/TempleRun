using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI score_Text;

    private void OnEnable() {
        Coin.onPickUpCoin += HandleOnPickUpCoin;
    }

    private void OnDisable() {
        Coin.onPickUpCoin -= HandleOnPickUpCoin;
    }

    private void HandleOnPickUpCoin() {
        UpdateUI(Coin.s_coinAmount);
    }

    void UpdateUI(int i) {
        score_Text.text = $"Score : {i}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
