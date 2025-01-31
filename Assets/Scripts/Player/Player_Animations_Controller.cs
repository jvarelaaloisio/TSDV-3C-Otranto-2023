using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations_Controller : MonoBehaviour
{
    [Header("Anim Setup")]
    [SerializeField] private Animator anim;
    private Player_Controller controller;
    //TODO: TP2 - Syntax - Consistency in naming convention
    private Player_Movement movement_Controller;

    private void OnEnable()
    {
        controller ??= GetComponent<Player_Controller>();
        if (!controller)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(controller)} is null");
        }

        movement_Controller ??= GetComponent<Player_Movement>();
        if (!movement_Controller)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(movement_Controller)} is null");
        }

        anim ??= GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(anim)} is null");
        }

        controller.OnPlayerMove += Controller_OnPlayerMove;
        controller.OnPlayerJump += Controller_OnPlayerJump;
        controller.OnPlayerSprint += Controller_OnPlayerSprint;
        controller.OnPlayerAttack += Controller_OnPlayerAttack;
        controller.OnPlayerBlock += Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage += Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead += Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump += Controller_OnPlayerJump;
    }

    private void Update()
    {
        //TODO: TP2 - SOLID
        if (movement_Controller.isGrounded())
        {
            //TODO - Fix - Hardcoded value
            anim.SetBool("IsJumping",false);
        }
        else
        {
            anim.SetBool("IsJumping", true);
        }
    }

    private void Controller_OnPlayerMove(Vector2 obj)
    {
        Vector3 pos = new Vector3(obj.x,0f,obj.y);
        //TODO - Fix - Hardcoded value
        anim.SetFloat("VelocityX/Z", pos.magnitude - pos.y);
    }


    private void Controller_OnPlayerJump(bool obj)
    {
        anim.SetBool("IsJumping", obj);
        //TODO: TP2 - Remove unused methods/variables
        //anim.Play("Jump");
    }

    private void Controller_OnPlayerJump(float obj)
    {
        anim.SetFloat("VelocityY",obj);
    }

    private void Controller_OnPlayerSprint(bool obj)
    {
        anim.SetBool("IsRuning",obj);
    }

    private void Controller_OnPlayerBlock(bool obj)
    {
        anim.SetBool("Blocking",obj);
    }

    private void Controller_OnPlayerAttack(bool obj)
    {
        anim.SetBool("Attacking",obj);
    }

    private void Controller_OnPlayerTakeDamage()
    {
        anim.Play("GetHit");
    }

    private void Controller_OnPlayerDead()
    {
        anim.Play("Death");
    }


    private void OnDisable()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage -= Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead -= Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump -= Controller_OnPlayerJump;
    }

    private void OnDestroy()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
        controller.OnPlayerAttack -= Controller_OnPlayerAttack;
        controller.OnPlayerBlock -= Controller_OnPlayerBlock;
        controller.OnPlayerTakeDamage -= Controller_OnPlayerTakeDamage;
        controller.OnPlayerDead -= Controller_OnPlayerDead;

        movement_Controller.OnPlayerJump -= Controller_OnPlayerJump;
    }
}
