using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 30;

    public GameObject rockPrefab;
    public Transform handPos;
    // »÷·É
    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            var targetStates = attackTarget.GetComponent<CharacterStats>();
            targetStates.TakeDamage(characterStats, targetStates, true);
        }
    }
    public void ThrowRock()
    {
        var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
        rock.GetComponent<Rock>().target = attackTarget;

    }
}
