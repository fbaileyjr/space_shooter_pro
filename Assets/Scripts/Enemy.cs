using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _enemyID;

    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;

    private Animator _enemyAnim;

    private AudioSource _explosionAudioSource;

    private bool _isDestroyed = false;

    [SerializeField]
    private AudioClip _explosionClip;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private float _enemyLaserOffset = 1.15f;

    [SerializeField] private float _sideMovementSpeed = 1.0f;

    [SerializeField]
    private float _sideSpeed;

    private bool _canFireProjectile = true;


    private float _startPoint;
    private float _endPoint;
    private bool _moveRight;
    private bool _dodgeRight = false;
    private bool _dodgeLeft = false;

    private UIManager _uiManager;

    // declare _enemylaser
    // assign _enemylaser in inspector
    // create an IEnumerator to shoot lasers at random intervals between 3-7 seconds
    // start coroutine in Start()


    // Start is called before the first frame update
    void Start()
    {
        _explosionAudioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _sideSpeed = Random.Range(0.0f, 2.0f);
        _startPoint = Random.Range(-3.0f, 0.0f);
        _endPoint = Random.Range(0.0f, 3.0f);
        _moveRight = Random.Range(0, 2) == 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        if (_player == null)
        {
            Debug.Log("Enemy::Start - _player does not exist");
        }

        _enemyAnim = GetComponent<Animator>();

        if (_enemyAnim == null)
        {
            Debug.Log("_enemyAnim is null");
        }

        if (_explosionAudioSource == null)
        {
            Debug.LogError("_explosionaudiosource on enemy is null");
        }

        if (_explosionClip == null)
        {
            Debug.Log("_explosionClip audio on Enemy is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("_uiManager on Enemy is null");
        }


        if (_player != null)
        {
            switch (_enemyID)
            {
                case 0:
                    StartCoroutine(SpawnEnemyLaserRoutine());
                    break;
                case 1:
                    break;
                case 2:
                    StartCoroutine(SpawnEnemyLaserRoutine());
                    break;
                default:
                    Debug.Log("Default value");
                    break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyID == 0 || _enemyID == 2)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            if (!_isDestroyed)
            {
                sideMovement();
            }
        }

        // implemented this earlier, this fulfils phase_2_aggresive_enemy_type
        if (_enemyID == 1)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            if (!_isDestroyed)
            {
                sideToSide();
                checkIfLower();
            }
        }

        // implemented this earlier below, this fulfils phase_2_smart_enemy
        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 8, 0);
            _canFireProjectile = true;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        // use tags
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            EnemyDeath();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        _isDestroyed = true;
        _enemySpeed = 0;
        _enemyAnim.SetTrigger("OnEnemyDeath");
        Collider2D this_collider = gameObject.GetComponent<Collider2D>();
        _uiManager.updateEnemyDestroyed();
        EnemyExplosion();
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }

    void EnemyExplosion()
    {
        _explosionAudioSource.Play();
    }

    IEnumerator SpawnEnemyLaserRoutine()
    {
        while (_isDestroyed == false)
        {
            GameObject laserObject = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, _enemyLaserOffset, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }

    // phase_2_smart_enemy
    IEnumerator SpawnProjectileRoutine()
    {
        _canFireProjectile = false;
        while (_isDestroyed == false)
        {
            GameObject ProjectileObject = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, _enemyLaserOffset, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }

    private void checkIfLower()
    {
        if (_player != null)
        {
            if (transform.position.y < _player.transform.position.y && _canFireProjectile == true)
            {
                StartCoroutine(SpawnProjectileRoutine());
            }
        }

    }

    void sideMovement()
    {
        if (_dodgeLeft)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        else if (_dodgeRight)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }
        else
        {
            if (transform.position.x > _endPoint)
            {
                _moveRight = false;
            }
            else if (transform.position.x < _startPoint)
            {
                _moveRight = true;
            }

            if (_moveRight && transform.position.x < _endPoint)
            {
                transform.Translate(Vector3.right * _sideSpeed * Time.deltaTime);
            }

            if (!_moveRight && transform.position.x > _startPoint)
            {
                transform.Translate(Vector3.left * _sideSpeed * Time.deltaTime);
            }
        }
    }

    private void sideToSide()
    {
        if (_player != null)
        {
            if (transform.position.y > _player.transform.position.y)
            {
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            }
            if (transform.position.y < _player.transform.position.y)
            {
                transform.Translate(0, 0, 0);

                if (_player.transform.position.x > transform.position.x)
                {

                    transform.Translate(Vector3.right * Time.deltaTime * 7);
                }
                else if (_player.transform.position.x < transform.position.x)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * 7);
                }

            }
        }

    }

    public void shootEnemyLasers()
    {
        GameObject laserObject = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, _enemyLaserOffset, 0), Quaternion.identity);
    }

    public void dodgeRight()
    {
        StartCoroutine(dodgeRightRoutine());
    }

    public void dodgeLeft()
    {
        StartCoroutine(dodgeLeftRoutine());
    }

    IEnumerator dodgeRightRoutine()
    {
        if (_shouldDodge())
        {
            _dodgeRight = true;
            yield return new WaitForSeconds(1.5f);
            _dodgeRight = false;
        }
    }

    IEnumerator dodgeLeftRoutine()
    {
        if (_shouldDodge())
        {
            _dodgeLeft = true;
            yield return new WaitForSeconds(1.5f);
            _dodgeLeft = false;
        }
    }

    private bool _shouldDodge()
    {
        bool _dodge = (Random.Range(0, 2) == 0);
        return _dodge;
    }
}