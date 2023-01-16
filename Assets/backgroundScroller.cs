using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundScroller : MonoBehaviour
{
    [SerializeField]
    private float _verticalScrollSpeed = 4.0f;

    [SerializeField]
    private float _maxScrollDistance = -43.008f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVerticalMovement();
    }

    void CalculateVerticalMovement()
    {
        transform.Translate(Vector3.down * _verticalScrollSpeed * Time.deltaTime);

        if (transform.position.y <= _maxScrollDistance)
        {
            transform.position = new Vector3(0, Mathf.Abs(_maxScrollDistance), 0);
        }
    }
}
