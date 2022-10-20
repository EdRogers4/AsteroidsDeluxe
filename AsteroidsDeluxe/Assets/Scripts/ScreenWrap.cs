using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;

    private void Update()
    {
               
        if (transform.position.z > _screenTop)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,_screenBottom);
        }
        else if (transform.position.z < _screenBottom)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,_screenTop);
        }
        
        if (transform.position.x > _screenRight)
        {
            transform.position = new Vector3(_screenLeft, transform.position.y,transform.position.z);
        }
        else if (transform.position.x < _screenLeft)
        {
            transform.position = new Vector3(_screenRight, transform.position.y,transform.position.z);
        } 
    }
}
