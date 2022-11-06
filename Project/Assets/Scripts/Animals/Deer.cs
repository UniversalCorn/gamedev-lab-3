using UnityEngine;

public class Deer : MonoBehaviour, ICreature
{
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private bool _isRunning;

    private DeerGroup _deerGroup;

    private Transform _runFrom;

    private CircleCollider2D _collider;

    private Vector3 _positionInSphere;

    private Vector3 _moveTo;

    private int _wallLayerMask;

    public float Speed { get => Time.deltaTime * _moveSpeed * AdditionalMoveSpeed(); set { } }

    public void Start()
    {
        _collider = GetComponent<CircleCollider2D>();

        _wallLayerMask = LayerMask.GetMask("Wall");
    }

    public void Update()
    {
        if (_deerGroup != null)
        {
            float signedAngle = Mathf.Clamp(Vector3.SignedAngle(transform.up, (_moveTo - transform.localPosition).normalized, transform.forward), -1, 1);

            transform.localEulerAngles += new Vector3(0, 0, signedAngle * Time.deltaTime * _rotateSpeed);

            transform.localPosition += transform.up * Time.deltaTime * _moveSpeed * AdditionalMoveSpeed();

            Debug.DrawLine(transform.localPosition, _moveTo, Color.red);
        }
    }

    private void FixedUpdate()
    {
        _moveTo = _deerGroup.transform.localPosition + _positionInSphere;

        GoFromWall();
    }

    private void GoFromWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, transform.up * 2, 2, _wallLayerMask);

        if (hit.collider != null)
        {
            Vector3 point = new Vector3(hit.point.x, hit.point.y, 0);

            _deerGroup.WallCollidet(point, hit.transform.right);
        }
    }

    public void InstantiateVariables(DeerGroup deerGroup, Vector2 positionInGroup)
    {
        _deerGroup = deerGroup;

        _positionInSphere = positionInGroup;
    }

    private float AdditionalMoveSpeed()
    {
        return _isRunning ? 1 : _deerGroup.GroupSpeed(transform.localPosition, _deerGroup.transform.localPosition + _positionInSphere);
    }

    public void Dead()
    {
        _deerGroup.DeerDead(this);

        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.name != "Body")
        {
            if (collision.GetComponent<Wolf>() || collision.GetComponent<Player>())
            {
                if (Vector2.Distance(collision.transform.localPosition, transform.localPosition) <= _collider.radius || (_runFrom != null && Vector2.Distance(_runFrom.localPosition, transform.localPosition) <= _collider.radius))
                {
                    if (_isRunning == false)
                    {
                        _runFrom = collision.transform;

                        _deerGroup.DeerScary(_runFrom);

                        _isRunning = true;
                    }
                }
                else
                {
                    _deerGroup.DeerNotScary();

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
        }
    }
}
