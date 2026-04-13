using UnityEngine;

public class ElementalSpell : MonoBehaviour
{
    public virtual void CastSpell(bool spellmode)
    {
        Debug.Log("Casting an elemental spell!");
    }
}
