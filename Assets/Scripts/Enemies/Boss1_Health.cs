using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1_Health : MonoBehaviour
{

    private Image _boss1HealthBar;

    // Start is called before the first frame update
    // Boss1_Health_Bar
    void Start()
    {
        _boss1HealthBar = GetComponent<Image>();
        if (_boss1HealthBar == null)
        {
            Debug.Log("_boss1HealthBar is null on Boss 1 Health Bar!");
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _decreaseHealthBar(float _decreaseAmount)
    {
        // decrease bar by 1/(boss/health)
        _boss1HealthBar.fillAmount -= _decreaseAmount;
        Debug.LogWarning("_decreaseAmount is: " + _decreaseAmount);
        Debug.LogWarning("_bossHealth bar is: " + _boss1HealthBar.fillAmount);
    }

    public void _changeToRedFiller()
    {
        // change color to red
        _boss1HealthBar.color = new Color(1, 0, 0, 1);
    }

}
