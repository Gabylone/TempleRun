using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour {

    public enum State {
        Idle,
        Move,
        Jump,
    }

    public static PlayerController Instance;
    /// <summary>
    /// COMPONENTS
    /// </summary>
    private Transform _transform;
    [SerializeField]
    private Animator _animator;

    /// <summary>
    /// LINES
    /// </summary>
    [SerializeField]
    private Transform[] _lineAnchors;
    [SerializeField]
    private int _lineIndex;

    /// <summary>
    /// MAIN PARAMS
    /// </summary>
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _rotSpeed = 1f;
    [SerializeField]
    private float _move_Height = 2f;


    [SerializeField]
    private float _jumping_Duration = 1f;
    [SerializeField]
    private float _jumping_Height = 2f;
    private Vector3 _jumping_StartPosition;

    [SerializeField]
    public PlayerTrigger _playerTrigger;

    /// <summary>
    /// INPUT
    /// </summary>
    [SerializeField]
    private InputActionReference _leftInput;
    [SerializeField]
    private InputActionReference _rightInput;
    [SerializeField]
    private InputActionReference _jumpInput;

    /// <summary>
    /// STATE MACHINE
    /// </summary>
    public delegate void OnUpdate();
    private OnUpdate onUpdate;

    private State currentState;
    private float _stateTimer;

    // delegate & actions = evenements
    private void Start() {
        Init();
        Move(_lineIndex);
    }

    void Init() {
        // setup
        _transform = GetComponent<Transform>();

        _leftInput.action.started += HandleOnLeftInput;
        _rightInput.action.started += HandleOnRightInput;
        _jumpInput.action.started += HandleOnJumpInput;

        _playerTrigger.onPlayerHit += HandleOnPlayerHit;

    }
    private void HandleOnJumpInput(InputAction.CallbackContext context) {
        StartState(State.Jump);
    }

    private void HandleOnRightInput(InputAction.CallbackContext context) {
        Move(_lineIndex + 1);
    }

    private void HandleOnLeftInput(InputAction.CallbackContext context) {
        Move(_lineIndex - 1);
    }

    private void Move(int i) {

        if (currentState == State.Jump) {
            return;
        }

        // change line index
        _lineIndex = i;
        _lineIndex = Mathf.Clamp(_lineIndex, 0, _lineAnchors.Length - 1);

        // change state
        StartState(State.Move);
    }

    private void Update() {
        UpdateState();
    }

    #region collision
    private void HandleOnPlayerHit() {
        Debug.Log($"Player Death");
        SceneManager.LoadScene(0);
    }

    #endregion

    #region STATE MACHINE

    public void StartState(State state) {

        ExitState(currentState);

        currentState = state;

        switch (currentState) {
            case State.Idle:
                Idle_Start();
                onUpdate = Idle_Update;
                break;
            case State.Move:
                Moving_Start();
                onUpdate = Moving_Update;
                break;
            case State.Jump:
                Jumping_Start();
                onUpdate = Jumping_Update;
                break;
            default:
                break;
        }

    }
    private void UpdateState() {
        if (onUpdate != null) {
            onUpdate();
        }
    }
    private void ExitState(State state) {
        switch (state) {
            case State.Idle:
                Idle_Exit();
                break;
            case State.Jump:
                Jumping_Exit();
                break;
            case State.Move:
                Moving_Exit();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Idle
    private void Idle_Start() {
        _animator.SetBool("IsJumping", false);
    }
    private void Idle_Update() {

    }
    private void Idle_Exit() {

    }
    #endregion


    #region Moving
    private void Moving_Start() {
        _animator.SetBool("IsJumping", true);
        _jumping_StartPosition = _transform.position;
        _stateTimer = 0f;

    }
    private void Moving_Update() {
        // lerp de deplacement
        float t = _stateTimer / _jumping_Duration;

        // lerp
        Vector3 horizontalPos = Vector3.Lerp(_jumping_StartPosition, _lineAnchors[_lineIndex].position, t);
        Vector3 verticalPos = Vector3.up * Mathf.Sin(t * Mathf.PI) * _move_Height;
        _transform.position = horizontalPos + verticalPos;

        _stateTimer += Time.deltaTime;

        if (_stateTimer > _jumping_Duration) {
            StartState(State.Idle);
        }
    }
    private void Moving_Exit() {

    }
    #endregion

    #region Jumping
    private void Jumping_Start() {

        _animator.SetBool("IsJumping", true);

        _stateTimer = 0f;
        _jumping_StartPosition = _transform.position;

    }
    private void Jumping_Update() {

        // lerp de deplacement
        float t = _stateTimer / _jumping_Duration;

        // lerp
        Vector3 verticalPos =  Vector3.up * Mathf.Sin(t * Mathf.PI) * _jumping_Height;

        _transform.position = verticalPos;

        _stateTimer += Time.deltaTime;

        if (_stateTimer > _jumping_Duration) {
            StartState(State.Idle);
        }
    }
    private void Jumping_Exit() {

    }
    #endregion

}