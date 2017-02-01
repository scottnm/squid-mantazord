using UnityEngine;

public class InputWrapper
{
    public static Vector2 GetLeftStick()
    {
        return new Vector2(Input.GetAxis(LeftStickX), Input.GetAxis(LeftStickY)).normalized;
    }

    [System.Serializable]
    public struct RightStickState
    {
        public RightStickState(float _angle, bool _stickPushed)
        {
            angle = _angle;
            stickPushed = _stickPushed;
        }
        public float angle;
        public bool stickPushed;
    }

    public static RightStickState GetRightStick()
    {
        var stickX = Input.GetAxis(RightStickX);
        var stickY = Input.GetAxis(RightStickY);
        if (Mathf.Abs(stickX) < Mathf.Epsilon && Mathf.Abs(stickY) < Mathf.Epsilon)
        {
            return new RightStickState( 0.0f, false );
        }

        Vector2 normXY= new Vector2(stickX, stickY).normalized;
        float angle = Mathf.Acos(normXY.x) * 180 / Mathf.PI;
        if (normXY.y < -Mathf.Epsilon)
        {
            angle = 360 - angle;
        }

        return new RightStickState(angle, true);
    }

    public static bool SubmitPressed()
    {
        return Input.GetAxis(Submit) > Mathf.Epsilon;
    }

    public static bool CancelPressed()
    {
        return Input.GetAxis(Cancel) > Mathf.Epsilon;
    }

    private static readonly string LeftStickX = "LeftStickX";
    private static readonly string LeftStickY = "LeftStickY";
    private static readonly string RightStickX =
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.OSXEditor ? "MacRightStickX" : "WinRightStickX";
    private static readonly string RightStickY =
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.OSXEditor ? "MacRightStickY" : "WinRightStickY";
    private static readonly string Submit =
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.OSXEditor ? "MacSubmit" : "WinSubmit";
    private static readonly string Cancel = "Cancel";
}
