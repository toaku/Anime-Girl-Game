using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JoystickTester
{
    public Joystick joystick
    {
        get;
        private set;
    }

    public JoystickTester(Joystick joystick)
    {
        this.joystick = joystick;
    }

    public RectTransform GetStick()
    {
        return (RectTransform)PrivateMemberAccessor.GetField(joystick, "stick");
    }

    public float GetMovementRange()
    {
        return float.Parse(PrivateMemberAccessor.GetField(joystick, "movementRange").ToString());
    }

    public void ControlJoystick(Vector2 draggedPosition)
    {
        PrivateMemberAccessor.InvokeMethod(joystick, "ControlJoystick", draggedPosition);
    }

    public void ResetJoystick()
    {
        PrivateMemberAccessor.InvokeMethod(joystick, "ResetJoystick");
    }

    public void ResetJoystickForTest()
    {
        GetStick().anchoredPosition = Vector2.zero;
    }
}
