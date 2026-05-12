using UnityEngine;

/// <summary>
/// Coin Behavior
/// </summary>
public class Coin : MonoBehaviour
{
    public delegate void OnPickUpCoin();
    public static OnPickUpCoin onPickUpCoin;

    public static int s_coinAmount = 0;

    private void Update() {
        transform.Translate(-Vector3.forward * TrackManager.Instance.speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

            ++TrackManager.Instance.CoinAmount;
            SaveManager.Instantiate.Save
            if (onPickUpCoin != null) {
                onPickUpCoin();
            }

            Destroy(gameObject);
        }
    }
}
