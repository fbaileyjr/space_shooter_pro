using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    [SerializeField]
    private float _defaultProjectspeed = 4.0f;
    private Transform _target;
    private Rigidbody2D _rigidBody;

    [SerializeField]
    private float _angleChangingSpeed = 1.0f;
    [SerializeField]
    private float _movementSpeed = 8.0f;

    private bool _delayMovement = true;



    void Start()
    {
        _target = GameObject.Find("Player").GetComponent<Transform>();
        _rigidBody = this.GetComponent<Rigidbody2D>();


        if(_target == null)
        {
            Debug.Log("_target is null on enemyProjectiles");
        }

        StartCoroutine(_delayRocketMovement());
    }


    private void FixedUpdate()
    {
        if (!_delayMovement)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
            _rigidBody.velocity = transform.up * _movementSpeed;

            if (transform.position.y > 8.0f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }

        if (_delayMovement)
        {
            transform.Translate(Vector3.up * _defaultProjectspeed * Time.deltaTime);
        }

    }


    IEnumerator _delayRocketMovement()
    {
        yield return new WaitForSeconds(0.15f);
        _delayMovement = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

    }
}

