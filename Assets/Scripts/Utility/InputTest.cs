using UnityEngine;

public class InputTest : MonoBehaviour
{
    [System.Serializable]
    class InputSet
    {
        public Vector2 LeftStick;
        public InputWrapper.RightStickState RightStick;
        public bool Submit;
        public bool Cancel;
    }

    [SerializeField]
    private InputSet inputs;

	// Update is called once per frame
    void Update ()
    {
        inputs.LeftStick = InputWrapper.GetLeftStick();
        inputs.RightStick = InputWrapper.GetRightStick();
        inputs.Submit = InputWrapper.SubmitPressed();
        inputs.Cancel = InputWrapper.CancelPressed();
    }
}
