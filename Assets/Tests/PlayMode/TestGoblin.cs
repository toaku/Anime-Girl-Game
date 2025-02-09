using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics.Tracing;

public class TestGoblin
{
    private bool isLoaded = false;

    private Goblin goblin;
    private Vector2 goblinOriginPosition;
    private Vector3 goblinOriginLocalScale;

    private Player player;
    private Vector2 playerOriginPosition;

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /*
     * 테스트용 scene 이름과 다른 테스트에서 공통으로 사용하는 초기화 로직을 attribute 와 같이 부모로 올리고 
     * 자식의 생성자에서 scene 이름 부모 변수를 초기화
     * 테스트마다 별개로 필요한 참조는 자식 테스트에서 참조하면
     * 설정 로직은 공통으로 작성할 수 있게 됨
     * 혹은 yield return startcoroutine(coroutine); 형식으로 작성하면 다른 코루틴 함수에
     * 프레임 스킵 등 코루틴 고유 기능과 같은 작업을 통째로 넘길 수 있음
     */
    [UnitySetUp]
    public IEnumerator SetUpBeforeTest()
    {
        if (isLoaded == false)
        {
            yield return null; // scene load
            yield return null; // start
            isLoaded = true;

            goblin = GameObject.Find("Goblin").GetComponent<Goblin>();
            goblinOriginPosition = goblin.rigid.position;
            goblinOriginLocalScale = goblin.transform.localScale;

            player = GameObject.Find("Player").GetComponent<Player>();
            playerOriginPosition = player.rigid.position;
        }
    }

    /* 
     * 1. 이동
     * 이동 중엔 애니메이션을 재생하고 이동하지 않고 있을 땐 애니메이션도 재생하지 않는다
     * 이동 중에 보기를 하지 않는다
     */
    [UnityTest]
    public IEnumerator TestMove()
    {
        ResetTest();

        float goblinAttackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
        player.rigid.position = goblin.rigid.position + new Vector2(goblinAttackDistance * 1.5f, 0f);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        PrivateMemberAccessor.InvokeMethod(goblin, "ControlMovingAnimation", null);

        Assert.AreEqual(false, goblin.rigid.velocity == Vector2.zero, "velocity : " + goblin.rigid.velocity);
        Assert.AreEqual(true, goblin.animator.GetBool("run"));

        float direction = MathF.Sign(goblin.transform.localScale.x);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(direction, MathF.Sign(goblin.transform.localScale.x));

        while (Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isMoving")) == true)
        {
            yield return null;
        }

        Assert.AreEqual(false, goblin.animator.GetBool("run"));
    }

    /* 
     * 2. 정지
     * 최대 이동 거리에 도달하거나 도착 지점에 도달하면 이동을 멈추고 대기한다
     */
    [UnityTest]
    public IEnumerator TestStop()
    {
        ResetTest();

        float maxMoveDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "maxMoveDistance").ToString());

        player.rigid.position = goblin.rigid.position + new Vector2(maxMoveDistance * 2, 0f);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);

        while (Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isMoving")) == true)
        {
            yield return null;
        }

        Assert.AreEqual(true, MathF.Abs(Vector2.Distance(goblinOriginPosition, goblin.rigid.position) - maxMoveDistance) < 0.1f);
        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isStay")));

        ResetTest();

        player.rigid.position = goblin.rigid.position + new Vector2(maxMoveDistance / 2, 0f);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        Vector2 arrivalPosition = (Vector2)PrivateMemberAccessor.GetField(goblin, "arrivalPosition");

        player.rigid.position = player.rigid.position + new Vector2(0f, maxMoveDistance);

        while (Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isMoving")) == true)
        {
            yield return null;
        }

        Assert.AreEqual(true, Vector2.Distance(goblin.rigid.position, arrivalPosition) < 0.1f);
        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isStay")));
    }

    /* 
     * 3. 대기
     * 대기 중 이동, 공격, 보기를 하지 않는다
     */
    [Test]
    public void TestStay()
    {
        ResetTest();

        PrivateMemberAccessor.InvokeMethod(goblin, "Stay", null);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        Assert.AreEqual(Vector2.zero, goblin.rigid.velocity);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryAttack", null);
        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")));

        float direction = Mathf.Sign(goblin.transform.localScale.x);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(direction, Mathf.Sign(goblin.transform.localScale.x));
    }

    /* 
     * 4. 공격
     * 공격 범위에 플레이어가 들어왔을 때 공격 애니메이션을 재생하고 적절한 타이밍에 플레이어의 hp 를 깎는다
     * 공격 시 이동, 보기를 멈춘다
     * 공격 후 대기한다
     */
    [UnityTest]
    public IEnumerator TestAttack()
    {
        ResetTest();

        float attackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
        player.rigid.position = goblin.rigid.position + new Vector2(attackDistance, 0);

        while(Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")) == false)
        {
            yield return null;
        }

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        Assert.AreEqual(Vector2.zero, goblin.rigid.velocity);

        float direction = Mathf.Sign(goblin.transform.localScale.x);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(direction, Mathf.Sign(goblin.transform.localScale.x));

        //transition 에 의해 state 가 전환되길 기다림
        yield return null;
        yield return null;

        Assert.AreEqual(true, goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack"));

        while (Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")) == true)
        {
            yield return null;
        }

        float damage = float.Parse(PrivateMemberAccessor.GetField(goblin, "damage").ToString());
        Assert.AreEqual(damage, player.hp.maxHP - player.hp.currentHP);

        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isStay")));
    }

    /* 
     * 5. 피격
     * 피격 애니메이션을 재생한다
     * 공격, 이동 중이라면 캔슬한다
     * 피격 중 보기를 하지 않는다
     */
    [UnityTest]
    public IEnumerator TestHitted()
    {
        ResetTest();

        float attackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
        player.rigid.position = goblin.rigid.position + new Vector2(attackDistance * 1.5f, 0);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        Assert.AreEqual(true, goblin.rigid.velocity != Vector2.zero);

        PrivateMemberAccessor.InvokeMethod(goblin, "OnHit", goblin.hp, EventArgs.Empty);

        //transition 에 의해 state 가 전환되길 기다림
        yield return null; //Run -> Idle
        yield return null; //Idle -> Hit

        Assert.AreEqual(true, goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Hit"));
        Assert.AreEqual(Vector2.zero, goblin.rigid.velocity);

        ResetTest();

        while(Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")) == false)
        {
            yield return null;
        }
        PrivateMemberAccessor.InvokeMethod(goblin, "OnHit", goblin.hp, EventArgs.Empty);
        Assert.AreEqual(false, PrivateMemberAccessor.GetField(goblin, "isAttack"));

        float direction = Mathf.Sign(goblin.transform.localScale.x);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(direction, Mathf.Sign(goblin.transform.localScale.x));
    }

    /* 
     * 6. 사망
     * 사망 후 이동, 공격, 보기, 피격하지 않는다
     */
    [UnityTest]
    public IEnumerator TestDie()
    {
        ResetTest();

        goblin.hp.Hit(goblin.hp.maxHP);

        //transition 에 의해 state 가 변경되길 기다림
        yield return null;
        yield return null;

        Assert.AreEqual(true, goblin.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die"));

        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
        Assert.AreEqual(Vector2.zero, goblin.rigid.velocity);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryAttack", null);
        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")));

        float direction = Mathf.Sign(goblin.transform.localScale.x);
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(direction, Mathf.Sign(goblin.transform.localScale.x));

        goblin.hp.Hit(1);
        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isHitting")));
    }

    /* 
     * 7. 보기
     * 플레이어가 있는 방향을 바라보는 것처럼 이미지를 바꾼다
     */
    [Test]
    public void TestFlip()
    {
        ResetTest();

        player.rigid.position = goblin.rigid.position + new Vector2(1, 0);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(1, Mathf.Sign(goblin.transform.localScale.x));

        ResetTest();

        player.rigid.position = goblin.rigid.position + new Vector2(-1, 0);

        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
        Assert.AreEqual(-1, Mathf.Sign(goblin.transform.localScale.x));
    }

    // setup attribute 와 test attribute 사이에 update 호출이 있어서 초기화한 값이 다시 재설정되므로 항상 테스트의 최상단에서 명시적으로 호출해줘야함
    private void ResetTest()
    {
        ResetGoblin();
        ResetPlayer();
    }

    // test 가 끝난 후 초기화해야하는 값이 있다면 기입함
    private void ResetGoblin()
    {
        PrivateMemberAccessor.SetField(goblin, "isMoving", false);
        PrivateMemberAccessor.SetField(goblin, "isAttack", false);
        PrivateMemberAccessor.SetField(goblin, "isStay", false);
        PrivateMemberAccessor.SetField(goblin, "isHitting", false);
        PrivateMemberAccessor.SetField(goblin, "isDie", false);

        PrivateMemberAccessor.SetField(goblin.hp, "_currentHP", goblin.hp.maxHP);

        PrivateMemberAccessor.SetField(goblin, "movingStartPosition", Vector2.zero);
        PrivateMemberAccessor.SetField(goblin, "arrivalPosition", Vector2.zero);

        goblin.transform.localScale = goblinOriginLocalScale;

        goblin.rigid.position = goblinOriginPosition;
        goblin.rigid.velocity = Vector2.zero;

        goblin.animator.SetBool("run", false);
        goblin.animator.Play("Base Layer.Idle", 0);

        goblin.StopAllCoroutines();
    }

    // test 가 끝난 후 초기화해야하는 값이 있다면 기입함
    private void ResetPlayer()
    {
        player.rigid.position = playerOriginPosition;
        PrivateMemberAccessor.SetField(player.hp, "_currentHP", player.hp.maxHP);
        PrivateMemberAccessor.SetField(player, "isHit", false);

        player.animator.Play("Base Layer.Idle", 0);
    }

    /*
    [Test]
    public void TestShouldMove()
    {
        bool ShouldMove()
        {
            return Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(goblin, "ShouldMove", null));
        }

        Vector2 playerOriginPosition = player.rigid.position;

        float goblinAttackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());

        player.rigid.position = goblin.rigid.position + new Vector2(goblinAttackDistance * 0.9f, 0f);
        Assert.AreEqual(false, ShouldMove());
        player.rigid.position = goblin.rigid.position + new Vector2(goblinAttackDistance * 1.5f, 0f);

        PrivateMemberAccessor.SetField(goblin, "isMoving", true);
        Assert.AreEqual(false, ShouldMove());
        PrivateMemberAccessor.SetField(goblin, "isMoving", false);

        PrivateMemberAccessor.SetField(goblin, "isAttack", true);
        Assert.AreEqual(false, ShouldMove());
        PrivateMemberAccessor.SetField(goblin, "isAttack", false);

        PrivateMemberAccessor.SetField(goblin, "isHitting", true);
        Assert.AreEqual(false, ShouldMove());
        PrivateMemberAccessor.SetField(goblin, "isHitting", false);

        PrivateMemberAccessor.SetField(goblin, "isStay", true);
        Assert.AreEqual(false, ShouldMove());
        PrivateMemberAccessor.SetField(goblin, "isStay", false);

        PrivateMemberAccessor.SetField(goblin, "isDie", true);
        Assert.AreEqual(false, ShouldMove());
        PrivateMemberAccessor.SetField(goblin, "isDie", false);

        Assert.AreEqual(true, ShouldMove());

        player.rigid.position = playerOriginPosition;
    }

    private void ResetMovingTest()
    {
        PrivateMemberAccessor.SetField(goblin, "isMoving", false);
        PrivateMemberAccessor.SetField(goblin, "movingStartPosition", Vector2.zero);
        PrivateMemberAccessor.SetField(goblin, "arrivalPosition", Vector2.zero);
        goblin.rigid.velocity = Vector2.zero;
    }

    [Test]
    public void TestMove()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "Move", null);

        float sideAngleOfArrival = float.Parse(PrivateMemberAccessor.GetField(goblin, "sideAngleOfArrival").ToString());
        float attackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
        float speed = float.Parse(PrivateMemberAccessor.GetField(goblin, "speed").ToString());

        Vector2 arrivalPosition = (Vector2)PrivateMemberAccessor.GetField(goblin, "arrivalPosition");
        Vector2 playerCenter = new Vector2(player.rigid.position.x, player.rigid.position.y + player.height / 2);

        float sideAngleResult = Vector2.Angle(arrivalPosition - playerCenter, Vector2.right);

        Assert.AreEqual(true, PrivateMemberAccessor.GetField(goblin, "isMoving"));
        Assert.AreEqual(true, 
            sideAngleResult > -sideAngleOfArrival && sideAngleResult < sideAngleOfArrival 
                || sideAngleResult > 180 - sideAngleOfArrival && sideAngleResult < 180 + sideAngleOfArrival);
        Assert.AreEqual(true, Mathf.Abs(Vector2.Distance(arrivalPosition, playerCenter) - attackDistance) < 0.1f);
        Assert.AreEqual(true, goblin.rigid.velocity == (arrivalPosition - goblin.rigid.position).normalized * speed);

        ResetMovingTest();
    }

    [Test]
    public void TestTryMoving()
    {
        bool shouldMove = Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(goblin, "ShouldMove", null));
        if (shouldMove == true)
        {
            PrivateMemberAccessor.InvokeMethod(goblin, "Move", null);
        }

        Assert.AreEqual(shouldMove, PrivateMemberAccessor.GetField(goblin, "isMoving"));
 
        ResetMovingTest();
    }

    [Test]
    public void TestShouldStop()
    {

    }

    [Test]
    public void TestStop()
    {
        goblin.rigid.velocity = new Vector2(1, 0);
        PrivateMemberAccessor.SetField(goblin, "isMoving", true);

        PrivateMemberAccessor.InvokeMethod(goblin, "Stop", null);

        Assert.AreEqual(Vector2.zero, goblin.rigid.velocity);
        Assert.AreEqual(false, PrivateMemberAccessor.GetField(goblin, "isMoving"));
    }

    [Test]
    public void TestControlMovingAnimation()
    {
        void ControlMovingAnimation()
        {
            PrivateMemberAccessor.InvokeMethod(goblin, "ControlMovingAnimation", null);
        }

        PrivateMemberAccessor.SetField(goblin, "isMoving", true);
        ControlMovingAnimation();
        Assert.AreEqual(true, goblin.animator.GetBool("run"));

        PrivateMemberAccessor.SetField(goblin, "isMoving", false);
        ControlMovingAnimation();
        Assert.AreEqual(false, goblin.animator.GetBool("run"));

        PrivateMemberAccessor.SetField(goblin, "isMoving", true);
        goblin.animator.SetBool("run", true);
        PrivateMemberAccessor.SetField(goblin, "isDie", true);
        ControlMovingAnimation();
        Assert.AreEqual(false, goblin.animator.GetBool("run"));

        PrivateMemberAccessor.SetField(goblin, "isDie", false);
        PrivateMemberAccessor.SetField(goblin, "isMoving", false);
    }

    [Test]
    public void TestShouldFlip()
    {
        Vector2 originPlayerPosition = player.rigid.position;

        player.rigid.position = new Vector2(goblin.rigid.position.x - 1, goblin.rigid.position.y);

        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(goblin, "ShouldFlip", null)));

        player.rigid.position = originPlayerPosition;
    }

    [Test]
    public void TestStay()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "Stay", null);

        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isStay")));

        PrivateMemberAccessor.SetField(goblin, "isStay", false);
    }

    [UnityTest]
    public IEnumerator TestIsPlayerInAttackArea()
    {
        float attackDistance = float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
        Vector2 goblinPosition = goblin.transform.position;
        
        player.rigid.position = new Vector2(goblinPosition.x + attackDistance, goblinPosition.y);

        Assert.AreEqual(true, Convert.ToBoolean(PrivateMemberAccessor.InvokeMethod(goblin, "IsPlayerInAttackArea", null)));

        player.rigid.position = new Vector2(player.rigid.position.x + attackDistance + 1, player.rigid.position.y + 1);
        while(Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")) == true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    [Test]
    public void TestAttack()
    {
        Vector2 playerOriginPosition = player.rigid.position;
        float playerMaxHP = player.hp.maxHP;

        float goblinDamage = float.Parse(PrivateMemberAccessor.GetField(goblin, "damage").ToString());

        player.rigid.position = new Vector2(goblin.rigid.position.x + float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString()) / 10f, goblin.rigid.position.y);
        PrivateMemberAccessor.InvokeMethod(goblin, "Attack", null);

        Debug.Log(player.hp.currentHP);
        Assert.AreEqual(playerMaxHP - goblinDamage, player.hp.currentHP);

        player.rigid.position = playerOriginPosition;
        PrivateMemberAccessor.SetField(player.hp, "_currentHP", playerMaxHP);
    }

    [Test]
    public void TestOnHit()
    {
        PrivateMemberAccessor.SetField(goblin, "isAttack", true);
        PrivateMemberAccessor.SetField(goblin, "isMoving", true);

        PrivateMemberAccessor.InvokeMethod(goblin, "OnHit", goblin.hp, EventArgs.Empty);

        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack")));
        Assert.AreEqual(false, Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isMoving")));
    }
    */
}
