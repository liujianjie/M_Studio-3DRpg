using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attack/Attack Data", fileName = "New Attack Data")]
public class Attack_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    public float criticalMultiplier;
    public float criticalChance;
}
