using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputHorizontal;
    private float inputVertical;

    [SerializeField]
    private float speed;
    public Rigidbody2D rigid { get; private set; }

    private bool isAttack = false;

    public Animator animator { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
            PlayAttackAnim();
        }
    }

    void FixedUpdate()
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

    //Attack 애니메이션 이벤트
    public void OnAttackEnd()
    {
        isAttack = false;
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

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void PlayAttackAnim()
    {
        isAttack = true;
        animator.SetTrigger("attack");
    }
}
