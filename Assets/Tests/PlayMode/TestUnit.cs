using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestUnit
{
    Unit unit;

    [OneTimeSetUp]
    public void CreateUnit()
    {
        GameObject go = new GameObject();
        unit = go.AddComponent<Unit>();
    }

    [Test]
    public void TestFlip()
    {
        float originLocalScaleX = unit.transform.localScale.x;

        PrivateMemberAccessor.InvokeMethod(unit, "Flip", null);

        Assert.AreEqual(-1, unit.transform.localScale.x);

        unit.transform.localScale = new Vector3(originLocalScaleX, unit.transform.localScale.y, unit.transform.localScale.z);
    }
}
