using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public abstract class SceneBaseTest
{
    private bool isLoaded = false;

    [OneTimeSetUp]
    public abstract void LoadScene();

    [UnitySetUp]
    public IEnumerator SetUpBeforeTest()
    {
        if (isLoaded == false)
        {
            yield return null; // scene load
            yield return null; // start
            isLoaded = true;

            OnSceneLoadingEnd();
        }
    }

    protected abstract void OnSceneLoadingEnd();
}
