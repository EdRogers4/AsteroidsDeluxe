using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float _speedThrust;
    [SerializeField] private float _speedThrustMin;
    [SerializeField] private float _speedThrustMax;
    [SerializeField] private float _speedThrustMod;
    [SerializeField] private float _speedTurn;
    [SerializeField] private bool _isThrust;
    [SerializeField] private bool _isTurnLeft;
    [SerializeField] private bool _isTurnRight;
    [SerializeField] private bool _isShield;


    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();

        if (_speedThrust > _speedThrustMin || _speedThrust < _speedThrustMin)
        {
            _speedThrust = _speedThrustMin;
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            _isThrust = true;
        }
        else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.UpArrow)))
        {
            _isThrust = false;
            _speedThrust = _speedThrustMin;
        }
        
        if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            _isTurnLeft = true;
        }
        else if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.LeftArrow)))
        {
            _isTurnLeft = false;
        }
        
        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            _isTurnRight = true;
        }
        else if ((Input.GetKeyUp(KeyCode.D)) || (Input.GetKeyUp(KeyCode.RightArrow)))
        {
            _isTurnRight = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Shoot");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Shield");
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            
        }
        
        if (_isTurnLeft && !_isTurnRight)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -_speedTurn);
        }
        else if (_isTurnRight && !_isTurnLeft)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * _speedTurn);
        }
    }

    private void FixedUpdate()
    {
        if (!_isThrust) return;
        
        _rigidBody.velocity = transform.forward * _speedThrust;

        if (_speedThrust < _speedThrustMax)
        {
            _speedThrust += (_speedThrust * _speedThrustMod);
        }
    }
}
