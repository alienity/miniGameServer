using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SnowBall
{
    // 爆炸半径
    public float boomRadius = 6f;
    // 半径威力加成
    public float explosionRatio = 2;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int otherId = other.GetComponent<GroupPlayer>().gId;
            if (!canTouchSelf && (otherId == attackerId)) return;
        }

        if (isHitted) return;
        isHitted = true;

        ShowHitParticleEffects(impactNormal);

        Collider[] colliders = Physics.OverlapSphere(transform.position, boomRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.tag == "Player")
            {
                Debug.Log(collider.gameObject.name);
                GroupPlayer aroundPlayer = collider.GetComponent<GroupPlayer>();
                
                Vector3 explosionDir = aroundPlayer.transform.position - transform.position;
                aroundPlayer.EffectSpeedMovement(explosionDir.normalized * explosionRatio * (suddenSpeed + chargeAttackTime * attackStrength));
                aroundPlayer.SetAttacker(attackerId); // 设置攻击者Id
            }
        }

        DestroySelf();
    }

}
