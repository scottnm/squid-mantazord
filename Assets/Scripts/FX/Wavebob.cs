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
        var y = 3 * Mathf.Sin(2 * t);
        var x = 3 * Mathf.Sin(t);
        var rot = 2 * Mathf.Sin(t);

        rxform.position = origin + Vector3.up * y * bobScale;
        rxform.position = rxform.position + Vector3.right * x * bobScale;
        rxform.eulerAngles = Vector3.forward * rot;
    }
}
