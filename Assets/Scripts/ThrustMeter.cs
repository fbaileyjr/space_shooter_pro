using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustMeter : MonoBehaviour
{
    [SerializeField]
    private float _reduceRefreshMultiplier = 1.25f;

    [SerializeField]
    private float _increaseRefreshMultiplier = 0.75f;

    private bool _isKeyPressed = false;

    private Image _thrustMeterBar;

    private float waitTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _thrustMeterBar = GetComponent<Image>();
    }


    void Update()
    {
        if (_isKeyPressed)
        {
            _thrustMeterBar.fillAmount -= _reduceRefreshMultiplier / waitTime * Time.deltaTime;
        }
        else
        {
            _thrustMeterBar.fillAmount += _increaseRefreshMultiplier / waitTime * Time.deltaTime;
        }

    }

    public void IsKeyPressed(bool answer)
    {
        _isKeyPressed = answer;
    }

    public float returnFillAmount()
    {
        return _thrustMeterBar.fillAmount;
    }

    public float returnIncreaseMultiplier()
    {
        return _increaseRefreshMultiplier;
    }
}
