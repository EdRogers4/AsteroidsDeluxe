using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStar : MonoBehaviour
{
    public int startingAxis;
    public GameManager scriptGameManager;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private GameObject _prefabDeathStarMedium;
    [SerializeField] private GameObject _prefabDeathStarSmall;
    [SerializeField] private int _size;
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;
    [SerializeField] private float _speed;
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
        var step =  _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, step);
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
            switch (_size)
            {
                case 0:
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    break;
                case 1:
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    break;
                case 2:
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    break;
            }

            collision.gameObject.GetComponent<Laser>().DestroyLaser();
        }

        if (_size == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                var newEnemy = Instantiate(_prefabDeathStarMedium, _spawnPoint[i].position, _spawnPoint[i].rotation);
                scriptGameManager.listEnemies.Add(newEnemy);
                var scriptDeathStar = newEnemy.GetComponent<DeathStar>();
                scriptDeathStar.scriptGameManager = scriptGameManager;
            }
            
            scriptGameManager.UpdateScore(20);
        }
        else if (_size == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                var newEnemy = Instantiate(_prefabDeathStarSmall, _spawnPoint[i].position, _spawnPoint[i].rotation);
                scriptGameManager.listEnemies.Add(newEnemy);
                var scriptDeathStar = newEnemy.GetComponent<DeathStar>();
                scriptDeathStar.scriptGameManager = scriptGameManager;
            }
            
            scriptGameManager.UpdateScore(50);
        }
        else
        {
            scriptGameManager.UpdateScore(100);
        }

        //Instantiate(_particleSmoke, transform.position, transform.rotation);
        scriptGameManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
