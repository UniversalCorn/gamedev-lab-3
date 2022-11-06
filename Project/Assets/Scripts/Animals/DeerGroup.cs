using System.Collections.Generic;
using UnityEngine;

public class DeerGroup : MonoBehaviour
{
    [SerializeField] private GameObject _deer;

    [SerializeField, Range(3, 10)] private int _deerCount;

    [SerializeField] private float _steeringRadius;

    [SerializeField] private float _groupRadius;

    [SerializeField] private bool _isRandomDeerCountToSapwn;

    private Vector3 _steeringPoint;

    private  List<Deer> _deers;

    private Queue<bool> _deerScary;

    private bool _isRunning;

    private Vector3 _startPosiiton;

    private Transform _runFrom;

    private void Awake()
    {
        _startPosiiton = transform.localPosition;

        _deerScary = new Queue<bool>();

        SpawnDeers();
    }

    private void SpawnDeers()
    {
        _deers = new List<Deer>();

        if (_isRandomDeerCountToSapwn)
            _deerCount = Random.Range(3, 10);

        for (int i = 0; i < _deerCount; i++)
        {
            Vector2 _pointInSphere = Random.insideUnitSphere * _groupRadius;

            Deer deer = Instantiate(_deer, transform.localPosition + new Vector3(_pointInSphere.x, _pointInSphere.y, 0), Quaternion.identity).GetComponent<Deer>();

            deer.InstantiateVariables(this, _pointInSphere);

            _deers.Add(deer);
        }
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.localPosition, _deers[0].transform.localPosition) <= _groupRadius) Steering();

        if (_isRunning)
        {
            _steeringPoint = _deers[0].transform.localPosition + (_deers[0].transform.localPosition - _runFrom.localPosition).normalized * 2;

            transform.localPosition = _steeringPoint;
        }
    }

    private void Steering()
    {
        if (_isRunning == false)
        {
            _steeringPoint = _startPosiiton + Random.insideUnitSphere * (_steeringRadius + 2);

            _steeringPoint = new Vector3(_steeringPoint.x, _steeringPoint.y, 0);

            transform.localPosition = _steeringPoint;
        }
    }

    public void WallCollidet(Vector3 wallPoint, Vector3 wallRight)
    {
        _steeringPoint = wallPoint + (wallRight * 4);

        transform.localPosition = _steeringPoint;
    }

    public float GroupSpeed(Vector2 deerPosition, Vector2 _deerTarget)
    {
        float speed = 1;

        //float distance = Vector2.Distance(deerPosition, transform.localPosition);
        float distance = Vector2.Distance(deerPosition, _deerTarget);

        if (distance <= 2)
        {
            speed = distance / 2;
        }

        return speed;
    }

    public void DeerScary(Transform runFrom)
    {
        _deerScary.Enqueue(true);

        _isRunning = true;

        if(_runFrom == null)
            _runFrom = runFrom;
    }

    public void DeerNotScary()
    {
        if(_deerScary.Count > 0)
            _deerScary.Dequeue();

        _isRunning = _deerScary.Count > 0;

        if (_isRunning == false)
        {
            if (_deers[0] == null) return;

            _startPosiiton = _deers[0].transform.localPosition;

            _runFrom = null;
        }
    }

    public void DeerDead(Deer deerDead)
    {
        _deers.Remove(deerDead);

        if (_deers.Count <= 0)
            Destroy(gameObject);
    }
}
