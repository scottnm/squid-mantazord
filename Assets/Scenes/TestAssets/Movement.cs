using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 motionVector = InputWrapper.GetLeftStickVector();
        transform.Translate(motionVector * Time.deltaTime);
	}
}
