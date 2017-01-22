using UnityEngine;

public class InputTest : MonoBehaviour
{
    [System.Serializable]
    class InputSet
    {
        public InputWrapper.StickState LeftStick;
        public InputWrapper.StickState RightStick;
        public bool Submit;
        public bool Cancel;
    }

    [SerializeField]
    private InputSet inputs;
    [SerializeField]
    float leftx;
    [SerializeField]
    float lefty;

	// Update is called once per frame
    void Update ()
    {
        inputs.LeftStick = InputWrapper.GetLeftStick();
        inputs.RightStick = InputWrapper.GetRightStick();
        inputs.Submit = InputWrapper.SubmitPressed();
        inputs.Cancel = InputWrapper.CancelPressed();

        leftx = Input.GetAxis("LeftStickX");
        lefty = Input.GetAxis("LeftStickY");
    }
}
