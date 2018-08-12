using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTree : MonoBehaviour
{
    // 南瓜id
    [HideInInspector] public int pumpkinId;

    // 最大生命值
    public short lifeNumber = 3;

    // 出生从最小放大到正常时长
    public float startScaleDuring = 0.5f;

    // 爆炸的缩放
    public float readyToExplodeScale = 1.2f;
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

    // 死亡后通知管理器
    public delegate void noticeViwer(int id);
    public noticeViwer noticeFunc;

    // 南瓜模型对象
    public Transform modelTrans;
    // 原始缩放
    [HideInInspector] public Vector3 originalScale;

    private AudioSource selfAudioSource;
    private short totalLife;

    public void Start()
    {
        totalLife = lifeNumber;
        selfAudioSource = gameObject.AddComponent<AudioSource>();
        selfAudioSource.clip = explosionAudio;
        
        originalScale = modelTrans.localScale;
        modelTrans.localScale = originalScale * 0.1f;
        modelTrans.DOScale(originalScale, startScaleDuring);
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
        if (noticeFunc != null)
            noticeFunc(pumpkinId);
        modelTrans.DOScale(originalScale * 0.1f, 0.3f);
        Destroy(this.gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball" || other.gameObject.tag == "FireBall")
        {
            if (lifeNumber == 0) return;

            // 播放被击打音效
            selfAudioSource.clip = hittedAudio;
            selfAudioSource.Play();

            modelTrans.DOShakeScale(0.2f, 0.2f);

            Vector3 symmetryAxis = (gameObject.transform.position - other.transform.position).normalized;
            Vector3 forward = other.transform.forward;

            Vector3 newVector3 = -2 * Vector3.Dot(forward, symmetryAxis) * symmetryAxis - forward;
            newVector3.y = 0;
            other.transform.forward = newVector3;
            
            lifeNumber--;

            if (lifeNumber == 0)
            {
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(modelTrans.DOShakeScale(explosionDelay, readyToExplodeScale))
                    .AppendCallback(()=> {
                        explode();
                    });
            }
        }
    }

}
