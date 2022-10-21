using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FlyCamera : MonoBehaviour {
	public float speed = 40f;
	public float turn = 30;
	public float correctiveStrength = 20;

	private Vector3 moveVector;
	private Vector3 rotationVector;

	private void FixedUpdate () {

		if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject ()) {
			//moveVector = new Vector3(Input.GetAxis ("Horizontal") , 0, Input.GetAxis ("Vertical"));
			//rotationVector = new Vector3(Input.GetAxis ("Mouse X"),Input.GetAxis("Mouse Y"),0);
		}

		GetComponent<Rigidbody>().AddRelativeForce (moveVector*speed);
		GetComponent<Rigidbody>().AddTorque (0, rotationVector.x * turn, 0);
		GetComponent<Rigidbody>().AddRelativeTorque (-rotationVector.y * turn, 0, 0);
		
		Vector3 properRight = Quaternion.Euler (0, 0, -transform.localEulerAngles.z) * transform.right;
		Vector3 upRightCorrection = Vector3.Cross (transform.right, properRight);
		GetComponent<Rigidbody>().AddRelativeTorque (upRightCorrection * correctiveStrength);

	}

	public void SetMoveInput(Vector2 input){
		moveVector= new Vector3 (input.x, 0, input.y);
	}
	
	public void SetRotationInput(Vector2 input){
		rotationVector =  new Vector3 (input.x,input.y,0);
	}
}