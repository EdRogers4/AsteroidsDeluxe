using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameManager scriptGameManager;
    public Transform target;
    public bool isMoveRight;
    [SerializeField] private float _speedMove;

    private void Start()
    {
        
    }
    
    private void Update()
    {
        transform.LookAt(target);
        var step =  _speedMove * Time.deltaTime;
        
        if (isMoveRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, step);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Shield") return;
        
        if (collision.transform.tag == "Laser")
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Laser>().DestroyLaser();
        }
        
        scriptGameManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
