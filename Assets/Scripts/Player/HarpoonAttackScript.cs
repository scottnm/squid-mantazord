using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAttackScript : MonoBehaviour
{
    GameObject harpoon = null;
    
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
        harpoon = transform.GetChild(0).gameObject;
        Reset();
    }

    private void Reset()
    {
        harpoonState = HarpoonState.Rest;
        extendedArmValue = extendedArmDuration;
        cooldownValue = cooldownDuration;
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
            var stickState = InputWrapper.GetRightStick();

            if (stickState.stickPushed) 
            {
                //activate specific harpoon
                var forwardVector = harpoon.transform.TransformVector(Vector2.up);
                if (!Physics2D.Raycast(transform.position, forwardVector, 1.5f, LayerMask.GetMask("Wall")))
                {
                    harpoon.GetComponent<SpriteRenderer>().sprite = extendedArm;
                    harpoonState = HarpoonState.Attack;
                }
            }
        }

        else if (harpoonState == HarpoonState.Attack)
        {
            var forwardVector = harpoon.transform.TransformVector(Vector2.up);
            RaycastHit2D hit = Physics2D.Raycast(harpoon.transform.position, forwardVector, 2f, LayerMask.GetMask("Enemy"));

            if (hit.collider != null)
            {
                GameObject hitGO = hit.collider.gameObject;
                hitGO.GetComponent<EnemyDieScript>().Die();
            }

            extendedArmValue -= Time.deltaTime;

            if (extendedArmValue <= 0)
            {
                harpoon.GetComponent<SpriteRenderer>().sprite = retractedArm;
                harpoonState = HarpoonState.Cooldown;
                extendedArmValue = extendedArmDuration;
            }
        }
    }
}
