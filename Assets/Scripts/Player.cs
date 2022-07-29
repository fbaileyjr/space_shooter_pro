using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _laserOffset = 1.15f;

    [SerializeField]
    private float _fireRate = 0.150f;

    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private float powerupCooldown = 5.0f;

    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    private bool _isGameOver = false;

    [SerializeField]
    private GameObject _left_Engine, _right_Engine;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL.");
        }

        if (_right_Engine == null)
        {
            Debug.Log("Unable to find _right_Engine");
        }

        if (_left_Engine == null)
        {
            Debug.Log("Unable to find _left_Engine");
        }
    }

    void Update()
    {
        calculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    void calculateMovement()
    {
        // get movement
        float horiztonalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horiztonalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        }
        
        
    }

    public void Damage()
    {

        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            // isdisable the visualizer
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            // show engine failure depending on lives
            case 2:
                _right_Engine.SetActive(true);
                break;
            case 1:
                _left_Engine.SetActive(true);
                break;

        }

        if (_lives < 1) 
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _isGameOver = true;
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine(powerupCooldown));
        // start power down coroutine
    }

    IEnumerator TripleShotPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine(5.0f));
    }

    IEnumerator SpeedBoostPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isSpeedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

}
