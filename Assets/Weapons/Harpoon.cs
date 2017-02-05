﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField]
    private Sprite retractedArm;
    [SerializeField]
    private Sprite extendedArm;

    private float extendedArmDuration = .25f;
    private float extendedArmValue;
    private float cooldownDuration = .2f;
    private float cooldownValue;
    private HarpoonState harpoonState;

    void Start ()
    {
        Reset();
	}

    private void Reset()
    {
        harpoonState = HarpoonState.Rest;
        extendedArmValue = extendedArmDuration;
        cooldownValue = cooldownDuration;
    }

            {
                var forwardVector = transform.TransformVector(Vector2.up);
    private enum HarpoonState
    {
        Rest,
        Cooldown,
        Attack
    }
}