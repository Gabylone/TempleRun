using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI score_Text;


    private void Start() {
        Debug.Log($"Mise à jour ui");
        UpdateUI();
    }

    private void OnEnable() {
        Coin.onPickUpCoin += UpdateUI;
    }

    private void OnDisable() {
        Coin.onPickUpCoin -= UpdateUI;
    }

    private void UpdateUI() {
        UpdateUI(TrackManager.Instance.CoinAmount);
    }

    private void UpdateUI(int i) {
        score_Text.text = $"Score : {i}";
    }

}
