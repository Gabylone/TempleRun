using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public Action onPlayerHit;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Hurt") {
            onPlayerHit?.Invoke();
        }
    }
}
