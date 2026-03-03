using UnityEngine;
using UnityEngine.InputSystem;

public class S_Movement : MonoBehaviour
{
    [SerializeField] InputActionReference _moveAction;
    [Range(1.0f, 100.0f)]
    [SerializeField] float _speed = 20.0f;

    private Vector2 _direction;

    private void Awake()
    {
    }
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
        Vector3 test = new Vector3(_direction.x, 0, _direction.y); 
        transform.Translate(test * _speed * Time.deltaTime);
    }
}
