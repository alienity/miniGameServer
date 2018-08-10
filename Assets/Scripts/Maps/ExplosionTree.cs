using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTree : BoxEffects
{
    // 最大生命值
    public short lifeNumber = 3;
    // 爆炸延迟
    public float explosionDelay = 2f;
    // 爆炸范围
    public float explosionRadius = 50;
    // 爆炸强度
    public float radio = -30.5f;
    // 爆炸音效
    public AudioClip explosionAudio;
    // 被击打音效
    public AudioClip hittedAudio;
    // 爆炸粒子效果
    public ParticleSystem explosionParticles;
    // 重生延迟
    public float rebornDelay = 5;
    // 重生特效播放时间
    public float rebornAnimDuring = 2;

    private AudioSource selfAudioSource;
    private short totalLife;

    public void Start()
    {
        totalLife = lifeNumber;
        selfAudioSource = gameObject.AddComponent<AudioSource>();
        selfAudioSource.clip = explosionAudio;
    }

    private void Update()
    {
        //rebornPastTime += Time.deltaTime;
        //if(lifeNumber == 0 && rebornPastTime > rebornDelay)
        //{
        //    lifeNumber = totalLife;
        //    ResetExplosionTree();
        //}
    }

    // 受到攻击爆炸
    private void explode()
    {
        selfAudioSource.clip = explosionAudio;
        selfAudioSource.Play();

        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
        explosionParticles.Play(true);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag != "Player") continue;

            Vector3 velocityDir = transform.position - col[i].transform.position;
            velocityDir.y = 0;

            Vector3 explosionvelocity = (explosionRadius - velocityDir.magnitude) * velocityDir.normalized;
            explosionvelocity = -radio * explosionvelocity;
            GroupPlayer gp = col[i].GetComponent<GroupPlayer>();
            gp.EffectSpeedMovement(explosionvelocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            if (lifeNumber == 0) return;

            // 播放被击打音效
            selfAudioSource.clip = hittedAudio;
            selfAudioSource.Play();

            Vector3 symmetryAxis = (gameObject.transform.position - other.transform.position).normalized;
            Vector3 forward = other.transform.forward;

            Vector3 newVector3 = -2 * Vector3.Dot(forward, symmetryAxis) * symmetryAxis - forward;
            newVector3.y = 0;
            other.transform.forward = newVector3;
            
            lifeNumber--;

            if (lifeNumber == 0)
                StartCoroutine(CountDownToExplode(explosionDelay));
        }
    }

    IEnumerator CountDownToExplode(float during)
    {
        Debug.Log("播放特效");
        yield return new WaitForSeconds(during);
        explode();
        
        ResetExplosionTree();
    }

    // 重置南瓜
    public void ResetExplosionTree()
    {
        StartCoroutine(CountDownToReborn(rebornDelay, rebornAnimDuring));
    }

    IEnumerator CountDownToReborn(float delay, float during)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("播放重生动画");
        yield return new WaitForSeconds(during);
        lifeNumber = totalLife;
    }

    // 更改模型
    public void ExchangeModel(Mesh modelMesh)
    {
        // 冬天会把模型变更成雪人
    }

}
