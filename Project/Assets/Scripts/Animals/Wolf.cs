using UnityEngine;

public class Wolf : Animal
{
    private void Eat(Collider2D collision)
    {
        if (Vector2.Distance(collision.transform.localPosition, transform.localPosition) <= 0.4f)
        {
            collision.transform.GetComponent<ICreature>().Dead();

            Debug.Log("EAT  " + collision.transform.name);

            CancelInvoke("Dead");

            Invoke("Dead", 30);
        }
    }

    protected override void WhenStart()
    {
        Invoke("Dead", 20);
    }

    protected override Vector3 RunDirection()
    {
        if (_runFrom == null)
        {
            _steeringPoint = transform.localPosition;

            _isRunning = false;

            return Vector3.zero;
        }

        float speed = _runFrom.GetComponent<ICreature>().Speed;

        return ((_runFrom.localPosition + _runFrom.up * speed) - transform.localPosition).normalized * 2;
    }

    protected override bool TriggerCondition(Collider2D collision)
    {
        return collision.GetComponent<Rabbit>() || collision.GetComponent<Deer>() || collision.GetComponent<Player>();
    }

    protected override void Colliding(Collider2D collision)
    {
        if (TriggerCondition(collision))
        {
            Eat(collision);
        }
    }
}
