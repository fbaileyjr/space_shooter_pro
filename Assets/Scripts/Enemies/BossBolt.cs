using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBolt : MonoBehaviour
{
    [SerializeField]
    private float _bossProjectileSpeed = 8.0f;

    void Start()
    {
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _bossProjectileSpeed * Time.deltaTime);

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
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
