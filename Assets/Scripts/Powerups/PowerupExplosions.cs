using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupExplosions : MonoBehaviour
{
    private ParticleSystem partSystem;

    void Start()
    {
        partSystem = gameObject.GetComponent<ParticleSystem>();

        if (partSystem == null)
        {
            Debug.Log("ParticleCommandBuffScript.Start(): Command buff particle system is null");
        }
        else
        {
            SwitchOnParticles();
        }
        
        Destroy(this.gameObject, 3.5f);
    }

    public void SwitchOnParticles()
    {
        partSystem.Play();
        partSystem.enableEmission = true;   
    }
}
