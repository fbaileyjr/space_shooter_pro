using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    // wave target `
    // set first_boss flag to true on spawn Manager `
    // instantiate orb
    // enable orb
    // orb brings in boss
    // phase 1:
    // instantiate orbs, green orbs are created (shooting script needs to be added to orbs)
    // move lower left
    // shoot projectiles x 3 seq
    // move back 
    // shoot projectiles x 3 seq
    // move lower right
    // shoot projectiles  x 3 seq
    // despawn orbs
    // phase 2:
    // move lower left/right (random selection)
    // charge beam
    // shoot and move (maybe make ship vibrate)
    // move all the way to the right
    // move to center
    // start phase 1 again
    // health bar = 50?

    // Start is called before the first frame update

    [SerializeField]
    private bool _isPhaseOne = true;

    [SerializeField]
    GameObject _EBPrefab;

    [SerializeField]
    GameObject _bossBoltPrefab;

    [SerializeField]
    GameObject _megaLaserChargePrefab;

    [SerializeField]
    GameObject _texturedMegaLaserPrefab;

    void Start()
    {

        if (_EBPrefab == null)
        {
            Debug.Log("_EB on Boss1 is null!");
        }

        if (_bossBoltPrefab == null)
        {
            Debug.Log("_bossBoltPrefab on Boss1 is null!");
        }

        if (_megaLaserChargePrefab == null)
        {
            Debug.Log("_megaLaserChargePrefab on Boss1 is null!");
        }

        if (_texturedMegaLaserPrefab == null)
        {
            Debug.Log("_texturedMegaLaserPrefab on Boss1 is null!");
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator _phaseOne ()
    {
        yield return new WaitForSeconds(0.50f);
    }

    IEnumerator _phaseTwo()
    {
        yield return new WaitForSeconds(0.50f);
    }



}
