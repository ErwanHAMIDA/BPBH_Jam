using UnityEngine;

public class S_PushedObject : MonoBehaviour
{
    [Range(1.0f, 100.0f)]
    [SerializeField] float _force = 1.0f;
    [SerializeField] S_SkillManager _skill;

    private float _pushForce;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_skill == null)
            Debug.LogError("S_SkillManager is null!");
        else
            Debug.Log("HasPush = " + _skill.HasPush);

        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody == null || rigidbody.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        if (_skill.HasPush == false) _pushForce = 1.0f;
        else _pushForce = _force;


        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized;
        float force = _pushForce * hit.controller.velocity.magnitude;
        rigidbody.AddForce(pushDirection * force, ForceMode.Impulse);
    }
}
