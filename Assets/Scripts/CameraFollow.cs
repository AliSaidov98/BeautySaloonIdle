using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player = null;

    [SerializeField]
    private float _smoothSpeed = 0.1f;

    private Vector3 _offset = Vector3.zero;

    private Vector3 _newPosition;

    private void Start()
    {
        _offset = transform.position - _player.position;
    }

    private void Update()
    {
        _newPosition = _player.position + _offset;
        transform.position = Vector3.Lerp(transform.position, _newPosition, _smoothSpeed);
    }
}
