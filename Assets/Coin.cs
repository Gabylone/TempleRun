using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Coin Behavior
/// </summary>
public class Coin : MonoBehaviour
{
    public delegate void OnPickUpCoin();
    public static OnPickUpCoin onPickUpCoin;

    bool _hasBeenCatched = false;

    private void Update() {
        transform.Translate(-Vector3.forward * TrackManager.Instance.speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player" && !_hasBeenCatched) {

            _hasBeenCatched = true;

            float dur = 0.5f;
            transform.DOMove(transform.position + Vector3.up * 1f, dur/2f).SetEase(Ease.InBounce);
            transform.DOScale(Vector3.zero, dur/2f).SetEase(Ease.InBounce).SetDelay(dur/2F);

            Invoke("CatchCoin", dur);

            //CatchCoin();
        }
    }

    private void CatchCoin() {
        ++TrackManager.Instance.CoinAmount;

        SaveManager.Instance.Save();

        if (onPickUpCoin != null) {
            onPickUpCoin();
        }

        Destroy(gameObject);
    }
}
