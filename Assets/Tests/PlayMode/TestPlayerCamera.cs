using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestPlayerCamera : BattleSceneTest
{
    private PlayerCamera playerCamera;
    private Player player;

    protected override void OnSceneLoadingEnd()
    {
        playerCamera = GetPlayerCamera();
        player = GetPlayer();
    }

    [Test]
    public void TestFollowPlayer()
    {
        player.transform.position = new Vector2(1f, 0f);
        PrivateMemberAccessor.InvokeMethod(playerCamera, "FollowPlayer", null);

        Vector2 playerPosition = player.transform.position;
        Vector2 playerCameraPosition = playerCamera.transform.position;

        Assert.AreEqual(true, playerPosition == playerCameraPosition, "expected : " + playerPosition + ", result : " + playerCameraPosition);
    }
}
