using System;
using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestPlayer : BattleSceneTest
{
    private PlayerTester playerTester;

    protected override void OnSceneLoadingEnd()
    {
        playerTester = new PlayerTester(GetPlayer());
    }

    [UnityTest]
    public IEnumerator TestMove()
    {
        ResetTest();

        Vector2 inputMovement = Vector2.right;
        float speed = playerTester.GetSpeed();

        JoystickTester joystickTester = playerTester.mobileControllerTester.joystickTester;
        RectTransform stick = joystickTester.GetStick();

        joystickTester.ControlJoystick((Vector2)stick.TransformPoint(inputMovement));

        yield return null;
        yield return new WaitForFixedUpdate();

        Vector2 expected = inputMovement * speed;
        Vector2 result = playerTester.player.rigid.velocity;

        Assert.AreEqual(true, Vector2.Distance(expected, result) < 0.1f, "expected : " + expected + ", result : " + result);

        joystickTester.ResetJoystick();

        yield return null;
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(Vector2.zero, playerTester.player.rigid.velocity);
    }

    [UnityTest]
    public IEnumerator TestAttack()
    {
        ResetTest();

        GoblinTester goblinTester = new GoblinTester(GetGoblin());
        float goblinMaxHP = goblinTester.goblin.hp.maxHP;

        float attackDistance = playerTester.GetAttackDistance();
        float damage = playerTester.GetDamage();

        Vector2 goblinPosition = new Vector2(playerTester.player.rigid.position.x + attackDistance / 0.9f, playerTester.player.rigid.position.y + playerTester.player.height * 0.9f);
        Coroutine stopGoblin = playerTester.player.StartCoroutine(StopGoblin(goblinTester.goblin, goblinPosition));

        playerTester.mobileControllerTester.SetAttackButtonIsInput(true);
        yield return null;
        playerTester.mobileControllerTester.SetAttackButtonIsInput(false);

        Vector2 inputMovement = Vector2.right;
        float speed = playerTester.GetSpeed();

        JoystickTester joystickTester = playerTester.mobileControllerTester.joystickTester;
        RectTransform stick = joystickTester.GetStick();

        joystickTester.ControlJoystick((Vector2)stick.TransformPoint(inputMovement));

        yield return null;
        yield return new WaitForFixedUpdate();

        Vector2 expected = inputMovement * speed;
        Vector2 result = playerTester.player.rigid.velocity;

        Assert.AreEqual(Vector2.zero, playerTester.player.rigid.velocity);

        while (playerTester.GetIsAttack() == true)
        {
            yield return null;
        }

        Assert.AreEqual(goblinMaxHP - damage, goblinTester.goblin.hp.currentHP);
    }

    private IEnumerator StopGoblin(Goblin goblin, Vector2 position)
    {
        while (true)
        {
            if(goblin.rigid.position != position)
                goblin.rigid.position = position;
            yield return null;
        }
    }

    [UnityTest]
    public IEnumerator TestHit()
    {
        ResetTest();

        while (playerTester.GetIsHit() == false)
        {
            yield return null;
        }

        Assert.AreEqual(true, playerTester.player.hp.maxHP != playerTester.player.hp.currentHP);

        Vector2 inputMovement = Vector2.right;
        float speed = playerTester.GetSpeed();

        JoystickTester joystickTester = playerTester.mobileControllerTester.joystickTester;
        RectTransform stick = joystickTester.GetStick();

        joystickTester.ControlJoystick((Vector2)stick.TransformPoint(inputMovement));
        yield return null;
        yield return new WaitForFixedUpdate();

        Vector2 expected = inputMovement * speed;
        Vector2 result = playerTester.player.rigid.velocity;

        Assert.AreEqual(Vector2.zero, playerTester.player.rigid.velocity);

        playerTester.mobileControllerTester.SetAttackButtonIsInput(true);
        yield return null;
        playerTester.mobileControllerTester.SetAttackButtonIsInput(false);

        Assert.AreEqual(false, playerTester.GetIsAttack());
    }

    [UnityTest]
    public IEnumerator TestDie()
    {
        ResetTest();

        playerTester.player.hp.Hit(playerTester.player.hp.maxHP);

        Vector2 inputMovement = Vector2.right;
        float speed = playerTester.GetSpeed();

        JoystickTester joystickTester = playerTester.mobileControllerTester.joystickTester;
        RectTransform stick = joystickTester.GetStick();

        joystickTester.ControlJoystick((Vector2)stick.TransformPoint(inputMovement));
        yield return null;
        yield return new WaitForFixedUpdate();

        Vector2 expected = inputMovement * speed;
        Vector2 result = playerTester.player.rigid.velocity;

        Assert.AreEqual(Vector2.zero, playerTester.player.rigid.velocity);

        playerTester.mobileControllerTester.SetAttackButtonIsInput(true);
        yield return null;
        playerTester.mobileControllerTester.SetAttackButtonIsInput(false);

        Assert.AreEqual(false, PrivateMemberAccessor.GetField(playerTester.player, "isAttack"));
    }

    private void ResetTest()
    {
        playerTester.ResetPlayer();
        playerTester.mobileControllerTester.ResetMobileController();
    }
}
