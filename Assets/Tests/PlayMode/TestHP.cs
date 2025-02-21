using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestHP
{
    private bool isLoaded = false;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    [UnitySetUp]
    public IEnumerator SetUpBeforeTest()
    {
        if (isLoaded == false)
        {
            yield return null; // scene load
            yield return null; // start
            isLoaded = true;
        }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestHit()
    {
        HP goblinHP = GameObject.Find("Goblin").GetComponent<HP>();

        float damage = 1;
        float currentHP = goblinHP.currentHP;

        bool isHitted = false;
        goblinHP.onHit += (object o, EventArgs args) => { isHitted = true; };

        bool isDie = false;
        goblinHP.onDie += (object o, EventArgs args) => { isDie = true; };

        goblinHP.Hit(damage);

        Assert.AreEqual(currentHP - damage, goblinHP.currentHP);
        Assert.AreEqual(((Slider)PrivateMemberAccessor.GetField(goblinHP, "HPBar")).value, goblinHP.currentHP);
        Assert.AreEqual(true, isHitted);

        goblinHP.Hit(goblinHP.currentHP);

        Assert.AreEqual(true, isDie);
    }
}
