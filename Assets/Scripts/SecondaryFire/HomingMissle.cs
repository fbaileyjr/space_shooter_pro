using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
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

    [SerializeField]
    private GameObject _explosion;



    void Start()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            Debug.Log("There are no enemies for homing missle.");
        }

        if (closestEnemy != null)
        {
            _target = closestEnemy.GetComponent<Transform>();
        }
        if (_target == null)
        {
            Debug.Log("_target transform is null");
        }
        _rigidBody = this.GetComponent<Rigidbody2D>();


        if (_target == null)
        {
            Debug.Log("_target is null on Homing Missle");
        }

        StartCoroutine(_delayRocketMovement());
        StartCoroutine(_startDestroyTimer());
    }


    private void FixedUpdate()
    {
        if (_target != null)
        {
            if (!_delayMovement)
            {
                Vector2 direction = (Vector2)_target.position - _rigidBody.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                _rigidBody.angularVelocity = -_angleChangingSpeed * rotateAmount;
                _rigidBody.velocity = transform.up * _movementSpeed;

                if (transform.position.y > 10.0f)
                {
                    if (transform.parent != null)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                    _destroyProjectile();
                }
            }

            if (_delayMovement)
            {
                transform.Translate(Vector3.up * _defaultProjectspeed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
            if (transform.position.y > 10.0f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                _destroyProjectile();
            }
        }


    }


    IEnumerator _delayRocketMovement()
    {
        yield return new WaitForSeconds(0.15f);
        _delayMovement = false;
    }

    private void _destroyProjectile()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {

            _destroyProjectile();
        }

    }
    IEnumerator _startDestroyTimer()
    {
        yield return new WaitForSeconds(3.0f);
        _destroyProjectile();
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        if(gos.Length == 0)
            gos = GameObject.FindGameObjectsWithTag("Boss");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if (closest)
        {
            return closest;
        }
        else
        {
            return null;
        }
    }
}
