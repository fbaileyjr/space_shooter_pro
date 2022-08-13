using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    // declare _enemylaser
    // assign _enemylaser in inspector
    // create an IEnumerator to shoot lasers at random intervals between 3-7 seconds
    // start coroutine in Start()


    // Start is called before the first frame update
    void Start()
    {
        _explosionAudioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

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

        StartCoroutine(SpawnEnemyLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 8,0);
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
        _enemyAnim.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;
        Collider2D this_collider = gameObject.GetComponent<Collider2D>();
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
            // newPowerup.transform.parent = this.gameObject.transform;
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }

    }

    // if orb
    // destroy this object
    // maybe create a new label for orb

}
