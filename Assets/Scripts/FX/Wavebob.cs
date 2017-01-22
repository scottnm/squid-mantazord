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
        float x = Time.time;
        var y = Mathf.Sin(2 * x) + Mathf.Sin(4.1f * x + 1);
        rxform.position = origin + Vector3.up * y * bobScale;
    }
}
