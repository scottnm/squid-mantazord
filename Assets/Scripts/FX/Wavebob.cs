using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavebob : MonoBehaviour
{
    [SerializeField]
    float bobScale;
    RectTransform rxform;
    Vector3 origin;

    private void Start()
    {
        rxform = GetComponent<RectTransform>();
        origin = rxform.position;
    }

	void Update ()
    {
        float t = Time.time;
        var y = 3 * Mathf.Sin(2 * t);// + Mathf.Sin(4.1f * t + 1);
        var x = 3 * Mathf.Sin(t);// + Mathf.Sin(2.1f * t + 1);
        var r = 2 * Mathf.Sin(t);/// 30;

        rxform.position = origin + Vector3.up * y * bobScale;
        rxform.position = rxform.position + Vector3.right * x * bobScale;
        rxform.eulerAngles = Vector3.forward * r;
    }
}
