using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEB : MonoBehaviour
{
    private bool _stopShooting = false;
    private Transform _target;

    [SerializeField]
    GameObject _bossProjectile;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").GetComponent<Transform>();
        if (_target == null)
        {
            Debug.Log("_target on BossBolt is null!");
        }

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
        yield return new WaitForSeconds(2.0f);
        while (_stopShooting == false)
        {
            Vector3 _diff = (_target.position - this.transform.position);
            _diff.Normalize();
            float _lookRotation = Mathf.Atan2(_diff.y, _diff.x) * Mathf.Rad2Deg;
            GameObject _bProjectile = Instantiate(_bossProjectile, transform.position, Quaternion.Euler(0f, 0f, (_lookRotation + 90)));
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
            if(_target == null)
            {
                _stopShooting = true;
            }
        }

    }
}
