using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TweenTest : MonoBehaviour
{
    public float duration = 1f;
    public float amount = 0.5f;

    public float bounceAmount = 1.1f;
    public float bounceDuration = 1f;

    private bool _clicked = false;

    public Image image;

    public Ease ease;
    public Ease uiEase;

    public int vibrato = 10;
    public float elasticity = 1f;

    private void OnMouseDown() {

        // obsolete
        GetComponent<Transform>().DOScale(Vector3.one * bounceAmount, bounceDuration).SetEase(Ease.InBounce);
        GetComponent<Transform>().DOScale(Vector3.one, bounceDuration).SetEase(Ease.OutBounce).SetDelay(bounceDuration);

        // punch
        image.GetComponent<Transform>().DOPunchScale(Vector3.one * bounceAmount, bounceDuration, vibrato, elasticity);

        if (_clicked) {
            image.DOColor(Color.green, 0.5f).SetEase(uiEase);
        } else {
            image.DOColor(Color.red, 0.5f).SetEase(uiEase);
        }

        _clicked = !_clicked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // tween
            Vector3 newPos = transform.position + Vector3.up * amount;
            transform.DOMove(newPos, duration).SetEase(ease);
        }
    }
}
