using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheck : MonoBehaviour
{
    public bool isThrust;
    public bool isTurnLeft;
    public bool isTurnRight;
    public bool isShoot;
    public bool isShield;
    
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
        {
            isThrust = true;
        }
        else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.UpArrow)))
        {
            isThrust = false;
        }

        if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            isTurnLeft = true;
        }
        else if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.LeftArrow)))
        {
            isTurnLeft = false;
        }

        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            isTurnRight = true;
        }
        else if ((Input.GetKeyUp(KeyCode.D)) || (Input.GetKeyUp(KeyCode.RightArrow)))
        {
            isTurnRight = false;
        }

        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0)))
        {
            isShoot = true;
        }
        else if ((Input.GetKeyUp(KeyCode.Space)) || (Input.GetMouseButtonUp(0)))
        {
            isShoot = false;
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetMouseButtonDown(1)))
        {
            isShield = true;
        }
        else if ((Input.GetKeyUp(KeyCode.LeftShift)) || (Input.GetMouseButtonUp(1)))
        {
            isShield = false;
        }
    }
}
