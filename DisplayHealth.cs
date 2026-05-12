using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    public int health = 50;
    public int maxHealth = 100;
    public Image _image;

    private void Update() {
        float lerp = (float)health / maxHealth;
        _image.fillAmount = lerp;
    }
}
