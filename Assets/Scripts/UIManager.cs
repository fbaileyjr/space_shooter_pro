using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // handle to text
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoCountText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private TextMeshProUGUI _waveUI;

    private TextMeshPro _waveText;


    private GameManager _gameManager;

    private Player _player;

    private SpawnManager _spawnManager;

    private int _currentEnemyDestroyed = 0;
    private int _targetWaveCount = 10;

    [SerializeField]
    private int _targetFirstBossWaveCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "0";
        _ammoCountText.text = "15/15";
        _waveText = _waveUI.GetComponent<TextMeshPro>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_gameManager == null)
        {
            Debug.Log("GameManager doesn't exist");
        }

        if (_spawnManager == null)
        {
            Debug.Log("_spawnManger not found on UIManager");
        }
    }

    void Update()
    {
        if (_currentEnemyDestroyed == _targetWaveCount)
        {
            _spawnManager.startNextWave();
            _currentEnemyDestroyed = 0;
            _targetWaveCount += 5;
            StartCoroutine(waveCount());
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = playerScore.ToString();
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.text = ammoCount.ToString() + "/15";

    }
    // declare Ammo count text
    // function to update Ammo text


    public void UpdateLives(int currentLives)
    {
        if (currentLives > -1)
        {
            _LivesImg.sprite = _liveSprites[currentLives];
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _spawnManager.updateWaveCount(1);
        StartCoroutine(TextFlicker());
    }
    IEnumerator TextFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    IEnumerator waveCount()
    {
        int count = 0;

        // only for first boss because the animation is sooo long
        if (_spawnManager.currentWaveCount() == _targetFirstBossWaveCount)
        {
            _spawnManager.spawnFirstBoss();
        }

        while (count < 3)
        {
            _waveUI.SetText("WAVE " + _spawnManager.currentWaveCount());
            _waveUI.gameObject.SetActive(true);
            _waveUI.enabled = true;
            yield return new WaitForSeconds(1.5f);
            _waveUI.gameObject.SetActive(false);
            _waveUI.enabled = false;
            yield return new WaitForSeconds(.5f);
            count += 1;
        }

        // if wave count = targetbosscount
        // spawn boss
        // other wise regular count
        if ( _spawnManager.currentWaveCount() == _targetFirstBossWaveCount)
        {
            Debug.Log("Passing as boss wave");
            Debug.Log("Hooraw!");
        }
        else
        {
            _spawnManager.StartSpawning();
        }
    }

    public void startWaveText()
    {
        StartCoroutine(waveCount());
    }

    public void updateEnemyDestroyed()
    {
        _currentEnemyDestroyed += 1;
    }

    public int returnTargetWaveCount()
    {
        return _targetWaveCount;
    }
}
