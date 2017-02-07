using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCrosshairs : MonoBehaviour
{
    Transform pivotTransform;
    Transform spriteTransform;

    private void Start()
    {
        pivotTransform = transform;
        spriteTransform = transform.Find("CrosshairSprite");
    }
 
    void Update ()
    {
        var aimStickState = InputWrapper.GetRightStick();
        if (aimStickState.stickPushed)
        {
            pivotTransform.rotation = Quaternion.Euler(0, 0, aimStickState.angle);
            spriteTransform.rotation = Quaternion.identity;
        }
    }
}
