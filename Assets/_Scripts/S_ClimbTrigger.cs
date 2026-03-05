using UnityEngine;


public class S_ClimbTrigger : MonoBehaviour
{
    [Header("Mur")]
    [Tooltip("Normale du mur : direction perpendiculaire au mur pointant VERS le joueur. " +
             "Ex: facade perpendiculaire à l'axe z -> normale = (0, 0, -1).")]
    [SerializeField] Vector3 _wallNormal = Vector3.back;

    [Header("Destination (optionnel)")]
    [Tooltip("Transform vide placé au sommet du mur, où le joueur doit atterrir. " +
             "Si non assigné, la destination est calculée grâce " +
             "aux paramètres _climbAngle et _climbDistance.")]
    [SerializeField] Transform _landingPoint;

    [Header("Direction requise (optionnel)")]
    [Tooltip("Si activé, l'escalade ne se déclenche que si le joueur se dirige vers le mur")]
    [SerializeField] bool _checkDirection = true;
    [Tooltip("Direction requise pour déclencher l'escalade (doit pointer vers le mur)")]
    [SerializeField] Vector3 _requiredDirection = Vector3.forward;
    [Tooltip("Tolérance du checkDirection")]
    [SerializeField, Range(10f, 90f)] float _directionTolerance = 50f;

    private void OnValidate()
    {
        if (_wallNormal != Vector3.zero)
            _wallNormal = _wallNormal.normalized;
        if (_requiredDirection != Vector3.zero)
            _requiredDirection = _requiredDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<S_Movement>(out var movement)) return;
        if (!other.TryGetComponent<S_SkillManager>(out var skills)) return;
        if (!other.TryGetComponent<S_Climb>(out var climb)) return;
        if (!skills.HasClimb) return;

        if (_checkDirection)
        {
            Vector3 playerDir = movement.MoveDirection;
            if (playerDir == Vector3.zero) return;
            if (Vector3.Angle(playerDir, _requiredDirection) > _directionTolerance) return;
        }

        climb.TriggerClimb(_wallNormal, _landingPoint);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Normale du mur
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _wallNormal * 1.2f);

        // Direction requise
        if (_checkDirection)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, _requiredDirection * 1.2f);
        }

        // Landing point
        if (_landingPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_landingPoint.position, 0.2f);
            Gizmos.DrawLine(transform.position, _landingPoint.position);

            // Prévisualisation la courbe
            var player = FindFirstObjectByType<S_Climb>();
            if (player != null)
                player.DrawClimbPreview(transform.position, _landingPoint.position, _wallNormal);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0f, 1f, 0.2f);
        if (TryGetComponent<BoxCollider>(out var box))
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.center, box.size);
        }
    }
#endif
}