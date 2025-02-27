using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GoblinTester
{
    public Goblin goblin
    {
        get;
        private set;
    }

    public Vector2 goblinOriginPosition
    {
        get;
        private set;
    }
    public Vector3 goblinOriginLocalScale
    {
        get;
        private set;
    }

    public GoblinTester(Goblin goblin)
    {
        this.goblin = goblin;
        goblinOriginPosition = goblin.rigid.position;
        goblinOriginLocalScale = goblin.transform.localScale;
    }

    private void SetCurrentHP(float currentHP)
    {
        PrivateMemberAccessor.SetField(goblin.hp, "_currentHP", currentHP);
    }

    public float GetMaxMoveDistance()
    {
        return float.Parse(PrivateMemberAccessor.GetField(goblin, "maxMoveDistance").ToString());
    }

    private void SetMovingStartPosition(Vector2 movingStartPosition)
    {
        PrivateMemberAccessor.SetField(goblin, "movingStartPosition", movingStartPosition);
    }

    public Vector2 GetArrivalPosition()
    {
        return (Vector2)PrivateMemberAccessor.GetField(goblin, "arrivalPosition");
    }

    private void SetArrivalPosition(Vector2 arrivalPosition)
    {
        PrivateMemberAccessor.SetField(goblin, "arrivalPosition", arrivalPosition);
    }

    public bool GetIsMoving()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isMoving"));
    }

    private void SetIsMoving(bool isMoving)
    {
        PrivateMemberAccessor.SetField(goblin, "isMoving", isMoving);
    }

    public bool GetIsAttack()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isAttack"));
    }

    private void SetIsAttack(bool isAttack)
    {
        PrivateMemberAccessor.SetField(goblin, "isAttack", isAttack);
    }

    public float GetDamage()
    {
        return float.Parse(PrivateMemberAccessor.GetField(goblin, "damage").ToString());
    }

    public float GetAttackDistance()
    {
        return float.Parse(PrivateMemberAccessor.GetField(goblin, "attackDistance").ToString());
    }

    public bool GetIsStay()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isStay"));
    }

    private void SetIsStay(bool isStay)
    {
        PrivateMemberAccessor.SetField(goblin, "isStay", isStay);
    }

    public bool GetIsHitting()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(goblin, "isHitting"));
    }

    private void SetIsHitting(bool isHitting)
    {
        PrivateMemberAccessor.SetField(goblin, "isHitting", isHitting);
    }

    private void SetIsDie(bool isDie)
    {
        PrivateMemberAccessor.SetField(goblin, "isDie", isDie);
    }

    public void TryMoving()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "TryMoving", null);
    }

    public void ControlMovingAnimation()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "ControlMovingAnimation", null);
    }

    public void TryAttack()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "TryAttack", null);
    }

    public void TryFlipping()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "TryFlipping", null);
    }

    public void Stay()
    {
        PrivateMemberAccessor.InvokeMethod(goblin, "Stay", null);
    }

    public void ResetGoblin()
    {
        SetIsMoving(false);
        SetIsAttack(false);
        SetIsStay(false);
        SetIsHitting(false);
        SetIsDie(false);

        SetCurrentHP(goblin.hp.maxHP);

        SetMovingStartPosition(Vector2.zero);
        SetArrivalPosition(Vector2.zero);

        goblin.transform.localScale = goblinOriginLocalScale;

        goblin.rigid.position = goblinOriginPosition;
        goblin.rigid.velocity = Vector2.zero;

        goblin.animator.SetBool("run", false);
        goblin.animator.Play("Base Layer.Idle", 0);

        goblin.StopAllCoroutines();
    }
}
