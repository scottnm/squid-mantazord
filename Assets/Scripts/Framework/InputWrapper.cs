using UnityEngine;

public class InputWrapper
{
    public enum StickState
    {
        Right = 0,
        RightUp,
        Up,
        LeftUp,
        Left,
        LeftDown,
        Down,
        RightDown,
        Center,
        NumStickStates
    }

    public static StickState GetLeftStick()
    {
        return GetStickState(Input.GetAxis(LeftStickX), Input.GetAxis(LeftStickY));
    }

    public static Vector2 GetLeftStickVector()
    {
        return new Vector2(Input.GetAxis(LeftStickX), Input.GetAxis(LeftStickY)).normalized;
    }

    public static StickState GetRightStick()
    {
        return GetStickState(Input.GetAxis(RightStickX), Input.GetAxis(RightStickY));
    }

    private static StickState GetStickState(float stickX, float stickY)
    {
        if (Mathf.Abs(stickX) < Mathf.Epsilon && Mathf.Abs(stickY) < Mathf.Epsilon)
        {
            return StickState.Center;
        }

        float angle = Mathf.Acos(stickX) * 180 / Mathf.PI;

        if (stickY < -Mathf.Epsilon)
        {
            angle = -angle;
        }

        if (angle > -22.5 && angle <= 22.5)
        {
            // 0
            return StickState.Right;
        }

        else if (angle > 22.5 && angle <= 67.5)
        {
            // 45
            return StickState.RightUp;
        }
        else if ((angle > 67.5 && angle <= 90)
                || (angle > 90 && angle <= 112.5))
        {
            // 90
            return StickState.Up;
        }
        else if (angle > 112.5 && angle <= 157.5)
        {
            // 135
            return StickState.LeftUp;
        }
        else if ((angle > 157.5 && angle <= 180.0) ||
            (angle >= -180 && angle <= -157.5))
        {
            // 180
            return StickState.Left;
        }
        else if (angle > -67.5 && angle <= -22.5)
        {
            // -45
            return StickState.RightDown;
        }
        else if ((angle > -112.5 && angle <= -90)
                 || (angle > -90 && angle <= -67.5))
        {
            // -90
            return StickState.Down;
        }
        else
        {
            // -135
            UnityEngine.Assertions.Assert.IsTrue(angle > -157.5 && angle <= -112.5);
            return StickState.LeftDown;
        }
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