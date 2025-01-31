using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation_Controller : MonoBehaviour
{
    [Header("Anim Setup")]
    [SerializeField] private Animator anim;
    //TODO: TP2 - Syntax - Consistency in naming convention
    [SerializeField] private Enemy_Controller controller;

    public event Action OnBulletSpawn;
    // Start is called before the first frame update
    //TODO: TP2 - Syntax - Consistency in access modifiers (private/protected/public/etc)
    void Start()
    {
        

        controller.OnEnemyMove += Controller_OnEnemyMove;
        controller.OnEnemyAttack += Controller_OnEnemyAttack;
        controller.OnEnemyHit += Controller_OnEnemyHit;
        controller.OnEnemyDeath += Controller_OnEnemyDeath;
    }

    private void Controller_OnEnemyDeath()
    {
        anim.Play("Standing React Death Backward");
    }

    private void Controller_OnEnemyHit()
    {
        anim.Play("Get_Hit");
    }

    private void Controller_OnEnemyAttack()
    {
        anim.Play("Standing 1H Magic Attack 01");
    }

    private void Controller_OnEnemyMove(Vector2 obj)
    {
        Vector3 pos = new Vector3(obj.x, 0f, obj.y);
        //TODO - Fix - shouldn't it be (pos * new Vector3(1, 0, 1).magnitude ?
        anim.SetFloat("Velocity_X/Z",pos.magnitude - pos.y);
    }

    public void Spawn_Bullet() 
    {
        Debug.Log("Bullet Invoke");
        OnBulletSpawn.Invoke();
    }

    private void OnDestroy()
    {
        controller.OnEnemyMove -= Controller_OnEnemyMove;
        controller.OnEnemyAttack -= Controller_OnEnemyAttack;
        controller.OnEnemyHit -= Controller_OnEnemyHit;
        controller.OnEnemyDeath -= Controller_OnEnemyDeath;
    }
}
