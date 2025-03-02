
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class Goblin : Unit
{
    public HP hp;
    private bool isHitting = false;
    private bool isDie = false;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxMoveDistance;
    [SerializeField]
    private int sideAngleOfArrival;
    private Vector2 movingStartPosition;
    private Vector2 arrivalPosition;
    private bool isMoving = false;
    public Rigidbody2D rigid
    {
        get;
        private set;
    }

    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private AudioSource attackSound;
    private bool isAttack = false;

    [SerializeField]
    private float stayTime;
    private float stayCurrentTime;
    private bool isStay = false;

    [SerializeField]
    private Player player;

    public Animator animator
    {
        get;
        private set;
    }

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        hp.onHit += OnHit;
        hp.onDie += OnDie;
    }

    void Update()
    {
        if (IsNotSleep() == true)
        {
            if (ShouldAttack())
                StartAttack();
        }

        ControlMovingAnimation();

        CheckStayTime();
        if (stayCurrentTime > stayTime)
            StayEnd();
    }

    void FixedUpdate()
    {
        if (IsNotSleep() == true)
        {
            if (ShouldMove())
                Move();

            if (ShouldStop() == true)
            {
                Stop();
                Stay();
            }
        }
    }

    //Attack 애니메이션 이벤트
    public void OnSwingSword()
    {
        attackSound.Play();
        GiveDamage();
    }

    //Attack 애니메이션 이벤트
    public void OnAttackEnd()
    {
        EndAttack();
    }

    //Hit 애니메이션 이벤트
    public void OnHitEnd()
    {
        EndHit();
    }

    private bool IsNotSleep()
    {
        bool isNotSleep = false;
        if(isDie == false && isStay == false)
        {
            isNotSleep = true;
        }

        return isNotSleep;
    }

    private bool ShouldMove()
    {
        bool shouldMove = false;
        if (IsPlayerInAttackArea() == false 
            && isMoving == false 
            && isAttack == false 
            && isHitting == false)
        {
            shouldMove = true;
        }

        return shouldMove;
    }

    private void Move()
    {
        if (ShouldFlip())
            Flip();

        isMoving = true;

        int arrivalAngle = Random.Range(-sideAngleOfArrival, sideAngleOfArrival + 1); // 최대값은 범위에 포함되지 않기 때문에 1 을 더해서 포함시킴
        arrivalAngle += Random.Range(0, 2) * 180;

        movingStartPosition = rigid.position;
        arrivalPosition = new Vector2(player.rigid.position.x, player.rigid.position.y + player.height / 2)
            + (Vector2)(Quaternion.AngleAxis(arrivalAngle, Vector3.back) * new Vector2(attackDistance, 0));
        Vector2 arrivalDirection = (arrivalPosition - rigid.position).normalized;

        rigid.velocity = arrivalDirection * speed;
    }

    private void ControlMovingAnimation()
    {
        bool animatorRun = animator.GetBool("run");
        if (isMoving == true && isDie == false)
        {
            if (animatorRun == false)
                animator.SetBool("run", true);
        }
        else
        {
            if (animatorRun == true)
                animator.SetBool("run", false);
        }
    }

    private bool IsArrived()
    {
        bool isArrived = false;
        if (Vector2.Distance(movingStartPosition, rigid.position) > maxMoveDistance
            || Vector2.Distance(rigid.position, arrivalPosition) < 0.1f)
        {
            isArrived = true;
        }

        return isArrived;
    }

    private bool ShouldStop()
    {
        bool shouldStop = false;
        if (isMoving == true && (IsArrived() == true || isAttack == true))
        {
            shouldStop = true;
        }

        return shouldStop;
    }

    private void Stop()
    {
        movingStartPosition = Vector2.zero;
        arrivalPosition = Vector2.zero;
        rigid.velocity = Vector2.zero;
        isMoving = false;
    }

    private bool ShouldFlip()
    {
        bool shouldFlip = false;

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(direction, player.rigid.position - rigid.position);

        if (angle > 90)
            shouldFlip = true;

        return shouldFlip;
    }

    private void Stay()
    {
        isStay = true;
        stayCurrentTime = 0f;
    }

    private void StayEnd()
    {
        isStay = false;
    }

    private void CheckStayTime()
    {
        stayCurrentTime += Time.deltaTime;
    }

    private bool IsPlayerInAttackArea()
    {
        bool isPlayerInAttackArea = false;

        float attackDirection = Mathf.Sign(transform.localScale.x);

        Collider2D player = Physics2D.OverlapArea(rigid.position, new Vector2(rigid.position.x + attackDistance * attackDirection, rigid.position.y + height), LayerMask.GetMask("Player"));
        if (player != null) 
        {
            isPlayerInAttackArea = true;
        }

        return isPlayerInAttackArea;
    }

    private bool ShouldAttack()
    {
        bool shouldAttack = false;
        if (IsPlayerInAttackArea() && isAttack == false && isHitting == false)
        {
            shouldAttack = true;
        }

        return shouldAttack;
    }

    private void StartAttack()
    {
        isAttack = true;
        animator.SetTrigger("attack");
    }

    private void EndAttack()
    {
        isAttack = false;
        Stay();
    }

    private void GiveDamage()
    {
        if (IsPlayerInAttackArea())
        {
            player.hp.Hit(damage);
        }
    }

    private void OnHit(object obj, EventArgs args)
    {
        isHitting = true;
        animator.SetTrigger("hit");

        if (isAttack == true)
            isAttack = false;
        if (isMoving == true)
            Stop();
    }

    private void EndHit()
    {
        isHitting = false;
    }

    private void OnDie(object obj, EventArgs args)
    {
        isDie = true;
        animator.SetTrigger("die");
        Stop();
    }
}
