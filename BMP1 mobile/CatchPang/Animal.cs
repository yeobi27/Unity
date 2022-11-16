using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private NavMeshAgent navMeshAgent;

    public float health;
    public float damage;
    public float points;
    public string getCoin;

    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(Run());
    }
    
    public void HitPlayer()
    {
        CatchPang_PlayerController.Instance.GetHit();
    }

    private IEnumerator Run()
    {
        yield return new WaitUntil(() => CatchPang_Timer.isPlaying);

        navMeshAgent.SetDestination(CatchPang_PlayerController.Instance.transform.position);
        animator.SetBool("Running", true);

        while (GetDistanceToPlayer() > 2.5f)
            yield return null;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        navMeshAgent.isStopped = true;

        animator.SetBool("Running", false);
        animator.SetBool("Attacking", true);

        yield return null;
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Balls"))
        {
            // hit sound
            CatchPang_SoundManager.Instance.PlaySE(getCoin);

            ReceiveDamage();
        }
    }

    void ReceiveDamage()
    {
        health -= CatchPang_DataManager.Instance.projectileDamage;

        CatchPang_DataManager.Instance.AddScore(points * GetDistanceToPlayer());

        if (health <= 0) Perish();
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, CatchPang_PlayerController.Instance.transform.position);
    }

    private void Perish()
    {
        // animator.SetTrigger("Death");

        Destroy(gameObject);
    }

}
