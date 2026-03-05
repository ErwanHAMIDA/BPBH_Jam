using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(S_Movement))]
public class S_Jump : MonoBehaviour
{
    [Header("Arc de saut")]
    [Tooltip("Hauteur de l'arc de saut")]
    [SerializeField] float _jumpArcHeight = 1.8f;
    [Tooltip("Durée du saut en secondes")]
    [SerializeField] float _jumpDuration = 0.45f;

    private CharacterController _controller;
    private S_Movement _movement;
    private bool _isJumping = false;

    public bool IsJumping => _isJumping;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _movement = GetComponent<S_Movement>();
    }

    public void TriggerJump(Vector3 landingPoint)
    {
        if (_isJumping) return;
        StartCoroutine(JumpArc(landingPoint));
    }

    private IEnumerator JumpArc(Vector3 target)
    {
        _isJumping = true;
        _movement.SetMovementEnabled(false);
        _controller.enabled = false;

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < _jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _jumpDuration);

            Vector3 pos = Vector3.Lerp(start, target, t);
            pos.y += _jumpArcHeight * Mathf.Sin(t * Mathf.PI);

            transform.position = pos;
            yield return null;
        }

        transform.position = target;
        _controller.enabled = true;
        _movement.SetMovementEnabled(true);
        _isJumping = false;
    }
}