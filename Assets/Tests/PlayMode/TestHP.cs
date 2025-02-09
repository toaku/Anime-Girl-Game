using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

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

        goblinHP.Hit(damage);

        Assert.AreEqual(currentHP - damage, goblinHP.currentHP);
        Assert.AreEqual(true, isHitted);
    }

    /*
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestHPWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    */
}
