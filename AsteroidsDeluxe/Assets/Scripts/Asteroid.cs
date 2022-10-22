using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int startingAxis;
    public GameManager scriptGameManager;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private GameObject _prefabAsteroidMedium;
    [SerializeField] private GameObject _prefabAsteroidSmall;
    [SerializeField] private int _size;
    [SerializeField] private float _screenTop;
    [SerializeField] private float _screenBottom;
    [SerializeField] private float _screenLeft;
    [SerializeField] private float _screenRight;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _startingPosition;
    [SerializeField] private Vector3 _startingTarget;

    private void Start()
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
    
    private void FixedUpdate()
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
            gameObject.GetComponent<SphereCollider>().enabled = false;
            collision.gameObject.GetComponent<Laser>().DestroyLaser();
        }

        if (_size == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                var newAsteroid = Instantiate(_prefabAsteroidMedium, _spawnPoint[i].position, transform.rotation);
                scriptGameManager.listEnemies.Add(newAsteroid);
                var scriptAsteroid = newAsteroid.GetComponent<Asteroid>();
                scriptAsteroid.scriptGameManager = scriptGameManager;
            }
            
            scriptGameManager.UpdateScore(20);
        }
        else if (_size == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                var newAsteroid = Instantiate(_prefabAsteroidSmall, _spawnPoint[i].position, transform.rotation);
                scriptGameManager.listEnemies.Add(newAsteroid);
                var scriptAsteroid = newAsteroid.GetComponent<Asteroid>();
                scriptAsteroid.scriptGameManager = scriptGameManager;
            }
            
            scriptGameManager.UpdateScore(50);
        }
        else
        {
            scriptGameManager.UpdateScore(100);
        }

        scriptGameManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
