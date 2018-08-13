using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem Trail;
    public ParticleSystem markCircle;

    // 画线
    private LineRenderer lineRenderer;
    // 画的线的颜色
    public Color lineColor;
    //wwq 爆炸半径
    public float explosionRadius;
    // 力度
    public float radio = 7;

    // 检查半径，如果以该半径为球中没有火球物品就生成火球物品
    public float fireBallItemRadius = 6f;
    // 生成的火球
    public GameObject fireBallItem;

    // 陨石掉落音效
    public AudioClip dropAudioClip;
    // 陨石爆炸音效
    public AudioClip exposionAudioClip;
    private AudioSource myAudioSource;

    // 陨石控制器毁掉，判断能否生成物品
    public delegate bool JudgeAction();
    public JudgeAction beableToInstantiateFireBall;

    public System.Action increaseFireBallNumbers;
    public System.Action decreaseFireBallNumbers;

    private void Start()
    {
        ParticleSystem trailSystem = Instantiate(Trail, transform);
        trailSystem.transform.position = transform.position;
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.material.SetColor("_TintColor", lineColor);
        lineRenderer.widthMultiplier = 0.1f;

        if(dropAudioClip != null)
        {
            if (myAudioSource == null)
                myAudioSource = GetComponent<AudioSource>();
            if (myAudioSource == null)
                myAudioSource = gameObject.AddComponent<AudioSource>();
            myAudioSource.clip = dropAudioClip;
            myAudioSource.Play();
        }
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = hit.point;
            lineRenderer.SetPositions(points);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("line explosion" + other.gameObject.tag);
        if (other.gameObject.tag == "Plane"|| other.gameObject.tag == "Player")
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity).gameObject, 1);
            explode();
        }
    }

    // 为玩家添加速度
    private void explode()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag != "Player") continue;

            Vector3 velocityDir = col[i].transform.position - transform.position;
            velocityDir.y = 0;

            Vector3 explosionvelocity = (explosionRadius - velocityDir.magnitude) * velocityDir.normalized * radio;
            GroupPlayer gp = col[i].GetComponent<GroupPlayer>();
            gp.EffectSpeedMovement(explosionvelocity);
        }

        if(beableToInstantiateFireBall())
            CreateFireBallItem();

        if(myAudioSource != null)
        {
            if (myAudioSource.isPlaying)
                myAudioSource.Stop();
            myAudioSource.clip = exposionAudioClip;
            myAudioSource.Play();
        }

        Destroy(this.gameObject);
    }

    public void CreateFireBallItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, fireBallItemRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "SkillItem")
                return;
        }

        Vector3 finalInstantiatePos = Vector3.zero;

        int layerMask = 1 << 9;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f, layerMask))
        {
            Vector3 hitPos = hit.point;
            finalInstantiatePos = hitPos + Vector3.up * 1.2f;
        }

        GameObject item = Instantiate(fireBallItem);
        item.transform.position = finalInstantiatePos;
        item.GetComponent<SkillItem>().noticeGenerator = decreaseFireBallNumbers;

        increaseFireBallNumbers();

    }

    private void CreateFinalBoom(AudioClip finalAudioClip)
    {
        float audioLength = finalAudioClip.length;
        GameObject finalBoom = new GameObject();
        AudioSource boomSource = finalBoom.AddComponent<AudioSource>();
        boomSource.clip = finalAudioClip;
        boomSource.Play();
    }

}
