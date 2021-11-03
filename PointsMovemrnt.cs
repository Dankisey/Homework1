using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsMovemrnt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    private Vector2 _startPosition;
    private float _xPosition;
    private float _yPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _yPosition = transform.position.y;
    }

    private void Update()
    {
        _xPosition = Mathf.MoveTowards(transform.position.x, _target.position.x, _speed * Time.deltaTime);
        transform.position = new Vector2(_xPosition, _yPosition);

        if (transform.position.x <= _startPosition.x || transform.position.x >= _target.position.x)
        {
            _speed *= -1;
        }
    }
}
