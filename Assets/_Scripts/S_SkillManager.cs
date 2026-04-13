using UnityEngine;

public class S_SkillManager : MonoBehaviour
{
    [Header("Compétences débloquées")]
    [SerializeField] private bool _hasPush = false;
    [SerializeField] private bool _hasJump = false;
    [SerializeField] private bool _hasClimb = false;
    [SerializeField] private bool _EarthActive = false;
    [SerializeField] private bool _WaterActive = false;
    [SerializeField] private bool _WindActive = false;
    [SerializeField] private bool _FireActive = false;
    private bool _isSpellMode1 = true;
    private ElementalSpell currentElementalSpell;
    private int skillUnlocked = -1;

    public bool HasPush => _hasPush;
    public bool HasJump => _hasJump;
    public bool HasClimb => _hasClimb;

    private void Awake()
    {
        _hasPush = false;
        _hasJump = false;
        _hasClimb = false;

        Debug.Log($"[SkillManager] HasPush au démarrage = {_hasPush}");
    }

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

    public void UnlockEarth()
    {
        _EarthActive = true;
        Debug.Log("[SkillManager] Compétence EARTH débloquée !");
        _WaterActive = false;
        _WindActive = false;
        _FireActive = false;
        currentElementalSpell = GetComponent<EarthSpell>();
    }

    public void UnlockWater()
    {
        _WaterActive = true;
        Debug.Log("[SkillManager] Compétence WATER débloquée !");
        _EarthActive = false;
        _WindActive = false;
        _FireActive = false;
        currentElementalSpell = GetComponent<WaterSpell>();
    }

    public void UnlockWind()
    {
        _WindActive = true;
        Debug.Log("[SkillManager] Compétence WIND débloquée !");
        _EarthActive = false;
        _WaterActive = false;
        _FireActive = false;
        currentElementalSpell = GetComponent<WindSpell>();
    }

    public void UnlockFire()
    {
        _FireActive = true;
        Debug.Log("[SkillManager] Compétence FIRE débloquée !");
        _EarthActive = false;
        _WaterActive = false;
        _WindActive = false;
        currentElementalSpell = GetComponent<FireSpell>();
    }
    public void UpdateSpellModeText()
    {
        if (_isSpellMode1)
        {
            _isSpellMode1 = false;
        }
        else
        {
            _isSpellMode1 = true;
        }
    }
    public void CastElementalSpell()
    {
        currentElementalSpell?.CastSpell(_isSpellMode1);
    }
}