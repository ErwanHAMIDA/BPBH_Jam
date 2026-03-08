using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class S_Word : MonoBehaviour
{
    [SerializeField] public S_SnapSentence _parentSentence;
    [SerializeField] private SO_Word _word;
    [SerializeField] private TMP_Text _textMesh;
    [SerializeField] private Transform _visual;

    [SerializeField] private BoxCollider _box;

    public SO_Word Word => _word;

    private Camera _cam;


    private void Awake()
    {
        if (_box == null)
            _box = GetComponent<BoxCollider>();

        if (_word != null && _textMesh != null)
        {
            _textMesh.text = _word.GetWord();

            _textMesh.ForceMeshUpdate();

            UpdateColliderSize();

        }
    }
    private void Start()
    {
        _cam = Camera.main;
    }

    private void UpdateColliderSize()
    {
        Bounds bounds = _textMesh.textBounds;

        Vector3 size = new Vector3(bounds.size.x + 0.2f, 1.0f, bounds.size.y + 1.0f);
        _box.size = size;

        _box.center = Vector3.zero;

        _box.transform.localRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if (_parentSentence != null)
            _parentSentence.UpdateSnap(this);
    }

    void LateUpdate()
    {
        if (_visual != null && _cam != null)
        {
            _visual.rotation = Quaternion.LookRotation(_cam.transform.forward);
        }
    }
}