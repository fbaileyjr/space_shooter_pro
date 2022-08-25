using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _enemySpawnTime = 5.0f;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject [] powerups;

    [SerializeField]
    private int _specialPowerupCount = 3;


    [SerializeField]
    private GameObject _powerupContainer;

    [SerializeField]
    private float _healthTimerMultiplier = 2.0f;

    private int _waveCount = 1;

    private UIManager _uiManager;

    // Start is called before the first frame update

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.Log("_uiManger on Spawnmanager is null");
        }
    }
    public void StartSpawning()
    {
        //_uiManager.startWaveText();
        StartCoroutine(SpawnEnemyRoutine(_enemySpawnTime));
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, transform.position + new Vector3(randomX, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            int randomPowerup = Random.Range(0, 6);
            if (randomPowerup == 5 && _specialPowerupCount > 0)
            {
                Debug.Log("Orb selected: " + _specialPowerupCount);
                _specialPowerupCount--;
                randomPowerup = Random.Range(0, 4);
            }
            else if (randomPowerup == 5 && _specialPowerupCount == 0)
            {
                _specialPowerupCount = 3;
            }
            GameObject newPowerup = Instantiate(powerups[randomPowerup], transform.position + new Vector3(randomX, 8, 0), Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

        }
    }

    // need a way to stop the waves
    // display wave
    // start spawning again


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void resumeSpawning()
    {
        _stopSpawning = false;
    }

    public int currentWaveCount()
    {
        return _waveCount;
    }
}
