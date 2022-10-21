using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject projectile;
	public float fireRate=1.5f;
	public float destroyDelay = 8f;

	private float time;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && Time.time > time && (EventSystem.current == null || EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())) {
			GameObject go = (GameObject)Instantiate(projectile,transform.position+ transform.forward,transform.rotation);
			Destroy(go,destroyDelay);
			time=Time.time+fireRate;
		}
	}
}
