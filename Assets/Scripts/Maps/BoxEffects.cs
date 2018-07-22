using UnityEngine;
using System.Collections;

public class BoxEffects : MonoBehaviour
{

    public virtual void EffectPlayer(GroupPlayer groupPlayer)
    {
        if (!groupPlayer.IsAlive || groupPlayer.IsInvincible)
            return;

    }
    
}
