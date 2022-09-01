using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int enemyID;

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


    private float _startPoint;
    private float _endPoint;
    private bool _moveRight;

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
            Debug.LogError("_explosionAudioSource on Enemy is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("_uiManager on Enemy is null");
        }

        // maybe switch for id is easier
        // switch
        StartCoroutine(SpawnEnemyLaserRoutine());


    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if(!_isDestroyed)
        {
            sideMovement();
        }


        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 8,0);
        }

        // 
        // if id is 1 and transform.position.y < transform.position.y of player
        // star coroutinte for rocket

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
            float randomX = Random.Range(-8.0f, 8.0f);
            GameObject newPowerup = Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, _enemyLaserOffset, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }

    void sideMovement()
    {

        if(transform.position.x > _endPoint)
        {
            _moveRight = false;
        }
        else if(transform.position.x < _startPoint)
        {
            _moveRight = true;
        }

        if(_moveRight && transform.position.x < _endPoint)
        {
            transform.Translate(Vector3.right * _sideSpeed * Time.deltaTime);
        }

        if(!_moveRight && transform.position.x > _startPoint)
        {
            transform.Translate(Vector3.left * _sideSpeed * Time.deltaTime);
        }
    }


}
