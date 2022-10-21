using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> listLaser;
    [SerializeField] private GameManager _scriptGameManager;
    [SerializeField] private Transform _gunBarrel;
    [SerializeField] private GameObject _prefabLaser;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private int _health;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _shieldBar;
    [SerializeField] private float _shieldTime;
    [SerializeField] private float _shieldDeavtivateTime;
    [SerializeField] private float _speedThrust;
    [SerializeField] private float _speedThrustMin;
    [SerializeField] private float _speedThrustMax;
    [SerializeField] private float _speedThrustMod;
    [SerializeField] private float _speedTurn;
    [SerializeField] private float _speedLaser;
    [SerializeField] private float _fireRate;
    [SerializeField] private bool _isPlay;
    [SerializeField] private bool _isThrust;
    [SerializeField] private bool _isTurnLeft;
    [SerializeField] private bool _isTurnRight;
    [SerializeField] private bool _isShoot;
    [SerializeField] private bool _isShield;
    [SerializeField] private bool _isShieldActive;
    [SerializeField] private bool _isShieldDeactivating;
    [SerializeField] private ParticleSystem _particleShield;
    [SerializeField] private ParticleSystem _particleAura;
    private Rigidbody _rigidBody;
    private bool _isReload;
    
    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void PlayerStart()
    {
        _isPlay = true;
        _renderer.enabled = true;
        _health = 100;
    }

    private void Update()
    {
        if (_isPlay)
        {
            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetMouseButtonDown(1)))
            {
                _isThrust = true;
            }
            else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.UpArrow)) || (Input.GetMouseButtonUp(1)))
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

            if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetMouseButtonDown(0))) && _health > 0f)
            {
                if (!_isShoot)
                {
                    _isShoot = true;
                    StartCoroutine(ShootLaser());
                }
            }
            else if ((Input.GetKeyUp(KeyCode.Space)) || (Input.GetMouseButtonUp(0)))
            {
                _isShoot = false;
            }

            if ((Input.GetKeyDown(KeyCode.LeftShift)) && _shieldTime > 0f && _health > 0f)
            {
                _isShield = true;
                _particleShield.Play();
                _particleAura.Play();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _isShield = false;
                _particleShield.Stop();
                _particleAura.Stop();
            }
        }

        if (_isTurnLeft && !_isTurnRight)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -_speedTurn);
        }
        else if (_isTurnRight && !_isTurnLeft)
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * _speedTurn);
        }

        if (_isShield && !_isShieldActive)
        {
            _isShieldActive = true;
            _collider.radius = 1.0f;
        }
        else if (!_isShield && _isShieldActive && !_isShieldDeactivating)
        {
            _isShieldDeactivating = true;
            StartCoroutine(DeactivateShield());
        }

        if (_isShield && _shieldTime > 0f)
        {
            _shieldTime -= Time.deltaTime;
            _shieldBar.rectTransform.sizeDelta = new Vector2((_shieldTime * 25f), 25f);
        }
    }

    private IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(_shieldDeavtivateTime);
        _collider.radius = 0.5f;
        _isShieldDeactivating = false;
        _isShieldActive = false;
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
        if (_isShieldActive) return;
        
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

        _healthBar.rectTransform.sizeDelta = new Vector2(((float) _health * 3f), 50f);

        if (_health < 0)
        {
            _health = 0;
            _isPlay = false;
            _rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            _collider.enabled = false;
            _renderer.enabled = false;
            _scriptGameManager.GameOver();
        }
    }
}
