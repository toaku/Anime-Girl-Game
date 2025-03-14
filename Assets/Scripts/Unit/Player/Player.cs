using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [SerializeField]
    private MobileController mobileController;
    private IPlayerInput playerInput;
    private Vector2 inputMovement;

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
    [SerializeField]
    private AudioSource attackSound;
    private bool isAttack = false;

    public Animator animator { get; private set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerInput = mobileController;

        hp.onHit += OnHit;
        hp.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsNotSleep() == true)
        {
            SetInputMovement();

            if (ShouldFlip())
                Flip();

            if (playerInput.GetAttackInput() && isAttack == false)
            {
                PlayAttackAnimation();
            }
        }

        ControlMoveAnimation();
    }

    void FixedUpdate()
    {
        if (IsNotSleep() == true)
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
        attackSound.Play();
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

    private bool IsNotSleep()
    {
        bool isNotSleep = false;
        if (isHit == false && isDie == false)
            isNotSleep = true;

        return isNotSleep;
    }

    private void SetInputMovement()
    {
        inputMovement = playerInput.GetMovementInput();
    }

    private void ControlMoveAnimation()
    {
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
    }

    private bool ShouldRun()
    {
        bool shouldRun = false;
        if (inputMovement != Vector2.zero && isAttack == false)
        {
            shouldRun = true;
        }

        return shouldRun;
    }

    private void Run()
    {
        Vector2 velocity = inputMovement * speed;
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
        if ((inputMovement.x < 0 && transform.localScale.x > 0 || inputMovement.x > 0 && transform.localScale.x < 0) && isAttack == false)
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
