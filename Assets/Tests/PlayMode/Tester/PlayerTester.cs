using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTester
{
    public Player player
    {
        get;
        private set;
    }

    private Vector2 playerOriginPosition;

    public MobileControllerTester mobileControllerTester
    {
        get;
        private set;
    }

    public PlayerTester(Player player)
    {
        this.player = player;
        mobileControllerTester = new MobileControllerTester(GetMobileController());
        playerOriginPosition = player.rigid.position;
    }

    private MobileController GetMobileController()
    {
        return (MobileController)PrivateMemberAccessor.GetField(player, "mobileController");
    }

    public float GetSpeed()
    {
        return float.Parse(PrivateMemberAccessor.GetField(player, "speed").ToString());
    }

    private void SetCurrentHP(float currentHP)
    {
        PrivateMemberAccessor.SetField(player.hp, "_currentHP", currentHP);
    }

    public float GetDamage()
    {
        return float.Parse(PrivateMemberAccessor.GetField(player, "damage").ToString());
    }

    public float GetAttackDistance()
    {
        return float.Parse(PrivateMemberAccessor.GetField(player, "attackDistance").ToString());
    }

    public bool GetIsAttack()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(player, "isAttack"));
    }

    public bool GetIsHit()
    {
        return Convert.ToBoolean(PrivateMemberAccessor.GetField(player, "isHit"));
    }

    private void SetIsHit(bool isHit)
    {
        PrivateMemberAccessor.SetField(player, "isHit", isHit);
    }

    private void SetIsDie(bool isDie)
    {
        PrivateMemberAccessor.SetField(player, "isDie", isDie);
    }

    public void ResetPlayer()
    {
        player.rigid.position = playerOriginPosition;

        SetCurrentHP(player.hp.maxHP);
        SetIsHit(false);
        SetIsDie(false);

        player.rigid.velocity = Vector2.zero;
        player.animator.Play("Base Layer.Idle", 0);

        player.StopAllCoroutines();
    }
}
