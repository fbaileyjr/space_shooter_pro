using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField]
    private GameObject _orbPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (_orbPrefab == null)
        {
            Debug.Log("_orbPrefab missing from Orb");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shootOrb()
    {
        GameObject _childOrb = Instantiate(_orbPrefab, transform.position, Quaternion.identity);
    }
}
