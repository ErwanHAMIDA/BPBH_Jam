using System.Collections.Generic;
using UnityEngine;

public class S_SnapSentence : MonoBehaviour
{
    [SerializeField] int _piecesNumber;
    [SerializeField] BoxCollider _originalBoxCollider;

    private List<Vector3> _snapPoints = new();

    private void Start()
    {
        Vector3 pieceSize = _originalBoxCollider.size;
        pieceSize.x /= _piecesNumber;

        for (int i = 0; i < _piecesNumber; i++)
        {
            GameObject zone = new GameObject($"SnapZone_{i}");

            zone.transform.SetParent(transform);
            zone.transform.localRotation = Quaternion.identity;

            // Trust ez Math
            float offsetX = -_originalBoxCollider.size.x / 2 + pieceSize.x / 2 + i * pieceSize.x;

            Vector3 localPos = new Vector3(_originalBoxCollider.center.x + offsetX, _originalBoxCollider.center.y, _originalBoxCollider.center.z);

            zone.transform.localPosition = localPos;
            _snapPoints.Add(new Vector3(0,0,0)); //to change for snapPoints center

            BoxCollider box = zone.AddComponent<BoxCollider>();
            box.size = pieceSize;
            box.isTrigger = true;

            S_SnapZone snapZone = zone.AddComponent<S_SnapZone>();
            snapZone.Init(this, i, transform.TransformPoint(localPos));
        }
    }

    public void Snap(int index, Transform sentence)
    {
        sentence.position = _snapPoints[index];

        //check the sentence here
    }
}