using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    [SerializeField]
    private Transform[] _lineAnchors;
    [SerializeField]
    private int _lineIndex;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float rotSpeed = 1f;
    [SerializeField]
    private float jumpDuration = 1f;
    [SerializeField]
    private float jumpHeight = 2f;
    private Vector3 _previousPosition;

    // caches
    private Transform _transform;

    // Inputs
    public InputActionReference _leftInput;
    public InputActionReference _rightInput;
    public InputActionReference _jumpInput;

    // State Machine
    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    private State currentState;
    private float _stateTimer;

    public enum State {
        Idle,
        Jumping,
    }

    // delegate & actions = evenements
    private void Start() {
        // setup
        _transform = GetComponent<Transform>();
        _leftInput.action.started += HandleOnLeftInput;
        _rightInput.action.started += HandleOnRightInput;
        _jumpInput.action.started += HandleOnJumpInput;

        Move(_lineIndex);
    }

    private void HandleOnJumpInput(InputAction.CallbackContext context) {
        
    }

    private void HandleOnRightInput(InputAction.CallbackContext context) {
        Move(_lineIndex + 1);
    }

    private void HandleOnLeftInput(InputAction.CallbackContext context) {
        Move(_lineIndex - 1);
    }

    private void Move(int i) {
        _lineIndex = i;
        _lineIndex = Mathf.Clamp(_lineIndex, 0, _lineAnchors.Length - 1);
        StartState(State.Jumping);
    }

    private void Update() {
        UpdateState();
    }

    #region STATE MACHINE

    public void StartState(State state) {

        ExitState(currentState);

        currentState = state;

        switch (currentState) {
            case State.Idle:
                Idle_Start();
                onUpdate = Idle_Update;
                break;
            case State.Jumping:
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
            case State.Jumping:
                Jumping_Exit();
                break;
            default:
                break;
        }
    }

    #region Idle
    private void Idle_Start() {
        Debug.Log($"Idle : Start");

    }
    private void Idle_Update() {
        Debug.Log($"Idle : Update");

    }
    private void Idle_Exit() {
        Debug.Log($"Idle : Exit");

    }
    #endregion

    #region Jumping
    private void Jumping_Start() {
        Debug.Log($"Jump : Start");
        _stateTimer = 0f;
        _previousPosition = _transform.position;
    }
    private void Jumping_Update() {
        Debug.Log($"Jump : Update");

        float t = _stateTimer / jumpDuration;
        _transform.position = Vector3.Lerp(_previousPosition, _lineAnchors[_lineIndex].position, t);
        _stateTimer += Time.deltaTime;

        if (_stateTimer > jumpDuration) {
            StartState(State.Idle);
        }
    }
    private void Jumping_Exit() {
        Debug.Log($"Jump : Exit");

    }
    #endregion

    #endregion


}