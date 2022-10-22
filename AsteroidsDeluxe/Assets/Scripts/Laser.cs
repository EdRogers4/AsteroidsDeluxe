using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Player scriptPlayer;
    public Drone scriptDrone;
    [SerializeField] private bool _isEnemy;
    
    private void Start()
    {
        StartCoroutine(DelayDestroyLaser());
    }

    private IEnumerator DelayDestroyLaser()
    {
        if (!_isEnemy)
        {
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
        }

        DestroyLaser();
    }

    public void DestroyLaser()
    {
        if (!_isEnemy)
        {
            scriptPlayer.listLaser.Remove(transform);
        }
        else
        {
            scriptDrone.listLaser.Remove(transform);
        }

        Destroy(gameObject);
    }
}
