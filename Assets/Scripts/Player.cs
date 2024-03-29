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
    private GameObject _glowOrbPrefab;

    [SerializeField]
    private GameObject _OutOfAmmoParticle;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _laserOffset = 1.15f;

    [SerializeField]
    private float _particleExplosionOffset = 2.0f;

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
    private bool _isOrbActive = false;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    private bool _isGameOver = false;

    [SerializeField]
    private GameObject _left_Engine, _right_Engine;

    [SerializeField]
    private AudioClip _laserSoundClip;


    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private float _thrustMultiplier = 1.5f;

    private bool isThrusterActive = false;

    private ShieldHealth _shieldHealth;

    private bool _outOfAmmo = false;

    private bool _refillAmmo = false;

    private int _ammoCount = 15;

    private GameObject _glowOrb;

    private GlowOrb _gOrbScript;

    private bool _isHomingMissle = false;

    private ThrustMeter _thrustMeter;

    private CameraShake _cameraShake;

    private bool _refillMeter = false;

    private Animator _playerAnimator;

    [SerializeField]
    private GameObject _homingMissilePrefab;


    [SerializeField]
    private float _shakeDuration = .1f;

    [SerializeField]
    private float _shakeMag = .25f;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldHealth = _shield.GetComponent<ShieldHealth>();
        _thrustMeter = GameObject.Find("Thruster_Meter_Bars").GetComponent<ThrustMeter>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _playerAnimator = GetComponent<Animator>();

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

        if (_audioSource == null)
        {
            Debug.LogError("Audisource on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_shieldHealth == null)
        {
            Debug.LogError("ShieldHealth on player is null");
        }

        if (_cameraShake == null)
        {
            Debug.Log("CameraShake on player is null");
        }

        if (_homingMissilePrefab == null)
        {
            Debug.Log("_homingMissilePrefab is null");
        }

        if (_playerAnimator == null)
        {
            Debug.Log("_playerAnimator is null");
        }
    }

    void Update()
    {
        calculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (_refillAmmo)
        {
            _ammoCount = 15;
            _uiManager.UpdateAmmoCount(_ammoCount);
            _refillAmmo = _outOfAmmo = false; 
        }


    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        _playerAnimator.SetFloat("playerIdle", h);
    }



void calculateMovement()
    {
        // get movement
        float horiztonalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horiztonalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        // calculating thrustmeter
        if (_thrustMeter.returnFillAmount() >= 0)
        {
            thrusterBoost();
        }

        if (_thrustMeter.returnFillAmount() == 0)
        {
            _refillMeter = true;
            thrusterBoost();
            StartCoroutine(RefillThruster());
        }

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

        if (_outOfAmmo == false)
        {
            if (_isTripleShotActive)
            {
                if (_isOrbActive)
                {
                    _isOrbActive = false;
                    _gOrbScript.destroyOrbs();
                }


                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                playLaserClip();
            }
            else if (_isOrbActive)
            {
                _gOrbScript.shootOrbsInEachGlowOrb();

            }
            else if (_isHomingMissle)
            {
                Instantiate(_homingMissilePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                playLaserClip();
            }
            updateAmmo();

        }

        if (_outOfAmmo)
        {
            Instantiate(_OutOfAmmoParticle, transform.position + new Vector3(0, _particleExplosionOffset, 0), Quaternion.identity);
        }

    }

    public void Damage()
    {

        if (_isShieldActive)
        {
            _isShieldActive = _shieldHealth.shieldDamage();
            _shield.SetActive(_isShieldActive);

            // isdisable the visualizer
            // void function to assign _isShieldActive bool
            return;
        }

        StartCoroutine(_cameraShake.ShakeCamera(_shakeDuration, _shakeMag));
        _lives--;
        _uiManager.UpdateLives(_lives);
        _updateEngines();


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

    IEnumerator OrbPowerupDownRoutine()
    {
        yield return new WaitForSeconds(6.0f);
        _isOrbActive = false;
    }

    IEnumerator RefillThruster()
    {
        _speed /= _thrustMultiplier;
        _thrustMeter.IsKeyPressed(false);
        yield return new WaitForSeconds(5.0f / _thrustMeter.returnIncreaseMultiplier());
        _refillMeter = false;
    }

    IEnumerator slowPlayerRoutine()
    {
        _speed *= .75f;
        yield return new WaitForSeconds(5.0f);
        _speed /= .75f;
    }

    IEnumerator HomingMissleRoutine()
    {
        _isHomingMissle = true;
        yield return new WaitForSeconds(5.0f);
        _isHomingMissle = false;
    }

    public void ShieldActive()
    {

        _isShieldActive = true;
        _shieldHealth.newShield();
        _shield.SetActive(true);

    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

    void playLaserClip() // method to play an audio clip
    {
        _audioSource.PlayOneShot(_laserSoundClip);
    }

    void thrusterBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _refillMeter == false)
        {
            _speed *= _thrustMultiplier;
            _thrustMeter.IsKeyPressed(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _refillMeter == false)
        {
            _speed /= _thrustMultiplier;
            _thrustMeter.IsKeyPressed(false);
        }
    }

    void updateAmmo()
    {
        if (_ammoCount > 0)
        {
            _ammoCount -= 1;
            _uiManager.UpdateAmmoCount(_ammoCount);
        }
        else
        {
            _outOfAmmo = true;
        }
    }

    private void _updateEngines()
    {
        switch (_lives)
        {
            // show engine failure depending on lives
            case 3:
                _right_Engine.SetActive(false);
                _left_Engine.SetActive(false);
                break;
            case 2:
                _right_Engine.SetActive(true);
                _left_Engine.SetActive(false);
                break;
            case 1:
                _left_Engine.SetActive(true);
                break;
        }
    }

    // ------------------ 
    // demarcation for public voids

    public void refillAmmo()
    {
        _ammoCount = 15;
        _refillAmmo = true;
    }

    public void addLife()
    {
        if (_lives < 3)
        {
            _lives += 1;
        }
        _uiManager.UpdateLives(_lives);
        _updateEngines();
    }

    public void isOrbWeaponActive()
    {
        _isOrbActive = true;
        // Instantiate(_glowOrbPrefab, transform.position, Quaternion.identity);
        _glowOrb = Instantiate(_glowOrbPrefab, transform.position, Quaternion.identity);
        _glowOrb.transform.parent = this.transform;
        _gOrbScript = _glowOrb.GetComponent<GlowOrb>();
        StartCoroutine(OrbPowerupDownRoutine());
    }

    public void slowPlayerMovement()
    {
        StartCoroutine(slowPlayerRoutine());
    }

    public void homingMissle()
    {
        StartCoroutine(HomingMissleRoutine());
    }

    public void canShoot(bool shootBool)
    {
        _outOfAmmo = shootBool;
    }

}
