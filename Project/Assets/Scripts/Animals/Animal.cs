using UnityEngine;

public class Animal : MonoBehaviour, ICreature
{
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private float _steeringRadius;

    [SerializeField] protected bool _isRunning;

    protected Transform _runFrom;

    protected Vector3 _steeringPoint;

    private CircleCollider2D _collider;

    private Vector3 _moveTo;

    private int _wallLayerMask;

    public float Speed { get => Time.deltaTime * _moveSpeed * AdditionalMoveSpeed(); set { } }

    public void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");

        _collider = GetComponent<CircleCollider2D>();

        _steeringPoint = transform.localPosition;

        SetTargetPosition();

        WhenStart();
    }

    public void Update()
    {
        float directionRotate = Mathf.Clamp(Vector3.SignedAngle(transform.up, (_moveTo - transform.localPosition).normalized, transform.forward), -1, 1);

        transform.localEulerAngles += new Vector3(0, 0, directionRotate * Time.deltaTime * _rotateSpeed);

        transform.localPosition += transform.up * Speed;

        Debug.DrawLine(transform.localPosition, _moveTo, Color.red);
    }

    private void FixedUpdate()
    {
        if (_isRunning)
        {
            _moveTo = transform.localPosition + RunDirection();
        }
        else
        {
            if (Vector2.Distance(_moveTo, transform.localPosition) <= 1) SetTargetPosition();
        }

        GoFromWall();
    }

    private void SetTargetPosition()
    {
        Debug.Log("Set Target Position");

        _moveTo = _steeringPoint + Random.insideUnitSphere * _steeringRadius;

        _moveTo = new Vector3(_moveTo.x, _moveTo.y, 0);
    }

    private void GoFromWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, transform.up * 2, 2, _wallLayerMask);

        if (hit.collider != null)
        {
            Vector3 point = new Vector3(hit.point.x, hit.point.y, 0);

            _moveTo = point + hit.transform.right * 3;
        }
    }

    protected virtual void WhenStart()
    {
        
    }

    protected virtual float AdditionalMoveSpeed()
    {
        return 1;
    }

    protected virtual Vector3 RunDirection()
    {
        if (_runFrom == null || _runFrom.gameObject.activeSelf == false)
        {
            _steeringPoint = transform.localPosition;

            _isRunning = false;

            return Vector3.zero;
        }

        return (transform.localPosition - _runFrom.localPosition).normalized * 2;
    }

    protected virtual bool TriggerCondition(Collider2D collision)
    {
        return collision.GetComponent<Animal>() || collision.GetComponent<Deer>() || collision.GetComponent<Player>();
    }

    protected virtual void Colliding(Collider2D collision)
    {
        
    }

    public void Dead()
    {
        Debug.Log("DEAD: " + transform.name);

        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name != "Body")
        {
            if (TriggerCondition(collision))
            {
                if (Vector2.Distance(collision.transform.localPosition, transform.localPosition) <= _collider.radius || (_runFrom != null && Vector2.Distance(_runFrom.localPosition, transform.localPosition) <= _collider.radius))
                {
                    if (_isRunning == false)
                    {
                        _runFrom = collision.transform;

                        _isRunning = true;
                    }
                }
                else
                {
                    _steeringPoint = transform.localPosition;

                    _isRunning = false;
                }
            }
            else if (collision.tag == "Walls")
            {
                float x = collision.transform.position.x;
                float y = collision.transform.position.y;

                if (x == 0)
                {
                    if (Mathf.Abs(transform.position.y) > Mathf.Abs(y))
                    {
                        Dead();
                    }
                }
                else if (y == 0)
                {
                    if (Mathf.Abs(transform.position.x) > Mathf.Abs(x))
                    {
                        Dead();
                    }
                }
            }

            Colliding(collision);
        }
    }
}
