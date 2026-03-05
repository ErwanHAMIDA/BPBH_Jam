using UnityEngine;

public class S_SkillManager : MonoBehaviour
{
    [Header("Compétences débloquées")]
    [SerializeField] private bool _hasJump = false;
    // Ajouter ici les futures compétences :


    public bool HasJump => _hasJump;

    public void UnlockJump()
    {
        _hasJump = true;
        Debug.Log("[SkillManager] Compétence SAUT débloquée !");
    }

    // Ajouter ici les méthodes d'activation des futures compétences :
}