using System.Collections;
using UnityEngine;

public class WindSpell : ElementalSpell
{
    [Header("Références")]
    [Tooltip("Transform du joueur pour la direction et l'origine du vent")]
    public Transform playerTransform;

    [Tooltip("(Optionnel) Particle System pour l'effet visuel de la bourrasque")]
    public ParticleSystem windVFX;

    [Header("Forme de la bourrasque")]
    [Tooltip("Longueur du cône de vent devant le joueur")]
    public float gustLength = 8f;

    [Tooltip("Rayon du cône de vent à son extrémité")]
    public float gustRadius = 3f;

    [Tooltip("Layers pris en compte par l'OverlapCapsule (tout par défaut)")]
    public LayerMask detectionMask = ~0;

    [Header("Force de la bourrasque")]
    [Tooltip("Force totale appliquée aux objets")]
    public float pushForce = 18f;

    [Tooltip("Force vers le haut ajoutée à l'impulsion (donne un effet de soulèvement)")]
    public float upwardForce = 4f;

    [Tooltip("Falloff : les objets plus loin reçoivent moins de force (0 = aucun falloff, 1 = falloff total)")]
    [Range(0f, 1f)]
    public float distanceFalloff = 0.6f;

    [Header("Bourrasque progressive")]
    [Tooltip("Activer l'application progressive de la force sur plusieurs frames")]
    public bool useGustWave = true;

    [Tooltip("Durée totale de la bourrasque progressive")]
    public float gustWaveDuration = 0.3f;

    [Tooltip("Nombre d'impulsions pendant la bourrasque")]
    public int gustPulseCount = 3;

    [Header("Forme de la zone d'attraction")]
    public float pullLength = 8f;
    public float pullRadius = 3f;

    [Header("Force d'attraction")]
    public float pullForce = 18f;

    [Tooltip("Force vers le bas ajoutée à l'impulsion (plaque les objets au sol en arrivant)")]
    public float downwardForce = 2f;

    [Tooltip("Stopper les objets une fois arrivés près du joueur")]
    public bool dampOnArrival = true;

    [Tooltip("Distance du joueur à partir de laquelle on freine l'objet")]
    public float arrivalRadius = 1.5f;
    // Tag Unity recherché sur les objets poussables
    private const string PUSHABLE_TAG = "WindPushable";

    public override void CastSpell(bool spellmode)
    {
        Debug.Log("Casting a powerful wind spell!");
        if (playerTransform == null)
        {
            Debug.LogWarning("[WindGustSpell] playerTransform non assigné !");
            return;
        }
        if (spellmode == true) WindPush();
        else WindPull();
    }

    private void WindPush()
    {
        Debug.Log("Executing Wind Push!");
        // Direction horizontale du regard du joueur
        Vector3 forward = new Vector3(
            playerTransform.forward.x,
            0f,
            playerTransform.forward.z
        ).normalized;

        // Centre du cône : à mi-chemin devant le joueur
        Vector3 capsuleCenter = playerTransform.position + forward * (gustLength * 0.5f);

        // On cherche tous les colliders dans une capsule représentant le cône de vent
        Collider[] hits = Physics.OverlapCapsule(
            playerTransform.position,
            capsuleCenter + forward * (gustLength * 0.5f),
            gustRadius,
            detectionMask
        );

        // Jouer l'effet visuel si assigné
        if (windVFX != null)
        {
            windVFX.transform.position = playerTransform.position;
            windVFX.transform.rotation = Quaternion.LookRotation(forward);
            windVFX.Play();
        }

        // Filtrer uniquement les WindPushable avec un Rigidbody
        foreach (Collider col in hits)
        {
            if (!col.CompareTag(PUSHABLE_TAG)) continue;

            Rigidbody rb = col.attachedRigidbody;
            if (rb == null || rb.isKinematic) continue;

            // Vérifier que l'objet est bien DEVANT le joueur (et pas derrière)
            Vector3 toObject = (col.transform.position - playerTransform.position).normalized;
            if (Vector3.Dot(forward, toObject) < 0f) continue;

            float distance = Vector3.Distance(playerTransform.position, col.transform.position);
            float falloff = Mathf.Lerp(1f, 1f - distanceFalloff, distance / gustLength);
            float finalForce = pushForce * falloff;

            // Direction de poussée : forward + légère composante vers le haut
            Vector3 pushDir = (forward + Vector3.up * (upwardForce / pushForce)).normalized;

            if (useGustWave)
                StartCoroutine(ApplyGustWave(rb, pushDir, finalForce));
            else
                rb.AddForce(pushDir * finalForce, ForceMode.Impulse);
        }
    }

    private IEnumerator ApplyGustWave(Rigidbody rb, Vector3 direction, float totalForce)
    {
        float forcePerPulse = totalForce / gustPulseCount;
        float interval = gustWaveDuration / gustPulseCount;

        for (int i = 0; i < gustPulseCount; i++)
        {
            if (rb == null) yield break;

            rb.AddForce(direction * forcePerPulse, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
        }
    }
    private void WindPull()
    {
        Debug.Log("Executing Wind Pull!");
        Vector3 forward = new Vector3(
        playerTransform.forward.x,
        0f,
        playerTransform.forward.z
        ).normalized;

        Vector3 capsuleEnd = playerTransform.position + forward * pullLength;

        Collider[] hits = Physics.OverlapCapsule(
            playerTransform.position,
            capsuleEnd,
            pullRadius,
            detectionMask
        );

        if (windVFX != null)
        {
            // VFX orienté vers le joueur (sens inverse du vent)
            windVFX.transform.position = playerTransform.position + forward * pullLength;
            windVFX.transform.rotation = Quaternion.LookRotation(-forward);
            windVFX.Play();
        }

        foreach (Collider col in hits)
        {
            if (!col.CompareTag(PUSHABLE_TAG)) continue;

            Rigidbody rb = col.attachedRigidbody;
            if (rb == null || rb.isKinematic) continue;

            // L'objet doit être devant le joueur
            Vector3 toObject = (col.transform.position - playerTransform.position).normalized;
            if (Vector3.Dot(forward, toObject) < 0f) continue;

            float distance = Vector3.Distance(playerTransform.position, col.transform.position);

            // Falloff inversé : plus l'objet est LOIN, plus il reçoit de force
            // pour compenser la distance et arriver avec une vitesse cohérente
            float falloff = Mathf.Lerp(1f - distanceFalloff, 1f, distance / pullLength);
            float finalForce = pullForce * falloff;

            // Direction vers le joueur + légère composante vers le bas
            Vector3 toPlayer = (playerTransform.position - col.transform.position).normalized;
            Vector3 pullDir = (toPlayer - Vector3.up * (downwardForce / pullForce)).normalized;

            if (useGustWave)
                StartCoroutine(ApplyPullWave(rb, pullDir, finalForce, distance));
            else
                rb.AddForce(pullDir * finalForce, ForceMode.Impulse);
        }
    }
    private IEnumerator ApplyPullWave(Rigidbody rb, Vector3 direction, float totalForce, float startDistance)
    {
        float forcePerPulse = totalForce / gustPulseCount;
        float interval = gustWaveDuration / gustPulseCount;

        for (int i = 0; i < gustPulseCount; i++)
        {
            if (rb == null) yield break;

            // Recalcule la direction à chaque impulsion (l'objet bouge)
            Vector3 toPlayer = (playerTransform.position - rb.position).normalized;
            Vector3 currentDir = (toPlayer - Vector3.up * (downwardForce / pullForce)).normalized;

            rb.AddForce(currentDir * forcePerPulse, ForceMode.Impulse);
            yield return new WaitForSeconds(interval);
        }

        // Freinage à l'arrivée
        if (dampOnArrival && rb != null)
            yield return StartCoroutine(DampOnArrival(rb));
    }

    private IEnumerator DampOnArrival(Rigidbody rb)
    {
        float timeout = 3f;
        float elapsed = 0f;

        while (rb != null && elapsed < timeout)
        {
            elapsed += Time.deltaTime;

            float dist = Vector3.Distance(rb.position, playerTransform.position);
            if (dist <= arrivalRadius)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                yield break;
            }

            yield return null;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Visualise le cône de vent dans la Scene View (Editor uniquement).
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;

        Vector3 forward = new Vector3(
            playerTransform.forward.x,
            0f,
            playerTransform.forward.z
        ).normalized;

        Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.25f);

        // Représentation simplifiée du cône avec des sphères et une ligne
        int steps = 8;
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float radius = Mathf.Lerp(0.1f, gustRadius, t);
            Vector3 pos = playerTransform.position + forward * (gustLength * t);
            Gizmos.DrawWireSphere(pos, radius);
        }

        Gizmos.color = new Color(0.4f, 0.8f, 1f, 0.8f);
        Gizmos.DrawRay(playerTransform.position, forward * gustLength);
    }
#endif
}
