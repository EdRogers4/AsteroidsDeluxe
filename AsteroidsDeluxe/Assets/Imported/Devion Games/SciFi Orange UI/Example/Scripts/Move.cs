using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public Vector3 direction;
	// Update is called once per frame
	void Update () {
		transform.Translate (direction*Time.deltaTime);
	}
}
