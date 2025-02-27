using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;

public class TestJoystick : BattleSceneTest
{
    private JoystickTester joystickTester;
    private RectTransform stick;

    protected override void OnSceneLoadingEnd()
    {
        joystickTester = new JoystickTester(GetJoystick());
        stick = joystickTester.GetStick();
    }

    [Test]
    public void TestControlJoystick()
    {
        joystickTester.ResetJoystickForTest();

        float movementRange = joystickTester.GetMovementRange();

        Vector2 anchoredDestination = Quaternion.Euler(0f, 0f, 45f) * new Vector2(movementRange / 2f, 0f);
        Vector2 dragDestination = stick.TransformPoint(anchoredDestination);

        joystickTester.ControlJoystick(dragDestination);

        Assert.AreEqual(true, Vector2.Distance(stick.anchoredPosition, anchoredDestination) < 0.1f);

        anchoredDestination *= 4f;
        dragDestination = stick.TransformPoint(anchoredDestination);

        joystickTester.ControlJoystick(dragDestination);

        Assert.AreEqual(true, Vector2.Angle(stick.anchoredPosition, anchoredDestination) < 1f);
        Assert.AreEqual(true, Mathf.Abs(Vector2.Distance(Vector2.zero, stick.anchoredPosition) - movementRange) < 0.1f);
    }

    [Test]
    public void TestResetJoystick()
    {
        joystickTester.ResetJoystickForTest();

        stick.anchoredPosition = stick.anchoredPosition + new Vector2(50f, 0f);

        joystickTester.ResetJoystick();

        Assert.AreEqual(Vector2.zero, stick.anchoredPosition);
    }

    [Test]
    public void TestGetDirection()
    {
        joystickTester.ResetJoystickForTest();

        stick.anchoredPosition = stick.anchoredPosition + new Vector2(50f, 0f);

        Assert.AreEqual(true, stick.anchoredPosition.normalized == joystickTester.joystick.GetDirection());
    }
}
