using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject [] _enemyPrefab;

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
    private int _currentEnemyCount;
    private int _targetEnemeyCount;
    private float _powerupSpawnAdjuster = 0.0f;
    private int _totalEnemyCount = 0;

    private UIManager _uiManager;

    private Coroutine _spawnEnemy;
    private Coroutine _lowPowerups;
    private Coroutine _mediumPowerups;
    private Coroutine _highPowerups;

    // int for rate of powerups according to enemies spawned
    [SerializeField]
    private float _lowPowerupRate = 40.0f;

    [SerializeField]
    private float _mediumPowerupRate = 25.0f;

    [SerializeField]
    private float _highPowerupRate = 12.0f;

    // lists for different Powerup objects 
    [SerializeField]
    private PowerUp[] _lowRatePowerups;

    [SerializeField]
    private PowerUp[] _mediumRatePowerups;

    [SerializeField]
    private PowerUp[] _highRatePowerups;

    // lists for different Powerup objects IDs
    private List<int> _lowRateList = new List<int>();
    private List<int> _mediumRateList = new List<int>();
    private List<int> _highRateList = new List<int>();
    // Start is called before the first frame update


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _targetEnemeyCount = _uiManager.returnTargetWaveCount();

        if (_uiManager == null)
        {
            Debug.Log("_uiManger on Spawnmanager is null");
        }

        // _lowRatePowerups
        // _mediumRatePowerups
        if (_lowRatePowerups != null)
        {
            foreach (PowerUp x in _lowRatePowerups)
            {
                _lowRateList.Add(x.returnPowerupId());
            }

        }

        if (_mediumRatePowerups != null)
        {
            foreach (PowerUp x in _mediumRatePowerups)
            {
                _mediumRateList.Add(x.returnPowerupId());
            }

        }


        if (_highRatePowerups != null)
        {
            foreach (PowerUp x in _highRatePowerups)
            {
                _highRateList.Add(x.returnPowerupId());
            }

        }
        
        foreach (int y in _highRateList)
        {
            Debug.Log("High Rate List is" + y);
        }
        if(_highRateList.Contains(3))
        {
            Debug.Log("The list contains a 3!!!");
        }

    }
    public void StartSpawning()
    {
        _stopSpawning = false;
        _spawnEnemy = StartCoroutine(SpawnEnemyRoutine(_enemySpawnTime));
        SpawnPowerupRoutine();

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEnemyCount == _targetEnemeyCount)
        {
            _stopSpawning = true;
        }
    }

    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab[0], transform.position + new Vector3(randomX, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _currentEnemyCount += 1;
            _totalEnemyCount += 1;
            yield return new WaitForSeconds(waitTime);
        }

        // need to add some kind of int for rate
        // if(mylist.Contains(x) && enemy total = ?)

    }

    private void SpawnPowerupRoutine()
    {

        _lowPowerups = StartCoroutine(lowRateRoutine(_powerupSpawnAdjuster));
        _mediumPowerups = StartCoroutine(mediumRateRoutine(_powerupSpawnAdjuster));
        _highPowerups = StartCoroutine(highRateRoutine(_powerupSpawnAdjuster));
    }

    // need a way to stop the waves
    // display wave
    // start spawning again

    IEnumerator lowRateRoutine(float adjuster)
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range((_lowPowerupRate - 4.0f) - adjuster, _lowPowerupRate - adjuster));
            _spawnPowerupObject(_getPowerupID(_lowRateList), _randomX());
            yield return new WaitForSeconds(Random.Range((_lowPowerupRate - 4.0f) - adjuster, _lowPowerupRate - adjuster));
        }
    }

    IEnumerator mediumRateRoutine(float adjuster)
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range((_mediumPowerupRate - 4.0f) - adjuster, _mediumPowerupRate - adjuster));
            _spawnPowerupObject(_getPowerupID(_mediumRateList), _randomX());
            yield return new WaitForSeconds(Random.Range((_mediumPowerupRate - 4.0f) - adjuster, _mediumPowerupRate - adjuster));
        }
    }

    IEnumerator highRateRoutine(float adjuster)
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range((_highPowerupRate - 4.0f) - adjuster, _highPowerupRate - adjuster));
            _spawnPowerupObject(_getPowerupID(_highRateList), _randomX());
            yield return new WaitForSeconds(Random.Range((_highPowerupRate - 4.0f) - adjuster, _highPowerupRate - adjuster));
        }
    }

    private void _spawnPowerupObject(int _powerupID, float _randomX)
    {
        GameObject newPowerup = Instantiate(powerups[_powerupID], transform.position + new Vector3(_randomX, 8, 0), Quaternion.identity);
        newPowerup.transform.parent = _powerupContainer.transform;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private float _randomX()
    {
        return Random.Range(-8.0f, 8.0f);
    }

    private int _getPowerupID(List<int> rateList)
    {
        int randomPowerup = rateList[Random.Range(0, rateList.Count)];
        Debug.Log("random number is" + randomPowerup);


        return randomPowerup;
    }
    public void startNextWave()
    {
        StopCoroutine(_spawnEnemy);
        StopCoroutine(_lowPowerups);
        StopCoroutine(_mediumPowerups);
        StopCoroutine(_highPowerups);
        _waveCount += 1;
        _powerupSpawnAdjuster += 1.0f;
        _targetEnemeyCount = _uiManager.returnTargetWaveCount();
        if (_enemySpawnTime > 3.0f)
        {
            _enemySpawnTime -= 0.2f;
        }

    }

    public void stopSpawning()
    {
        _stopSpawning = false;
    }

    public int currentWaveCount()
    {
        return _waveCount;
    }

    public void updateWaveCount(int wave)
    {
        _waveCount = wave;
        _enemySpawnTime = 5.0f;
    }

}
