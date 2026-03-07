using UnityEngine;

public class S_Word : MonoBehaviour
{
    [SerializeField] public S_SnapSentence ParentSentence;
    [SerializeField] private SO_Word _word;

    public SO_Word Word => _word;

    void FixedUpdate()
    {
        if (ParentSentence != null)
            ParentSentence.UpdateSnap(this);
    }
}
