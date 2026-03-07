using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(S_Movement))]
public class S_Climb : MonoBehaviour
{
    [Header("Paramètres d'escalade")]

    [Tooltip("Vitesse d'escalade")]
    [Range(0.5f, 20f)]
    [SerializeField] float _climbSpeed = 4f;

    [Tooltip("Distance parcourue le long du mur")]
    [Range(0.5f, 10f)]
    [SerializeField] float _climbDistance = 2f;

    [Tooltip("Angle de la trajectoire d'escalade en degrés. " +
             "90° = montée verticale pure. ")]
    [Range(10f, 90f)]
    [SerializeField] float _climbAngle = 75f;

    [Tooltip("Courbure de la trajectoire. " +
             "0 = ligne droite. " +
             "1 = courbe")]
    [Range(0f, 1f)]
    [SerializeField] float _climbCurve = 0.5f;


    private CharacterController _controller;
    private S_Movement _movement;
    private bool _isClimbing = false;

    public bool IsClimbing => _isClimbing;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _movement = GetComponent<S_Movement>();
    }

    public void TriggerClimb(Vector3 wallNormal, Transform landingPoint = null)
    {
        if (_isClimbing) return;

        Vector3 destination;

        if (landingPoint != null)
        {
            destination = landingPoint.position;
        }
        else
        {
            Vector3 climbDir = -wallNormal;
            climbDir.y = 0f;
            climbDir = climbDir.normalized;

            float angleRad = _climbAngle * Mathf.Deg2Rad;
            float horizontalDist = _climbDistance * Mathf.Cos(angleRad);
            float verticalDist = _climbDistance * Mathf.Sin(angleRad);

            destination = transform.position
                        + climbDir * horizontalDist
                        + Vector3.up * verticalDist;
        }

        StartCoroutine(ClimbArc(destination, wallNormal));
    }

    private IEnumerator ClimbArc(Vector3 target, Vector3 wallNormal)
    {
        _isClimbing = true;
        _movement.SetMovementEnabled(false);
        _controller.enabled = false;

        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / Mathf.Max(_climbSpeed, 0.01f);
        float elapsed = 0f;

        Vector3 p0 = start;
        Vector3 p3 = target;
        Vector3 p1 = p0 + Vector3.up * (distance * _climbCurve);
        Vector3 p2 = p3 + wallNormal * (distance * _climbCurve * 0.5f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            transform.position = CubicBezier(p0, p1, p2, p3, t);
            yield return null;
        }

        transform.position = target;
        _controller.enabled = true;
        _movement.SetMovementEnabled(true);
        _isClimbing = false;
    }

    private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        return uu * u * p0
             + 3f * uu * t * p1
             + 3f * u * tt * p2
             + tt * t * p3;
    }


#if UNITY_EDITOR
    public void DrawClimbPreview(Vector3 startPos, Vector3 targetPos, Vector3 wallNormal)
    {
        float distance = Vector3.Distance(startPos, targetPos);
        Vector3 p0 = startPos;
        Vector3 p3 = targetPos;
        Vector3 p1 = p0 + Vector3.up * (distance * _climbCurve);
        Vector3 p2 = p3 + wallNormal * (distance * _climbCurve * 0.5f);

        int segments = 30;
        UnityEditor.Handles.color = Color.magenta;

        for (int i = 0; i < segments; i++)
        {
            float t0 = (float)i / segments;
            float t1 = (float)(i + 1) / segments;
            UnityEditor.Handles.DrawLine(
                CubicBezier(p0, p1, p2, p3, t0),
                CubicBezier(p0, p1, p2, p3, t1));
        }
    }
#endif
}