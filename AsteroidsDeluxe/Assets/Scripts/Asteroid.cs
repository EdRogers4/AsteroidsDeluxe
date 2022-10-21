using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int size;
    public int startingAxis;
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _startingTarget;

    private void Start()
    {
        _startingTarget = new Vector3(Random.Range(_screenLeft, _screenRight), 0f, Random.Range(_screenTop, _screenBottom));

        switch (startingAxis)
        {
            case 0:
                _startingPosition = new Vector3(Random.Range(_screenLeft, -1f), 0f, _screenTop);
                break;
            case 1:
                _startingPosition = new Vector3(Random.Range(1f, _screenRight), 0f, _screenTop);
                break;
            case 2:
                _startingPosition = new Vector3(_screenRight, 0f, Random.Range(_screenTop, _screenBottom));
                break;
            case 3:
                _startingPosition = new Vector3(Random.Range(1f, _screenRight), 0f, _screenBottom);
                break;
            case 4:
                _startingPosition = new Vector3(Random.Range(_screenLeft, -1f), 0f, _screenBottom);
                break;
            case 5:
                _startingPosition = new Vector3(_screenLeft, 0f, Random.Range(_screenTop, _screenBottom));
                break;
        }

        transform.position = _startingPosition;
        transform.LookAt(_startingTarget);
    }
    
    private void FixedUpdate()
    {
        var step =  _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, step);
    }
}
