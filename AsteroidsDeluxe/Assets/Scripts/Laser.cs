using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Player scriptPlayer;
    
    private void Start()
    {
        StartCoroutine(DestroyLaser());
    }

    private IEnumerator DestroyLaser()
    {
        yield return new WaitForSeconds(1.0f);
        scriptPlayer.listLaser.Remove(transform);
        Destroy(gameObject);
    }
}
