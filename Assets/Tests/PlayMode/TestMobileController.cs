using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestMobileController : BattleSceneTest
{
    private MobileControllerTester mobileControllerTester;
    private TouchButton attackButton;

    protected override void OnSceneLoadingEnd()
    {
        mobileControllerTester = new MobileControllerTester(GetMobileController());
        attackButton = mobileControllerTester.GetAttackButton();
    }

    [Test]
    public void TestGetAttackInput()
    {
        mobileControllerTester.ResetMobileController();

        PrivateMemberAccessor.SetFieldByPropertySetter(attackButton, "isInput", true, false);

        Assert.AreEqual(true, mobileControllerTester.mobileController.GetAttackInput());
    }

    [Test]
    public void TestGetMovementInput()
    {
        mobileControllerTester.ResetMobileController();

        mobileControllerTester.joystickTester.ControlJoystick(new Vector2(50f, 0f));
        Assert.AreEqual(mobileControllerTester.joystickTester.joystick.GetDirection(), mobileControllerTester.mobileController.GetMovementInput());
    }
}
