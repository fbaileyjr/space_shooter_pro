using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEB : MonoBehaviour
{
    private bool _stopShooting = false;

    [SerializeField]
    GameObject _bossProjectile;

    // Start is called before the first frame update
    void Start()
    {
        if (_bossProjectile == null)
        {
            Debug.Log("_bossProjectile is missing!");
        }

        if (_bossProjectile && !_stopShooting)
        {
            StartCoroutine(SpawnProjectile());
        }
        
    }

    IEnumerator SpawnProjectile()
    {
        while (_stopShooting == false)
        {
            GameObject _bProjectile = Instantiate(_bossProjectile, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }
}
