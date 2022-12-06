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
    private bool _startBossFight = true;

    [SerializeField]
    private bool _stopBossFight = false;

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

    [SerializeField]
    private float _bossMovementSpeed = 3.0f;

    private Vector3 _startPos = new Vector3(0f, 3.5f, 0f);

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
        if(_isPhaseOne && _startBossFight)
        {
            StartCoroutine(_phaseOne());
            _startBossFight = false;
        }
        
    }

    IEnumerator _phaseOne ()
    {
        bool _moveLeft = _randomBool();
        Debug.Log("moveleft value is: " + _moveLeft);
        yield return new WaitForSeconds(2.0f);
        GameObject _EB = Instantiate(_EBPrefab, transform.position, Quaternion.identity);
        _EB.transform.parent = this.transform;
        // make eb
        // instantiate orbs, green orbs are created (shooting script needs to be added to orbs)
        yield return new WaitForSeconds(3.0f);
        Vector3 bLeft = new Vector3(-4.75f, 0f, 0f);
        Vector3 bRight = new Vector3(4.75f, 0f, 0f);
        if(_moveLeft)
        {
            while (this.transform.position != bLeft)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bLeft, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                _moveLeft = ! _moveLeft;
            }
        }
        else
        {
            while (this.transform.position != bRight)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bRight, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
                _moveLeft = ! _moveLeft;
            }
        }
        yield return new WaitForSeconds(3.0f);
        while (this.transform.position != _startPos)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _bossMovementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3.0f);
        Debug.Log("moveleft value is: " + _moveLeft);
        if (_moveLeft)
        {
            while (this.transform.position != bLeft)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bLeft, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (this.transform.position != bRight)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, bRight, _bossMovementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(3.0f);
        while (this.transform.position != _startPos)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _startPos, _bossMovementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3.0f);

        // randomize moving between true or false (left or right)
        // move lower left
        // shoot projectiles x 3 seq
        // move back 
        // shoot projectiles x 3 seq
        // move lower right
        // shoot projectiles  x 3 seq
        // despawn orbs
    }

    IEnumerator _phaseTwo()
    {
        yield return new WaitForSeconds(0.50f);
    }

    private bool _randomBool()
    {
        bool _randomValue = (Random.Range(0, 2) == 0);
        return _randomValue;
    }

    public void stopBossFight()
    {
        _stopBossFight = true;
    }

}