using UnityEngine;

public class Player : MonoBehaviour, ICreature
{
    [SerializeField] private float _moveSpeed;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private int _bullets;

    private LineRenderer _lineRenderer;

    private int _layerMask;

    public float Speed { get => Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime; set { } }

    public bool IsHaveBullets { get => _bullets > 0; }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _layerMask = LayerMask.GetMask("Animal");
    }

    private void Update()
    {
        Move();

        Shoot();
    }

    private void Move()
    {
        float vertical = Speed;

        float horizontal = Input.GetAxis("Horizontal") * _rotateSpeed * -Time.deltaTime;

        transform.localPosition += transform.up * vertical;

        transform.localEulerAngles += transform.forward * horizontal;
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsHaveBullets)
        {
            _bullets--;

            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.localPosition, transform.up * 5, 5, _layerMask);

            Debug.DrawLine(transform.localPosition, transform.localPosition + (transform.up * 5), Color.blue, 20);

            _lineRenderer.SetPosition(0, transform.localPosition);
            _lineRenderer.SetPosition(1, transform.localPosition + transform.up * 5);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform != transform && hit[i].transform.name == "Body")
                {
                    Debug.Log("HIT: " + hit[i].transform.parent.name);

                    hit[i].transform.parent.GetComponent<ICreature>().Dead();

                    break;
                }
            }
        }
    }

    public void Dead()
    {
        gameObject.SetActive(false);

        enabled = false;

        Debug.Log("PLAYER DEAD");
    }
}
