using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MobileControllerTester
{
    public MobileController mobileController
    {
        get;
        private set;
    }

    public JoystickTester joystickTester
    {
        get;
        private set;
    }

    public MobileControllerTester(MobileController mobileController)
    {
        this.mobileController = mobileController;
        joystickTester = new JoystickTester(GetJoystick());
    }

    public Joystick GetJoystick()
    {
        return (Joystick)PrivateMemberAccessor.GetField(mobileController, "joystick");
    }

    public TouchButton GetAttackButton()
    {
        return (TouchButton)PrivateMemberAccessor.GetField(mobileController, "attackButton");
    }

    public void SetAttackButtonIsInput(bool isInput)
    {
        PrivateMemberAccessor.SetFieldByPropertySetter(GetAttackButton(), "isInput", isInput, false);
    }

    public void ResetMobileController()
    {
        SetAttackButtonIsInput(false);
        joystickTester.ResetJoystickForTest();
    }
}
