using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public GameManager scriptGameManager;
    public Transform target;
    public bool isMoveRight;
    public bool isActive;
    public List<Transform> listLaser;
    public SphereCollider collider;
    public MeshRenderer renderer;
    public ParticleSystem[] particleJet;
    [SerializeField] private ScreenWrap _scriptScreenWrap;
    [SerializeField] private Transform _barrel;
    [SerializeField] private GameObject _prefabLaser;
    [SerializeField] private ParticleSystem _particleExplosion;
    [SerializeField] private float _speedTurn;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _speedLaser;
    [SerializeField] private float _strafeFrequencyMin;
    [SerializeField] private float _strafeFrequencyMax;
    [SerializeField] private float _strafeDistanceMin;
    [SerializeField] private float _strafeDistanceMax;
    [SerializeField] private float _fireRateMin;
    [SerializeField] private float _fireRateMax;
    [SerializeField] private float _fireRateBurst;
    [SerializeField] private int _roundsPerBurst;
    [SerializeField] private bool _isStrafe;
    [SerializeField] private bool _isStrafeUp;

    private void Start()
    {
        StartCoroutine(StrafeVertically());
        StartCoroutine(FireLaser());
    }
    
    private void Update()
    {
        if (isActive)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _speedTurn);

            var step = _speedMove * Time.deltaTime;

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
                    transform.position =
                        Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, step);
                }
                else
                {
                    transform.position =
                        Vector3.MoveTowards(transform.position, transform.position + Vector3.back, step);
                }
            }
        }

        if (listLaser.Count > 0)
        {
            for (int i = 0; i < listLaser.Count; i++)
            {
                var velocity = _speedLaser * Time.deltaTime;
                listLaser[i].position = Vector3.MoveTowards(listLaser[i].position, listLaser[i].position + listLaser[i].up, velocity);
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
        StartCoroutine(StrafeVertically());
    }

    private IEnumerator FireLaser()
    {
        yield return new WaitForSeconds(Random.Range(_fireRateMin, _fireRateMax));

        if (isActive && !scriptGameManager.isGameOver)
        {
            for (int i = 0; i < _roundsPerBurst; i++)
            {
                if (!isActive) break;
                var newLaser = Instantiate(_prefabLaser, _barrel.position, _barrel.rotation);
                listLaser.Add(newLaser.transform);
                newLaser.GetComponent<Laser>().scriptDrone = this;
                yield return new WaitForSeconds(_fireRateBurst);
            }
        }

        StartCoroutine(FireLaser());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Laser")
        {

            if (collision.transform.tag == "Laser")
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
                collision.gameObject.GetComponent<Laser>().DestroyLaser();
            }

            Instantiate(_particleExplosion, transform.position, transform.rotation);
            scriptGameManager.RemoveEnemy(gameObject);
            scriptGameManager.PlaySoundDestroyDrone();
            collider.enabled = false;
            renderer.enabled = false;
            particleJet[0].Stop();
            particleJet[1].Stop();
            isActive = false;
            StartCoroutine(scriptGameManager.SpawnDrone());
        }
    }
}
