using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryFire : MonoBehaviour
{
    [SerializeField]
    private float _orbSpeed = 8.0f;

    private void Start()
    {

    }

    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime * _orbSpeed;
        transform.Translate(Vector3.up * _orbSpeed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }


    // need fuction
    // to shoot orbs
    //instantiate when space is pressed
    // transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
}
