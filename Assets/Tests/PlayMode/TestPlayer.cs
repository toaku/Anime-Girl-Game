using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestPlayer
{
    private bool isLoaded = false;

    private Player player;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    [UnitySetUp]
    public IEnumerator SetUpBeforeTest()
    {
        if(isLoaded == false)
        {
            yield return null; // scene load
            yield return null; // start
            isLoaded = true;

            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }

    [Test]
    public void TestShouldRun()
    {
        PrivateMemberAccessor.SetField(player, "inputHorizontal", 1);

        Assert.AreEqual(true, PrivateMemberAccessor.InvokeMethod(player, "ShouldRun", null));
    }

    [Test]
    public void TestRun()
    {
        float inputHorizontal = 1;
        float inputVertical = 0;

        float speed = float.Parse(PrivateMemberAccessor.GetField(player, "speed").ToString());

        PrivateMemberAccessor.SetField(player, "inputHorizontal", inputHorizontal);
        PrivateMemberAccessor.SetField(player, "inputVertical", inputVertical);

        PrivateMemberAccessor.InvokeMethod(player, "Run", null);

        Vector2 expected = new Vector2(inputHorizontal, inputVertical) * speed;
        Vector2 result = player.rigid.velocity;

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void TestStop()
    {
        player.rigid.velocity = new Vector2(1, 0);
        PrivateMemberAccessor.InvokeMethod(player, "Stop", null);

        Assert.AreEqual(Vector2.zero, player.rigid.velocity);
    }

    [UnityTest]
    public IEnumerator TestDisableActionOnAttack()
    {
        //한글 주석 인코딩 테스트
        PrivateMemberAccessor.InvokeMethod(player, "PlayAttackAnim", null);

        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(player, "ShouldRun", null)));
        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(player, "ShouldFlip", null)));

        while(Convert.ToBoolean(PrivateMemberAccessor.GetField(player, "isAttack")) == true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
}
