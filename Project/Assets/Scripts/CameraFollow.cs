using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _follow;

    private Vector3 _stave;

    private void Awake()
    {
        _stave = new Vector3(0,0, -10);
    }

    void Update()
    {
        transform.localPosition = _follow.localPosition + _stave;
    }
}
