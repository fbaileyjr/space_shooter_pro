using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterAnimation : MonoBehaviour
{
    [SerializeField]
    private float delay = 0f;

    [SerializeField]
    GameObject _targetObject;

    [SerializeField]
    private float _activeDelay = 0f;
    // Use this for initialization
    void Start()
    {
        if(_targetObject == null)
        {
            Debug.Log("_targetObject on DestroyObjectAfterAnimation is missing");
        }

        StartCoroutine(_setActiveTarget(_targetObject));
        Destroy(gameObject,delay);
    }

    IEnumerator _setActiveTarget(GameObject _target)
    {
        yield return new WaitForSeconds(_activeDelay);
        GameObject bossGameObject = Instantiate(_target, new Vector3(0, 3.5f, 0), Quaternion.identity);
    }
}
