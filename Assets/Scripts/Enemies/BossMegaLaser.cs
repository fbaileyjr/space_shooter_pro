using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMegaLaser : MonoBehaviour
{
    private bool _castDamage = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                _castDamage = true;
                StartCoroutine(startDamageOverTime(player));
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopAllCoroutines();
        _castDamage = false;
    }

    IEnumerator startDamageOverTime(Player player)
    {
        while(_castDamage)
        {
            Debug.Log("gonna hit with megalaser");
            if (player != null)
            {
                player.Damage();
            }
            yield return new WaitForSeconds(1.0f);
        }

    }

    
}
