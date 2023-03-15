using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer, HitEnemy, HitNothing}

    public RockStates rockStates;
    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    private Vector3 direction;
    public int damage;
    public GameObject rockBreakEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1.0f)
        {
            rockStates = RockStates.HitNothing;
        }
    }
    public void FlyToTarget()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>().gameObject;
            Debug.Log("AttackTarget == Null");
        }
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (rockStates)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag(Tags.Player))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());
                     
                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    //Debug.Log("Enemy  ‹…À");
                    var otherStats = other.gameObject.GetComponent<CharacterStats>();
                    otherStats.TakeDamage(damage, otherStats);
                    Instantiate(rockBreakEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);

                }
                break;
        }
    }
}
