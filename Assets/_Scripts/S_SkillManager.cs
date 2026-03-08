using UnityEngine;

public class S_SkillManager : MonoBehaviour
{
    [Header("Compétences débloquées")]
    [SerializeField] private bool _hasPush = false;
    [SerializeField] private bool _hasJump = false;
    [SerializeField] private bool _hasClimb = false;
    // Ajouter ici les futures compétences :


    private int skillUnlocked = 0;

    public bool HasPush => _hasPush;
    public bool HasJump => _hasJump;
    public bool HasClimb => _hasClimb;

    public void UnlockSkill()
    {
        switch (skillUnlocked)
        {
            case 0:
                UnlockPushRock();
                break;
            case 1:
                UnlockJump();
                break;
            case 2:
                UnlockClimb();
                break;
        }

        skillUnlocked++;
    }

    private void UnlockPushRock()
    {
        _hasPush = true;
        Debug.Log("[SkillManager] Compétence POUSSER débloquée !");
    }

    private void UnlockJump()
    {
        _hasJump = true;
        Debug.Log("[SkillManager] Compétence SAUT débloquée !");
    }

    private void UnlockClimb()
    {
        _hasClimb = true;
        Debug.Log("[SkillManager] Compétence ESCALADE débloquée !");
    }
    // Ajouter ici les méthodes d'activation des futures compétences :
}