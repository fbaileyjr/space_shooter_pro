using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    [SerializeField]
    private int _shieldHealth = 3;

    private SpriteRenderer _shield;

    private List<Color> _shieldColors = new List<Color>()
    {
        new Color(.68f,.39f,.27f,0.25f),
        new Color(.50f,.87f,.33f,0.50f),
        new Color(.70f,.70f,.83f,0.75f), 
        new Color(1,1,1,1)
    };


    private bool _startOver = false;

    private Coroutine _lastRoutine = null;
    // get the renderer
    // shieldHealth = 3
    // declare a list of colors
    // decrease opacity
    // disappear
    void Start()
    {
        _shield = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_startOver)
        {

            _shieldHealth = 3;
            _shield.color = _shieldColors[_shieldHealth];
            _startOver = false;
        }
    }

    public bool shieldDamage()
    {
        _shieldHealth -= 1;

        if (_shieldHealth > 0)
        {
            _shield.color = _shieldColors[_shieldHealth];
            return (true);

        }
        else if (_shieldHealth == 0)
        {
            _lastRoutine = StartCoroutine(blinkShield());

            return (true);
        }
        else
        {
            return (false);
        }
    }

    public void newShield()
    {
        _startOver = true;
        if (_lastRoutine != null)
        {
            StopCoroutine(_lastRoutine);
        }
        
    }

    IEnumerator blinkShield()
    {
        while (true)
        {
            _shield.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(0.25f);
            _shield.color = _shieldColors[_shieldHealth];
            yield return new WaitForSeconds(0.5f);
        }
    }


}
