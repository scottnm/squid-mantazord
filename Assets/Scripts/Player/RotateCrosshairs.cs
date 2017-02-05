using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCrosshairs : MonoBehaviour
{
	void Update ()
    {
        var aimStickState = InputWrapper.GetRightStick();
        if (aimStickState.stickPushed)
        {
            transform.rotation = Quaternion.Euler(0, 0, aimStickState.angle);
        }
	}
}
