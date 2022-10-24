using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStar : MonoBehaviour
{
    public int startingAxis;
    public GameManager scriptGameManager;
    public Transform target;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private GameObject _prefabDeathStarMedium;
    [SerializeField] private GameObject _prefabDeathStarSmall;
    [SerializeField] private int _size;
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedTurn;
    [SerializeField] private bool _isHit;
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _startingTarget;
    [SerializeField] private ParticleSystem _particleExplosionLarge;
    [SerializeField] private ParticleSystem _particleExplosionMedium;
    [SerializeField] private ParticleSystem _particleExplosionSmall;
    
    void Start()
    {
        _startingTarget = new Vector3(Random.Range(_screenLeft, _screenRight), 0f, Random.Range(_screenTop, _screenBottom));

        if (_size == 0)
        {
            switch (startingAxis)
            {
                case 0:
                    _startingPosition = new Vector3(Random.Range(_screenLeft, -1f), 0f, _screenTop);
                    break;
                case 1:
                    _startingPosition = new Vector3(Random.Range(1f, _screenRight), 0f, _screenTop);
                    break;
                case 2:
                    _startingPosition = new Vector3(_screenRight, 0f, Random.Range(_screenTop, _screenBottom));
                    break;
                case 3:
                    _startingPosition = new Vector3(Random.Range(1f, _screenRight), 0f, _screenBottom);
                    break;
                case 4:
                    _startingPosition = new Vector3(Random.Range(_screenLeft, -1f), 0f, _screenBottom);
                    break;
                case 5:
                    _startingPosition = new Vector3(_screenLeft, 0f, Random.Range(_screenTop, _screenBottom));
                    break;
            }

            transform.position = _startingPosition;
        }
        
        transform.LookAt(_startingTarget);
    }
    
    private void Update()
    {
        var step =  _speedMove * Time.deltaTime;
        
        if (_size == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, step);  
        }
        else
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _speedTurn);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, step);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Shield")
        {
            StartCoroutine(collision.gameObject.GetComponent<Player>().BounceOffAsteroid());
        }
        
        if (collision.transform.tag != "Laser" && collision.transform.tag != "Player") return;
        
        if (collision.transform.tag == "Laser")
        {
            collision.gameObject.GetComponent<Laser>().DestroyLaser();
        }

        if (!_isHit)
        {
            _isHit = true;
            
            if (_size == 0)
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
                Instantiate(_particleExplosionLarge, transform.position, transform.rotation);
                scriptGameManager.PlaySoundDestroyDeathStarLarge();

                for (int i = 0; i < 3; i++)
                {
                    var newEnemy = Instantiate(_prefabDeathStarMedium, _spawnPoint[i].position, _spawnPoint[i].rotation);
                    scriptGameManager.listEnemies.Add(newEnemy);
                    scriptGameManager.listDeathStar.Add(newEnemy);
                    var scriptDeathStar = newEnemy.GetComponent<DeathStar>();
                    scriptDeathStar.scriptGameManager = scriptGameManager;
                    scriptDeathStar.target = target;
                }

                scriptGameManager.UpdateScore(20);
            }
            else if (_size == 1)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                //Instantiate(_particleExplosionMedium, transform.position, transform.rotation);
                scriptGameManager.PlaySoundDestroyDeathStarMedium();

                for (int i = 0; i < 2; i++)
                {
                    var newEnemy = Instantiate(_prefabDeathStarSmall, _spawnPoint[i].position, _spawnPoint[i].rotation);
                    scriptGameManager.listEnemies.Add(newEnemy);
                    scriptGameManager.listDeathStar.Add(newEnemy);
                    var scriptDeathStar = newEnemy.GetComponent<DeathStar>();
                    scriptDeathStar.scriptGameManager = scriptGameManager;
                    scriptDeathStar.target = target;
                }

                scriptGameManager.UpdateScore(50);
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                Instantiate(_particleExplosionSmall, transform.position, transform.rotation);
                scriptGameManager.PlaySoundDestroyDeathStarSmall();
                scriptGameManager.UpdateScore(100);
            }

            scriptGameManager.RemoveEnemy(gameObject);
            scriptGameManager.RemoveDeathStar(gameObject);
            Destroy(gameObject);
        }
    }
}
