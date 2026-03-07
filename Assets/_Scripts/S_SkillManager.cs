using UnityEngine;

public class S_SkillManager : MonoBehaviour
{
    [Header("Compétences débloquées")]
    [SerializeField] private bool _hasJump = false;
    [SerializeField] private bool _hasClimb = false;
    // Ajouter ici les futures compétences :


    public bool HasJump => _hasJump;
    public bool HasClimb => _hasClimb;
    public void UnlockJump()
    {
        _hasJump = true;
        Debug.Log("[SkillManager] Compétence SAUT débloquée !");
    }

    public void UnlockClimb()
    {
        _hasClimb = true;
        Debug.Log("[SkillManager] Compétence ESCALADE débloquée !");
    }
    // Ajouter ici les méthodes d'activation des futures compétences :
}