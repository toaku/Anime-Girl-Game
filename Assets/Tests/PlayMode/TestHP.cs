using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TestHP : BattleSceneTest
{
    HP goblinHP;

    protected override void OnSceneLoadingEnd()
    {
        goblinHP = GetGoblin().GetComponent<HP>();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestHit()
    {
        goblinHP = GameObject.Find("Goblin").GetComponent<HP>();

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
