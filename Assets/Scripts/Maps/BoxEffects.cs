using UnityEngine;
using System.Collections;

public abstract class BoxEffects : MonoBehaviour
{
    // 移动速度
    [SerializeField] protected float Speed;
    // 死亡地点
    [SerializeField] protected Transform end;

    public virtual void EffectPlayer(GroupPlayer groupPlayer)
    {
        if (!groupPlayer.IsAlive || groupPlayer.IsInvincible)
            return;

    }

    public virtual void EndPointChange(Transform point)
    {
        end = point;
    }
}
