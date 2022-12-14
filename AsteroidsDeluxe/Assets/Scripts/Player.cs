using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> listLaser;
    public int lives;
    [SerializeField] private GameManager _scriptGameManager;
    [SerializeField] private InputCheck _scriptInputCheck;
    [SerializeField] private CameraShake _scriptCameraShake;
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
    [SerializeField] private bool _isReload;
    [SerializeField] private bool _isBounce;
    [SerializeField] private bool _isSpawn;
    [SerializeField] private ParticleSystem _particleShield;
    [SerializeField] private ParticleSystem _particleAura;
    [SerializeField] private ParticleSystem _particleExplosion;
    [SerializeField] private ParticleSystem _particleMuzzle;
    [SerializeField] private ParticleSystem[] _particleJet;
    [SerializeField] private ParticleSystem[] _particleSpawnAura;
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public void PlayerStart()
    {
        _isPlay = true;
        _renderer.enabled = true;
        _health = 100;
        lives = 2;
        StartCoroutine(PlaySpawnAura());
    }

    private void Update()
    {
        if (_isPlay)
        {
            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                _isThrust = true;
                _isBounce = false;
                _particleJet[0].Play();
                _particleJet[1].Play();
            }
            else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.UpArrow)))
            {
                _isThrust = false;
                _speedThrust = _speedThrustMin;
                _particleJet[0].Stop();
                _particleJet[1].Stop();
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

            if (((Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetMouseButtonDown(1))) && _shieldTime > 0f && _health > 0f)
            {
                _isShield = true;
                _particleShield.Play();
                _particleAura.Play();
                _scriptGameManager.PlaySoundShieldOn();
            }
            else if ((Input.GetKeyUp(KeyCode.LeftShift)) || (Input.GetMouseButtonUp(1)))
            {
                _isShield = false;
                _particleShield.Stop();
                _particleAura.Stop();
                _scriptGameManager.PlaySoundShieldOff();
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
            transform.tag = "Shield";
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
        else if (_isShield && _shieldTime <= 0)
        {
            _isShield = false;
            _particleShield.Stop();
            _particleAura.Stop();
            _isShieldDeactivating = true;
            StartCoroutine(DeactivateShield());
        }
    }
    
    private void FixedUpdate()
    {
        if (_isThrust && !_isBounce)
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

    private IEnumerator PlaySpawnAura()
    {
        _isSpawn = true;
        _particleSpawnAura[0].Play();
        _particleSpawnAura[1].Play();
        yield return new WaitForSeconds(2.0f);
        _particleSpawnAura[0].Stop();
        _particleSpawnAura[1].Stop();
        _isSpawn = false;
    }

    public IEnumerator BounceOffAsteroid()
    {
        _isBounce = true;
        yield return new WaitForSeconds(0.25f);
        _isBounce = false;
    }
    
    private IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(_shieldDeavtivateTime);
        _collider.radius = 0.5f;
        _isShieldDeactivating = false;
        _isShieldActive = false;
        transform.tag = "Player";
    }

    private IEnumerator ShootLaser()
    {
        if (_isShoot && !_isReload)
        {
            _isReload = true;
            var newLaser = Instantiate(_prefabLaser, _gunBarrel.position, _gunBarrel.rotation);
            newLaser.GetComponent<Laser>().scriptPlayer = this;
            listLaser.Add(newLaser.transform);
            _particleMuzzle.Play();
            yield return new WaitForSeconds(_fireRate);
            _isReload = false;
            StartCoroutine(ShootLaser());
        }
    }

    private IEnumerator DelayRespawn()
    {
        _health = 100;
        yield return new WaitForSeconds(1.25f);
        transform.position = Vector3.zero;
        _collider.enabled = true;
        _renderer.enabled = true;
        _isPlay = true;
        _scriptGameManager.SetLives(lives);
        _scriptGameManager.PlaySoundSpawnPlayer();
        _healthBar.rectTransform.sizeDelta = new Vector2(((float) _health * 3f), 50f);
        _particleJet[2].Play();
        _particleJet[3].Play();
        StartCoroutine(PlaySpawnAura());
        CheckInput();
    }

    private void CheckInput()
    {
        if (_scriptInputCheck.isThrust)
        {
            _isThrust = true;
            _particleJet[0].Play();
            _particleJet[1].Play();
            _isBounce = false;
        }
        
        if (_scriptInputCheck.isTurnLeft)
        {
            _isTurnLeft = true;
        }
        
        if (_scriptInputCheck.isTurnRight)
        {
            _isTurnRight = true;
        }
        
        if (_scriptInputCheck.isShoot)
        {
            _isShoot = true;
            StartCoroutine(ShootLaser());
        }
        
        if (_scriptInputCheck.isShield)
        {
            _isShield = true;
            _particleShield.Play();
            _particleAura.Play();
            _scriptGameManager.PlaySoundShieldOn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Laser")
        {
            collision.gameObject.GetComponent<Laser>().DestroyLaser();
            _scriptGameManager.PlaySoundDestroyDeathStarSmall();
        }

        if (_isShieldActive || _isSpawn)
        {
            StartCoroutine(_scriptCameraShake.Shake(0.1f, 0.1f));
            return;
        }
        
        switch (collision.transform.tag)
        {
            case "Small":
                _health -= 15;
                StartCoroutine(_scriptCameraShake.Shake(0.15f, 0.4f));
                break;
            case "Medium":
                _health -= 25;
                StartCoroutine(_scriptCameraShake.Shake(0.15f, 0.3f));
                break;
            case "Large":
                _health -= 95;
                StartCoroutine(_scriptCameraShake.Shake(0.35f, 0.8f));
                break;
            case "Laser":
                _health -= 20;
                StartCoroutine(_scriptCameraShake.Shake(0.25f, 0.4f));
                break;
            case "Enemy":
                _health -= 30;
                StartCoroutine(_scriptCameraShake.Shake(0.25f, 0.4f));
                break;
        }
        
        _healthBar.rectTransform.sizeDelta = new Vector2(((float) _health * 3f), 50f);

        if (_health <= 0)
        {
            lives -= 1;
            _isPlay = false;
            _isThrust = false;
            _isTurnLeft = false;
            _isTurnRight = false;
            _isShoot = false;
            _isShield = false;
            _collider.enabled = false;
            _renderer.enabled = false;
            Instantiate(_particleExplosion, transform.position, transform.rotation);

            for (int i = 0; i < _particleJet.Length; i++)
            {
                _particleJet[i].Stop();
            }
            
            if (lives <= -1)
            {
                _scriptGameManager.GameOver();
            }
            else
            {
                StartCoroutine(DelayRespawn());
            }
        }
    }
}
