using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private int _asteroidsToSpawn;
    [SerializeField] private GameObject _prefabAsteroidLarge;

    private void Start()
    {
        for (int i = 0; i < _asteroidsToSpawn; i++)
        {
            var newAsteroid = Instantiate(_prefabAsteroidLarge, transform.position, transform.rotation);
            newAsteroid.GetComponent<Asteroid>().startingAxis = i;
        }
    }
}
