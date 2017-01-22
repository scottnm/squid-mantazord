using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float speed;

	void FixedUpdate ()
    {
        Vector3 motionVector = InputWrapper.GetLeftStickVector();
        transform.Translate(motionVector * Time.deltaTime * speed);
	}
}
