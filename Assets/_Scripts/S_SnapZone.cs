using UnityEngine;

public class S_SnapZone : MonoBehaviour
{
    private bool isFilled = false;
    private int index;
    private Vector3 snapPoint;

    private S_SnapSentence parent;

    public void Init(S_SnapSentence parentSentence, int i, Vector3 point)
    {
        parent = parentSentence;
        index = i;
        snapPoint = point;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sentence") && !isFilled)
        {
            isFilled = true;

            Rigidbody rigidbody = other.attachedRigidbody;

            if (rigidbody != null)
            {
                rigidbody.linearVelocity = Vector3.zero;
                Debug.Log("stop !");
            }

            parent.Snap(index, other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sentence") && isFilled)
        {
            isFilled = false;
        }
    }
}