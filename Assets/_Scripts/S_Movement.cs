using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(S_SkillManager))]
public class S_Movement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] InputActionReference _moveAction;
    [Range(1.0f, 100.0f)]
    [SerializeField] float _speed = 20.0f;
    [Range(-30.0f, 30.0f)]
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] CharacterController _controller;

    private Vector2 _direction;
    private float _verticalVelocity;
    private bool _movementEnabled = true;

    public Vector3 MoveDirection => new Vector3(_direction.x, 0f, _direction.y);

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _moveAction.action.performed += OnMovementStarted;
        _moveAction.action.canceled += OnMovementCanceled;
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _moveAction.action.performed -= OnMovementStarted;
        _moveAction.action.canceled -= OnMovementCanceled;
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _direction = Vector2.zero;
    }

    private void Update()
    {
        if (!_movementEnabled) return;

        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 move = new Vector3(_direction.x, 0f, _direction.y);
        move.y = _verticalVelocity;
        _controller.Move(move * Time.deltaTime * _speed);
    }

    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
        if (!enabled) _direction = Vector2.zero;
    }
}