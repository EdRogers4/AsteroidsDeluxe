using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameManager scriptGameManager;
    public Transform target;
    public bool isMoveRight;
    [SerializeField] private ScreenWrap _scriptScreenWrap;
    [SerializeField] private ParticleSystem _particleExplosion;
    [SerializeField] private float _speedTurn;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _strafeFrequencyMin;
    [SerializeField] private float _strafeFrequencyMax;
    [SerializeField] private float _strafeDistanceMin;
    [SerializeField] private float _strafeDistanceMax;
    [SerializeField] private bool _isStrafe;
    [SerializeField] private bool _isStrafeUp;

    private void Start()
    {
        StartCoroutine(StrafeVertically());
    }
    
    private void Update()
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation , Time.deltaTime * _speedTurn);
        
        var step =  _speedMove * Time.deltaTime;
        
        if (isMoveRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, step);
        }

        if (_isStrafe)
        {
            if (_isStrafeUp)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, step);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.back, step);
            }
        }
    }

    private IEnumerator StrafeVertically()
    {
        yield return new WaitForSeconds(Random.Range(_strafeFrequencyMin, _strafeFrequencyMax));
        _isStrafe = true;
        var directionIndex = Random.Range(0, 1);
        _isStrafeUp = directionIndex != 0;
        yield return new WaitForSeconds(Random.Range(_strafeDistanceMin, _strafeDistanceMax));
        _isStrafe = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Shield" || collision.transform.tag == "Screen") return;
        
        if (collision.transform.tag == "Laser")
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Laser>().DestroyLaser();
        }
        
        Instantiate(_particleExplosion, transform.position, transform.rotation);
        scriptGameManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _scriptScreenWrap.isEnter = true;
    }
}
