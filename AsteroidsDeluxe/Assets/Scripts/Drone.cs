using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float _speedMove;
    [SerializeField] private bool _isMoveRight;
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        var step =  _speedMove * Time.deltaTime;
        
        if (_isMoveRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, step);
        }
        else
        {
            transform.Translate(-Vector3.left * _speedMove * Time.deltaTime);
        }
    }
}
