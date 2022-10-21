using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Player scriptPlayer;
    
    private void Start()
    {
        StartCoroutine(DelayDestroyLaser());
    }

    private IEnumerator DelayDestroyLaser()
    {
        yield return new WaitForSeconds(1.0f);
        DestroyLaser();
    }

    public void DestroyLaser()
    {
        scriptPlayer.listLaser.Remove(transform);
        Destroy(gameObject);
    }
}
