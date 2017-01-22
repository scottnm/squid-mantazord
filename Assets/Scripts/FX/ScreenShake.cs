using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    float shakeTime;
    [SerializeField]
    float shakeSpeed;
    [SerializeField]
    float shakeIntensity;

    private void OnWaveStart()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake()
    {
        Events.ScreenShakeBegin();
        var cameraOrigin = transform.position;
        float time = 0;
        while (time < shakeTime)
        {
            float shakeOffset = shakeIntensity * Mathf.Sin(shakeSpeed * time);
            transform.position = cameraOrigin + Vector3.right * shakeOffset;
            yield return null;
            time += Time.deltaTime;
        }
        transform.position = cameraOrigin;
        Events.ScreenShakeEnd();
    }
}
