using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    private float inputHorizontal;
    private float inputVertical;

    public HP hp;
    private bool isHit = false;
    private bool isDie = false;

    [SerializeField]
    private float speed;
    public Rigidbody2D rigid { get; private set; }

    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackDistance;
    private bool isAttack = false;

    public Animator animator { get; private set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        hp.onHit += OnHit;
        hp.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit == false && isDie == false)
        {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            bool animRun = animator.GetBool("run");
            if (ShouldRun())
            {
                if (animRun == false)
                    animator.SetBool("run", true);
            }
            else
            {
                if (animRun == true)
                    animator.SetBool("run", false);
            }

            if (ShouldFlip())
            {
                Flip();
            }

            if (Input.GetButtonDown("Attack") && isAttack == false)
            {
                PlayAttackAnimation();
            }
        }
    }

    void FixedUpdate()
    {
        if(isHit == false && isDie == false)
        {
            if (ShouldRun())
            {
                Run();
            }
            else
            {
                Stop();
            }
        }
    }

    //Attack 애니메이션 이벤트
    public void OnSwingStick()
    {
        Attack();
    }

    //Attack 애니메이션 이벤트
    public void OnAttackEnd()
    {
        isAttack = false;
    }

    //Hit 애니메이션 이벤트
    public void OnHitEnd()
    {
        isHit = false;
    }

    private bool ShouldRun()
    {
        bool shouldRun = false;
        if ((inputHorizontal != 0 || inputVertical != 0) && isAttack == false)
        {
            shouldRun = true;
        }

        return shouldRun;
    }

    private void Run()
    {
        Vector2 velocity = new Vector2(inputHorizontal, inputVertical) * speed;
        if(rigid.velocity != velocity)
        {
            rigid.velocity = velocity;
        }
    }

    private void Stop()
    {
        if (rigid.velocity != Vector2.zero)
            rigid.velocity = Vector2.zero;
    }

    private bool ShouldFlip()
    {
        bool shouldFlip = false;
        if ((inputHorizontal < 0 && transform.localScale.x > 0 || inputHorizontal > 0 && transform.localScale.x < 0) && isAttack == false)
        {
            shouldFlip = true;
        }

        return shouldFlip;
    }

    private void PlayAttackAnimation()
    {
        isAttack = true;
        animator.SetTrigger("attack");
    }

    private void Attack()
    {
        LayerMask ignorePlayer = ~LayerMask.GetMask("Player");
        float attackDirection = Mathf.Sign(transform.localScale.x);

        Collider2D target = Physics2D.OverlapArea(rigid.position, new Vector2(rigid.position.x + attackDistance * attackDirection, rigid.position.y + height), ignorePlayer);
        if(target != null)
        {
            target.GetComponent<Goblin>().hp.Hit(damage);
        }
    }

    private void OnHit(object obj, EventArgs args)
    {
        isHit = true;

        isAttack = false;
        Stop();

        animator.SetTrigger("hit");
    }

    private void OnDie(object obj, EventArgs args)
    {
        isDie = true;
        animator.SetTrigger("die");
    }
}
