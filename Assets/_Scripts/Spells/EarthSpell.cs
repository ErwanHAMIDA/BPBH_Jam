using System.Collections;
using UnityEngine;

public class EarthSpell : ElementalSpell
{
    [Header("Références")]
    [Tooltip("Le prefab de la plateforme de terre")]
    public GameObject platformPrefab;

    [Tooltip("Transform du joueur (ou de la caméra) pour calculer la direction")]
    public Transform playerTransform;

    [Header("Paramètres de détection")]
    [Tooltip("Distance devant le joueur où la plateforme doit apparaître")]
    public float spawnDistance = 3f;

    [Tooltip("Hauteur depuis laquelle le raycast part (au-dessus du sol)")]
    public float raycastOriginHeight = 5f;

    [Tooltip("Longueur maximale du raycast vers le bas")]
    public float raycastMaxDistance = 20f;

    [Header("Paramètres d'animation")]
    [Tooltip("Profondeur sous le sol d'où la plateforme commence à monter")]
    public float startDepth = 2f;

    [Tooltip("Durée de l'animation de montée en secondes")]
    public float riseDuration = 0.6f;

    [Tooltip("Hauteur cible au-dessus du point d'impact (0 = au ras du sol)")]
    public float targetHeightOffset = 0f;

    [Tooltip("Courbe d'animation de la montée")]
    public AnimationCurve riseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // Layer mask calculé automatiquement depuis le nom "EarthPlatform"
    private int earthPlatformLayer;
    private int earthPlatformLayerMask;

    void Awake()
    {
        earthPlatformLayer = LayerMask.NameToLayer("EarthPlatform");

        if (earthPlatformLayer == -1)
        {
            Debug.LogError("[EarthPlatformSpell] Le layer 'EarthPlatform' n'existe pas. " +
                           "Crée-le dans Edit > Project Settings > Tags and Layers.");
        }

        earthPlatformLayerMask = 1 << earthPlatformLayer;
    }

    public override void CastSpell(bool spellmode)
    {
        Debug.Log("Casting a powerful Earth spell!");
        if (platformPrefab == null)
        {
            Debug.LogWarning("[EarthPlatformSpell] platformPrefab non assigné !");
            return;
        }

        // Point devant le joueur (ignore l'axe Y pour rester horizontal)
        Vector3 flatForward = new Vector3(
            playerTransform.forward.x,
            0f,
            playerTransform.forward.z
            ).normalized;

        Vector3 spawnCenter = playerTransform.position + flatForward * spawnDistance;

        // Origine du raycast bien au-dessus du sol
        Vector3 rayOrigin = spawnCenter + Vector3.up * raycastOriginHeight;

        // On ne détecte QUE les colliders sur le layer EarthPlatform
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit,
                            raycastMaxDistance, earthPlatformLayerMask))
        {
            Vector3 targetPosition = hit.point + Vector3.up * targetHeightOffset;
            SpawnPlatform(targetPosition);
        }
        else
        {
            Debug.Log("[EarthPlatformSpell] Aucune zone EarthPlatform détectée devant le joueur.");
        }
    }

    private void SpawnPlatform(Vector3 targetPosition)
    {
        // Position de départ sous le sol
        Vector3 startPosition = targetPosition - Vector3.up * startDepth;

        GameObject platform = Instantiate(platformPrefab, startPosition, Quaternion.identity);

        StartCoroutine(RisePlatform(platform, startPosition, targetPosition));
    }

    private IEnumerator RisePlatform(GameObject platform, Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / riseDuration);
            float curvedT = riseCurve.Evaluate(t);

            platform.transform.position = Vector3.Lerp(from, to, curvedT);
            yield return null;
        }

        // S'assure que la plateforme est exactement à la position cible
        platform.transform.position = to;
    }
}
