using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOrb : MonoBehaviour
{
    [SerializeField]
    private float _orbRotateSpeed = 82.0f;
    private Animator[] _orbAnim;

    void Start()
    {
        _orbAnim = GetComponentsInChildren<Animator>();

        if (_orbAnim == null)
        {
            Debug.Log("_enemyAnim is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(rotateOrbs());
    }

    IEnumerator rotateOrbs()
    {
        transform.Rotate(Vector3.forward * _orbRotateSpeed * Time.deltaTime);
        yield return new WaitForSeconds(.50f);
        _orbRotateSpeed = 41.0f;
        transform.Rotate(Vector3.forward * _orbRotateSpeed * Time.deltaTime);
        yield return new WaitForSeconds(5.0f);
        foreach(Animator anim in _orbAnim)
            anim.SetTrigger("isDestroyed");
            Debug.Log("settrigger for " + 1);
        // access public void player to set is active to false
        Destroy(this.gameObject, 1.0f);


    }

    // public variable
    // if shot
    // tell all of child components to shoot
}
