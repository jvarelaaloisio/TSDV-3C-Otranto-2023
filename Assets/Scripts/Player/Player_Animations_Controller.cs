using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations_Controller : MonoBehaviour
{
    [Header("Anim Setup")]
    [SerializeField] private Animator anim;
    private Player_Controller controller;
    private Player_Movement movement_Controller;

    private void Awake()
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
    }

    private void Start()
    {
        controller.OnPlayerMove += Controller_OnPlayerMove;
        controller.OnPlayerJump += Controller_OnPlayerJump;
        controller.OnPlayerSprint += Controller_OnPlayerSprint;

        movement_Controller.OnPlayerJump += Controller_OnPlayerJump;
    }

    private void Update()
    {
        if (movement_Controller.isGrounded())
        {
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
        anim.SetFloat("VelocityX/Z", pos.magnitude - pos.y);
    }


    private void Controller_OnPlayerJump(bool obj)
    {
        anim.SetBool("IsJumping", obj);
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

    private void OnDestroy()
    {
        controller.OnPlayerMove -= Controller_OnPlayerMove;
        controller.OnPlayerJump -= Controller_OnPlayerJump;
        controller.OnPlayerSprint -= Controller_OnPlayerSprint;
    }
}