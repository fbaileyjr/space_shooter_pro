using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    // wave target `
    // set first_boss flag to true on spawn Manager `
    // instantiate orb
    // enable orb
    // orb brings in boss
    // phase 1:
    // instantiate orbs, green orbs are created (shooting script needs to be added to orbs)
    // move lower left
    // shoot projectiles x 3 seq
    // move back 
    // shoot projectiles x 3 seq
    // move lower right
    // shoot projectiles  x 3 seq
    // despawn orbs
    // phase 2:
    // move lower left/right (random selection)
    // charge beam
    // shoot and move (maybe make ship vibrate)
    // move all the way to the right
    // move to center
    // start phase 1 again
    // health bar = 50?

    // Start is called before the first frame update

    [SerializeField]
    private bool _startBossFight = true;

    [SerializeField]
    private bool _stopBossFight = false;

    [SerializeField]
    private bool _isPhaseOne = true;
    private bool _isPhaseTwo = false;

    [SerializeField]
    GameObject _EBPrefab;

    [SerializeField]
    GameObject _bossBoltPrefab;

    [SerializeField]
    GameObject _megaLaserChargePrefab;

    [SerializeField]
    private float _megaLaserChargeOffset = -3.5f;

    [SerializeField]
    GameObject _texturedMegaLaserPrefab;

    [SerializeField]
    private float _textureLaserOffset = 2.25f;

    [SerializeField]
    private float _bossMovementSpeed = 3.0f;

    private Vector3 _startPos = new Vector3(0f, 3.5f, 0f);

    Animator _bossAnimator;

    [SerializeField]
    private int _bossHealth = 10;

    private Boss1_Health _bossHealthBar;
    private Player _player;
    private float _bossDamage;
    private float _bossHealthThreshold;

    [SerializeField]
    private float _bossThresholdMultiplier = .40f;

    [SerializeField]
    private AudioSource _explosionAudioSource;

    private SpawnManager _spawnManager;

    Vector2 _prevPos;
    Vector2 _newPos;
    Vector2 _bossVelocity;

    void Start()
    {
        _bossAnimator = GetComponent<Animator>();
        _prevPos = transform.position;
        _newPos = transform.position;

        _bossHealthBar = this.transform.GetChild(0).transform.Find("Boss1_Health_Bar").GetComponent<Boss1_Health>();
        //GameObject _boss1_Health_Canvas = this.transform.GetChild(0).gameObject;
        //_bossHealth = ReturnDecendantOfParent(_boss1_Health_Canvas, "Boss1_Health_Bar" )
        _player = GameObject.Find("Player").GetComponent<Player>();
        _bossDamage = 1.0f/_bossHealth;
        _bossHealthThreshold = _bossHealth * _bossThresholdMultiplier;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("_spawnManger not found on UIManager");
        }

        if (_EBPrefab == null)
        {
            Debug.Log("_EB on Boss1 is null!");
        }

        if (_bossBoltPrefab == null)
        {
            Debug.Log("_bossBoltPrefab on Boss1 is null!");
        }

        if (_megaLaserChargePrefab == null)
        {
            Debug.Log("_megaLaserChargePrefab on Boss1 is null!");
        }

        if (_texturedMegaLaserPrefab == null)
        {
            Debug.Log("_texturedMegaLaserPrefab on Boss1 is null!");
        }

        if (_bossAnimator == null)
        {
            Debug.Log("_bossAnimator on Boss1 is null!");
        }

        if (_bossHealthBar == null)
        {
            Debug.LogWarning("_bossHealthBar is null on Boss1!");
        }

        if (_player == null)
        {
            Debug.LogError("_player on Enemy is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(_isPhaseOne && !_isPhaseTwo)
        {
            StartCoroutine(_phaseOne());
            _isPhaseOne = false;
            _startBossFight = false;
        }

        if (!_isPhaseOne && _isPhaseTwo)
        {
            StartCoroutine(_phaseTwo());
            _isPhaseTwo = false;
            _startBossFight = false;
        }
    }

    private void FixedUpdate()
    {
        _newPos = transform.position;  
        _bossVelocity = (_newPos - _prevPos) / Time.fixedDeltaTime;  // velocity = dist/time
        _prevPos = _newPos;  // update position for next frame calculation


        _bossAnimator.SetFloat("Boss1Idle", _bossVelocity.x) ;
    }

    IEnumerator _phaseOne ()
    {
        bool _moveLeft = _randomBool();
        Debug.Log("moveleft value is: " + _moveLeft);
        yield return new WaitForSeconds(2.0f);
        GameObject _EB = Instantiate(_EBPrefab, transform.position, Quaternion.identity);
        _EB.transform.parent = this.transform;
        yield return new WaitForSeconds(1.0f);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);
        _player.canShoot(false);
        yield return new WaitForSeconds(1.5f);
        Vector3 bLeft = new Vector3(-4.75f, 0f, 0f);
        Vector3 bRight = new Vector3(4.75f, 0f, 0f);
        if(_moveLeft)
        {
            while (this.transform.position != bLeft)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bLeft, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                _moveLeft = ! _moveLeft;
            }
        }
        else
        {
            while (this.transform.position != bRight)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bRight, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                _moveLeft = ! _moveLeft;
            }
        }
        yield return new WaitForSeconds(3.0f);
        while (this.transform.position != _startPos)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _bossMovementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3.0f);
        Debug.Log("moveleft value is: " + _moveLeft);
        if (_moveLeft)
        {
            while (this.transform.position != bLeft)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bLeft, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (this.transform.position != bRight)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bRight, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(3.0f);
        while (this.transform.position != _startPos)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _bossMovementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3.0f);
        //_enemyAnim.SetTrigger("OnEnemyDeath");
        Animator []animators = _EB.GetComponentsInChildren<Animator>();   
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("isExit");
        }
        yield return new WaitForSeconds(2.0f);
        Destroy(_EB);

        // set to true to start phase 2
        _isPhaseTwo = true;
    }

    IEnumerator _phaseTwo()
    {
        // instantiate megaball
        yield return new WaitForSeconds(0.50f);
        int _repCount = 2;
        bool _moveRight = _randomBool();
        Debug.Log("_moveRight value is: " + _moveRight);
        yield return new WaitForSeconds(2.0f);
        Vector3 toTheLeft = new Vector3(-7.5f, 2.5f, 0f);
        Vector3 toTheRight = new Vector3(7.5f, 2.5f, 0f);
            

        while (_repCount > 0)
        {
            GameObject _megaLaserCharge = Instantiate(_megaLaserChargePrefab, transform.position + new Vector3(0f, _megaLaserChargeOffset, 0), Quaternion.identity);
            _megaLaserCharge.transform.parent = this.transform;
            if (_moveRight)
            {
                while (this.transform.position != toTheLeft)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, toTheLeft, _bossMovementSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1.0f);
                _spawnMegaLaser(_megaLaserCharge);
                yield return new WaitForSeconds(1.0f);
                Destroy(_megaLaserCharge);
                GameObject _megaLaser = Instantiate(_texturedMegaLaserPrefab, transform.position + new Vector3(0.1f, _textureLaserOffset, 0), Quaternion.identity);
                _megaLaser.transform.parent = this.transform;
                yield return new WaitForSeconds(.25f);
                while (this.transform.position != toTheRight)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, toTheRight, _bossMovementSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1.5f);
                Destroy(_megaLaser);
            }
            else
            {
                while (this.transform.position != toTheRight)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, toTheRight, _bossMovementSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1.0f);
                _spawnMegaLaser(_megaLaserCharge);
                yield return new WaitForSeconds(1.0f);
                Destroy(_megaLaserCharge);
                GameObject _megaLaser = Instantiate(_texturedMegaLaserPrefab, transform.position + new Vector3(0.1f, _textureLaserOffset, 0), Quaternion.identity);
                _megaLaser.transform.parent = this.transform;
                yield return new WaitForSeconds(.25f);
                while (this.transform.position != toTheLeft)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, toTheLeft, _bossMovementSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(1.5f);
                Destroy(_megaLaser);
            }

            yield return new WaitForSeconds(1.5f);
            while (this.transform.position != _startPos)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3.0f);
            _repCount -= 1;
            _moveRight = !_moveRight;
        }
        
        
        _isPhaseOne = true;
    }

    private bool _randomBool()
    {
        bool _randomValue = (Random.Range(0, 2) == 0);
        return _randomValue;
    }

    private void _spawnMegaLaser(GameObject __megaLaserCharge)
    {
        //_enemyAnim.SetTrigger("OnEnemyDeath");
        Animator[] animators = __megaLaserCharge.GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            if(animator.name == "megaLaserBall")
            {
                animator.SetTrigger("isExitMegaLaserBall");
            }
        }
    }

    public void stopBossFight()
    {
        _stopBossFight = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        // use tags
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            //EnemyDeath();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            Debug.Log("Hit by laser!");
            _bossHealth -= 1;
            _bossHealthBar._decreaseHealthBar(_bossDamage);

            if (_bossHealth < _bossHealthThreshold)
            {
                _bossHealthBar._changeToRedFiller();
            }

            if (_bossHealth < 1)
            {
                _spawnManager.stopSpawning();
                _bossAnimator.SetTrigger("OnBossDeath");
                _bossMovementSpeed = 0f;
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                    
            }
        }

    }

}