using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;

    void Start()
    {
        // on startup, grab the camera's current position
        originalPos = this.transform.position;
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);

            elapsedTime += Time.deltaTime;

            // wait one frame
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
