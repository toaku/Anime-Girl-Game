using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics.Tracing;

public class TestGoblin : BattleSceneTest
{
    private GoblinTester goblinTester;
    private PlayerTester playerTester;

    protected override void OnSceneLoadingEnd()
    {
        goblinTester = new GoblinTester(GetGoblin());
        playerTester = new PlayerTester(GetPlayer());
    }

    [UnityTest]
    public IEnumerator TestMove()
    {
        ResetTest();

        float goblinAttackDistance = goblinTester.GetAttackDistance();
        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(goblinAttackDistance * 1.5f, 0f);

        yield return new WaitForFixedUpdate();
        goblinTester.ControlMovingAnimation();

        Assert.AreEqual(false, goblinTester.goblin.rigid.velocity == Vector2.zero, "velocity : " + goblinTester.goblin.rigid.velocity);
        Assert.AreEqual(true, goblinTester.goblin.animator.GetBool("run"));

        float direction = MathF.Sign(goblinTester.goblin.transform.localScale.x);
        yield return new WaitForFixedUpdate();
        Assert.AreEqual(direction, MathF.Sign(goblinTester.goblin.transform.localScale.x));

        while (goblinTester.GetIsMoving() == true)
        {
            yield return null;
        }

        Assert.AreEqual(false, goblinTester.goblin.animator.GetBool("run"));
    }

    [UnityTest]
    public IEnumerator TestStop()
    {
        ResetTest();

        float maxMoveDistance = goblinTester.GetMaxMoveDistance();

        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(maxMoveDistance * 2, 0f);
        yield return new WaitForFixedUpdate();

        while (goblinTester.GetIsMoving() == true)
        {
            yield return null;
        }

        float moveDistance = Vector2.Distance(goblinTester.goblinOriginPosition, goblinTester.goblin.rigid.position);
        Assert.AreEqual(true, Mathf.Abs(moveDistance - maxMoveDistance) < 0.1f, "이동 거리 : " + moveDistance + ", 최대 이동 거리 : " + maxMoveDistance);
        Assert.AreEqual(true, goblinTester.GetIsStay());

        ResetTest();

        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(maxMoveDistance / 2, 0f);

        yield return new WaitForFixedUpdate();
        Vector2 arrivalPosition = goblinTester.GetArrivalPosition();

        playerTester.player.rigid.position = playerTester.player.rigid.position + new Vector2(0f, maxMoveDistance);

        while (goblinTester.GetIsMoving() == true)
        {
            yield return null;
        }

        Assert.AreEqual(true, Vector2.Distance(goblinTester.goblin.rigid.position, arrivalPosition) < 0.1f, "현재 위치 : " + goblinTester.goblin.rigid.position + ", 도착 예상 위치 : " + arrivalPosition);
        Assert.AreEqual(true, goblinTester.GetIsStay());
    }

    [UnityTest]
    public IEnumerator TestStay()
    {
        ResetTest();

        goblinTester.Stay();

        yield return null;
        Assert.AreEqual(Vector2.zero, goblinTester.goblin.rigid.velocity);

        yield return null;
        Assert.AreEqual(false, goblinTester.GetIsAttack());

        float direction = Mathf.Sign(goblinTester.goblin.transform.localScale.x);
        yield return null;
        Assert.AreEqual(direction, Mathf.Sign(goblinTester.goblin.transform.localScale.x));
    }

    [UnityTest]
    public IEnumerator TestAttack()
    {
        ResetTest();

        float attackDistance = goblinTester.GetAttackDistance();
        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(attackDistance, 0);

        while (goblinTester.GetIsAttack() == false)
        {
            yield return null;
        }

        float direction = Mathf.Sign(goblinTester.goblin.transform.localScale.x);

        yield return new WaitForFixedUpdate();
        Assert.AreEqual(Vector2.zero, goblinTester.goblin.rigid.velocity);

        yield return null;
        Assert.AreEqual(direction, Mathf.Sign(goblinTester.goblin.transform.localScale.x));

        //transition 에 의해 state 가 전환되길 기다림
        yield return null;
        yield return null;

        Assert.AreEqual(true, goblinTester.goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack"));

        while (goblinTester.GetIsAttack() == true)
        {
            yield return null;
        }

        float damage = goblinTester.GetDamage();
        Assert.AreEqual(damage, playerTester.player.hp.maxHP - playerTester.player.hp.currentHP);
    }

    [UnityTest]
    public IEnumerator TestHitted()
    {
        ResetTest();

        float attackDistance = goblinTester.GetAttackDistance();
        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(attackDistance * 1.5f, 0);

        yield return new WaitForFixedUpdate();
        Assert.AreEqual(true, goblinTester.goblin.rigid.velocity != Vector2.zero);

        goblinTester.goblin.hp.Hit(1f);

        //transition 에 의해 state 가 전환되길 기다림
        yield return null; //Run -> Idle
        yield return null; //Idle -> Hit

        Assert.AreEqual(true, goblinTester.goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Hit"));
        Assert.AreEqual(Vector2.zero, goblinTester.goblin.rigid.velocity);

        ResetTest();

        while (goblinTester.GetIsAttack() == false)
        {
            yield return null;
        }

        goblinTester.goblin.hp.Hit(1f);

        Assert.AreEqual(false, goblinTester.GetIsAttack());
    }

    [UnityTest]
    public IEnumerator TestDie()
    {
        ResetTest();

        goblinTester.goblin.hp.Hit(goblinTester.goblin.hp.maxHP);

        //transition 에 의해 state 가 변경되길 기다림
        yield return null;
        yield return null;

        Assert.AreEqual(true, goblinTester.goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die"));

        float direction = Mathf.Sign(goblinTester.goblin.transform.localScale.x);

        yield return null;

        Assert.AreEqual(Vector2.zero, goblinTester.goblin.rigid.velocity);
        Assert.AreEqual(false, goblinTester.GetIsAttack());
        Assert.AreEqual(direction, Mathf.Sign(goblinTester.goblin.transform.localScale.x));

        goblinTester.goblin.hp.Hit(1);
        Assert.AreEqual(false, goblinTester.GetIsHitting());
    }

    [UnityTest]
    public IEnumerator TestFlip()
    {
        ResetTest();

        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(1, 0);

        yield return null;
        Assert.AreEqual(1, Mathf.Sign(goblinTester.goblin.transform.localScale.x));

        ResetTest();

        playerTester.player.rigid.position = goblinTester.goblin.rigid.position + new Vector2(-1, 0);

        yield return new WaitForFixedUpdate();
        Assert.AreEqual(-1, Mathf.Sign(goblinTester.goblin.transform.localScale.x));
    }

    // setup attribute 와 test attribute 사이에 update 호출이 있어서 초기화한 값이 다시 재설정되므로 항상 테스트의 최상단에서 명시적으로 호출해줘야함
    private void ResetTest()
    {
        goblinTester.ResetGoblin();
        playerTester.ResetPlayer();
    }
}
