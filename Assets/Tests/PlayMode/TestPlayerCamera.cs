using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestPlayerCamera
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

    //A Test behaves as an ordinary method
    [Test]
    public void TestFollowPlayer()
    {
        PlayerCamera playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
        Player player = GameObject.Find("Player").GetComponent<Player>();

        player.transform.position = new Vector2(1f, 0f);
        PrivateMemberAccessor.InvokeMethod(playerCamera, "FollowPlayer", null);

        Vector2 playerPosition = player.transform.position;
        Vector2 playerCameraPosition = playerCamera.transform.position;

        Assert.AreEqual(true, playerPosition == playerCameraPosition, "expected : " + playerPosition + ", result : " + playerCameraPosition);
    }

    /*
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestFollowPlayer()
    {
        PlayerCamera playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
        yield return null;
    }
    */
}
