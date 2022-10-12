using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPowerup : MonoBehaviour
{
    Transform getParent;
    Enemy parentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        getParent = this.transform.parent;
        if (getParent == null)
        {
            Debug.Log("Parent Transform is null on DestroyPowerup.cs script!");
        }
        else
        {
            parentEnemy = getParent.GetComponent <Enemy>();
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Powerup")
        {
            parentEnemy.shootEnemyLasers();
        }
    }
}
