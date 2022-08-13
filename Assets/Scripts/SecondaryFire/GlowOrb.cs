using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowOrb : MonoBehaviour
{
    [SerializeField]
    private float _orbRotateSpeed = 82.0f;

    [SerializeField]
    private GameObject _orbPrefab;

    private Animator[] _orbAnim;

    private Orb[] _glowOrbScript;

    void Start()
    {
        _orbAnim = GetComponentsInChildren<Animator>();

        if (_orbAnim == null)
        {
            Debug.Log("_enemyAnim is null");
        }

        _glowOrbScript = GetComponentsInChildren<Orb>();

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
        destroyOrbs();


    }

    public void shootOrbsInEachGlowOrb()
    {
        foreach (Orb _glowOrbs in _glowOrbScript)
            _glowOrbs.shootOrb();
    }

    public void destroyOrbs()
    {
        foreach (Animator anim in _orbAnim)
            anim.SetTrigger("isDestroyed");
        Destroy(this.gameObject, 1.0f);

    }

    // public variable
    // if shot
    // tell all of child components to shoot
}
