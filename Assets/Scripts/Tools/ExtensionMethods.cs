using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 为了敌人攻击的时候判定玩家是否在攻击的扇形范围内。超过就攻击无效
public static class ExtensionMethods
{
    private const float dotThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
