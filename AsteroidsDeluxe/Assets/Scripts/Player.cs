using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> listLaser;
    [SerializeField] private Transform _gunBarrel;
    [SerializeField] private GameObject _prefabLaser;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private int _health;
    [SerializeField] private Image _healthbar;
    [SerializeField] private float _speedThrust;
    [SerializeField] private float _speedThrustMin;
    [SerializeField] private float _speedThrustMax;
    [SerializeField] private float _speedThrustMod;
    [SerializeField] private float _speedTurn;
    [SerializeField] private float _speedLaser;
    [SerializeField] private float _fireRate;
    [SerializeField] private bool _isThrust;
    [SerializeField] private bool _isTurnLeft;
    [SerializeField] private bool _isTurnRight;
    [SerializeField] private bool _isShoot;
    [SerializeField] private bool _isShield;
    private Rigidbody _rigidBody;
    private bool _isReload;
    
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
            if (!_isShoot)
            {
                _isShoot = true;
                StartCoroutine(ShootLaser());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _isShoot = false;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isShield = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isShield = false;
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
        if (_isThrust)
        {
            _rigidBody.velocity = transform.forward * _speedThrust;

            if (_speedThrust < _speedThrustMax)
            {
                _speedThrust += (_speedThrust * _speedThrustMod);
            }
        }

        if (listLaser.Count > 0)
        {
            for (int i = 0; i < listLaser.Count; i++)
            {
                var step =  _speedLaser * Time.deltaTime;
                listLaser[i].position = Vector3.MoveTowards(listLaser[i].position + listLaser[i].up, listLaser[i].position, step);
            }
        }
    }

    private IEnumerator ShootLaser()
    {
        if (_isShoot && !_isReload)
        {
            _isReload = true;
            var newLaser = Instantiate(_prefabLaser, _gunBarrel.position, _gunBarrel.rotation);
            newLaser.GetComponent<Laser>().scriptPlayer = this;
            listLaser.Add(newLaser.transform);
            yield return new WaitForSeconds(_fireRate);
            _isReload = false;
            StartCoroutine(ShootLaser());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Small":
                _health -= 15;
                break;
            case "Medium":
                _health -= 25;
                break;
            case "Large":
                _health -= 95;
                break;
        }

        _healthbar.rectTransform.sizeDelta = new Vector2(((float) _health * 3f), 50f);
        
        if (_health < 0)
        {
            _health = 0;
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }
}
