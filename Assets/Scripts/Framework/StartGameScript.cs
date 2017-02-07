using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{

    public GameObject mainText;
    public GameObject shadowText;

    // Update is called once per frame
    void Update ()
    {
        if (InputWrapper.GetLeftStick().magnitude > Mathf.Epsilon)
        {
            mainText.SetActive(false);
            shadowText.SetActive(false);
            Events.StartGame();
            gameObject.SetActive(false);
        }
    }
}
