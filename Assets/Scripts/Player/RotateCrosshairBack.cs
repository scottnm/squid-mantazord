using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCrosshairBack : MonoBehaviour {

	// Update is called once per frame
	void Update ()
	{
		var aimStickState = InputWrapper.GetRightStick();
        if (aimStickState.stickPushed)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
