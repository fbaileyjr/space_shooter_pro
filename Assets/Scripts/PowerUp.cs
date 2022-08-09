using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _powerupSpeed = 3.0f;

    // ID for Powerups
    // 0 = Triple Shot
    // 1 = Speed
    // 2 = Shields
    [SerializeField]
    private int powerupID;

    [SerializeField]
    private AudioClip _clip;


    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }

        if (_clip == null)
        {
            Debug.LogError("_clip on powerup is set to null");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position, 1.0f);

            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.refillAmmo();
                        break;
                    case 4:
                        player.addLife();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
            }

            Destroy(this.gameObject);
        }

    }

}
