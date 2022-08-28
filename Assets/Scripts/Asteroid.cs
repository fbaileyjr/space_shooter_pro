using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _asteroidRotateSpeed = 21.0f;

    [SerializeField]
    private GameObject _asteroidExplosion;

    private SpawnManager _spawnManager;

    private UIManager _uIManager;

    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>(); 

        if(_uIManager == null)
        {
            Debug.Log("_uIManager is null on Asteroid.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // rotate object on zed axis, start with 3.0f
        transform.Rotate(Vector3.forward * _asteroidRotateSpeed * Time.deltaTime);
    }

    // check for laser collsions (trigger)
    // instantiate explosion at the position
    // destroy the explosion after 3 secondss
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Laser")
        {
            Instantiate(_asteroidExplosion, transform.position, Quaternion.identity);
            //_spawnManager.StartSpawning();
            _uIManager.startWaveText();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            

        }

    }
}
