using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;

    // handle to animator component

    private Animator _enemyAnim;

    private AudioSource _explosionAudioSource;

    [SerializeField]
    private AudioClip _explosionClip;


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

        void EnemyDeath()
        {
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Collider2D this_collider = gameObject.GetComponent<Collider2D>();
            EnemyExplosion();
            Destroy(this.gameObject, 2.8f);
        }

        void EnemyExplosion()
        {
            _explosionAudioSource.Play();
        }

    }
}
