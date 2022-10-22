using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;
    [SerializeField] private bool _isDrone;
    private bool _isLaser;

    private void Start()
    {
        if (transform.tag == "Laser")
        {
            _isLaser = true;
        }
    }
    
    private void Update()
    {
        if (transform.position.z > _screenTop)
        {
            WrapPosition(new Vector3(transform.position.x, transform.position.y, _screenBottom));
        }
        else if (transform.position.z < _screenBottom)
        {
            WrapPosition(new Vector3(transform.position.x, transform.position.y, _screenTop));
        }

        if (transform.position.x > _screenRight)
        {
            WrapPosition(new Vector3(_screenLeft, transform.position.y, transform.position.z));
        }
        else if (transform.position.x < _screenLeft)
        {
            WrapPosition(new Vector3(_screenRight, transform.position.y, transform.position.z));
        }
    }

    private void WrapPosition(Vector3 newPosition)
    {
        if (_isLaser)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }

        transform.position = newPosition;

        if (_isLaser)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }
}
