using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAttackScript : MonoBehaviour
{
    GameObject anchorObject = null;
    GameObject activeHarpoon = null;
    GameObject[] harpoonMap;
    
    [SerializeField]
    private Sprite retractedArm;
    [SerializeField]
    private Sprite extendedArm;

    enum HarpoonState
    {
        Rest,
        Cooldown,
        Attack
    }

    private HarpoonState harpoonState;
    private float extendedArmDuration = .25f;
    private float extendedArmValue;
    private float cooldownDuration = .2f;
    private float cooldownValue;

    void Start()
    {
        anchorObject = transform.GetChild(0).gameObject;
        UnityEngine.Assertions.Assert.IsTrue(anchorObject.name == "Anchor");

        harpoonMap = new GameObject[(int)InputWrapper.StickState.NumStickStates];
        harpoonMap[(int)InputWrapper.StickState.Down] = anchorObject.transform.Find("Harpoon_BC").gameObject;
        harpoonMap[(int)InputWrapper.StickState.Up] = anchorObject.transform.Find("Harpoon_UC").gameObject;
        harpoonMap[(int)InputWrapper.StickState.Left] = anchorObject.transform.Find("Harpoon_ML").gameObject;
        harpoonMap[(int)InputWrapper.StickState.Right] = anchorObject.transform.Find("Harpoon_MR").gameObject;
        harpoonMap[(int)InputWrapper.StickState.RightUp] = anchorObject.transform.Find("Harpoon_UR").gameObject;
        harpoonMap[(int)InputWrapper.StickState.LeftUp] = anchorObject.transform.Find("Harpoon_UL").gameObject;
        harpoonMap[(int)InputWrapper.StickState.RightDown] = anchorObject.transform.Find("Harpoon_BR").gameObject;
        harpoonMap[(int)InputWrapper.StickState.LeftDown] = anchorObject.transform.Find("Harpoon_BL").gameObject;

        Reset();
    }

    private void Reset()
    {
        harpoonState = HarpoonState.Rest;
        extendedArmValue = extendedArmDuration;
        cooldownValue = cooldownDuration;
        activeHarpoon = anchorObject.transform.GetChild(0).gameObject;
        UnityEngine.Assertions.Assert.IsTrue(activeHarpoon.name.StartsWith("Harpoon"));
    }

    void Update()
    {
        if (harpoonState == HarpoonState.Cooldown)
        {
            cooldownValue -= Time.deltaTime;

            if (cooldownValue <= 0)
            {
                harpoonState = HarpoonState.Rest;
                cooldownValue = cooldownDuration;
            }
        }

        else if (harpoonState == HarpoonState.Rest)
        {
            var stickState = InputWrapper.GetRightStick ();

            if (stickState != InputWrapper.StickState.Center)
            {
                activeHarpoon = harpoonMap[(int)stickState];

                //activate specific harpoon
                var forwardVector = activeHarpoon.transform.TransformVector(Vector2.up);
                if (!Physics2D.Raycast(transform.position, forwardVector, 1.5f, LayerMask.GetMask("Wall")))
                {
                    activeHarpoon.GetComponent<SpriteRenderer>().sprite = extendedArm;
                    harpoonState = HarpoonState.Attack;
                }
            }
        }

        else if (harpoonState == HarpoonState.Attack)
        {
            var forwardVector = activeHarpoon.transform.TransformVector(Vector2.up);
            RaycastHit2D hit = Physics2D.Raycast(anchorObject.transform.position, forwardVector, 2f, LayerMask.GetMask("Enemy"));

            if (hit.collider != null)
            {
                GameObject hitGO = hit.collider.gameObject;
                hitGO.GetComponent<EnemyDieScript>().Die();
            }

            extendedArmValue -= Time.deltaTime;

            if (extendedArmValue <= 0)
            {
                activeHarpoon.GetComponent<SpriteRenderer>().sprite = retractedArm;
                harpoonState = HarpoonState.Cooldown;
                extendedArmValue = extendedArmDuration;
            }
        }
    }
}
