using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private bool _pauseActive = false;
    private float _transitionDuration = 0.5f;
    private float _pauseTimer = 0f;

    public GameObject _menuObj;

    bool _transitionActive = false;

    private void Start() {
        _menuObj.SetActive(false);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape) && !_pauseActive) {
            Debug.Log($"pause !");
            Pause();
        }

        if (_transitionActive) {
            UpdateTimeTransition();
        }

    }

    void UpdateTimeTransition() {

        if (_pauseActive) {

            Time.timeScale = _pauseTimer / _transitionDuration;
            _pauseTimer -= Time.deltaTime;

            if (_pauseTimer <= 0f) {
                _transitionActive = false;
            }

        } else {

            Time.timeScale = _pauseTimer / _transitionDuration;
            _pauseTimer += Time.deltaTime;

            if (_pauseTimer >= 1f) {
                _transitionActive = false;
            }
        }
    }

    public void Resume() {
        _pauseTimer = 0f;
        _pauseActive = false;
        _transitionActive = true;
        _menuObj.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Pause() {
        _pauseActive = true;
        _pauseTimer = 1f;

        Invoke("PauseDelay", _transitionDuration);
    }

    void PauseDelay() {
        _menuObj.SetActive(true);
    }


}
