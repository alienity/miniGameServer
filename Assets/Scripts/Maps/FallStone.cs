using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem Trail;
    public ParticleSystem markCircle;

    // ����
    private LineRenderer lineRenderer;
    // �����ߵ���ɫ
    public Color lineColor;
    //wwq ��ը�뾶
    public float explosionRadius;
    // ����
    public float radio = 7;

    // �����ϵ�Ȧ
    //private ParticleSystem groundCircle;

    private void Start()
    {
        ParticleSystem trailSystem = Instantiate(Trail, transform);
        trailSystem.transform.position = transform.position;
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.material.SetColor("_TintColor", lineColor);
        lineRenderer.widthMultiplier = 0.1f;
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("line explosion" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Plane"|| collision.gameObject.tag == "Player")
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity).gameObject, 1);
            explode();
        }
    }

    // Ϊ�������ٶ�
    private void explode()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
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
        Destroy(this.gameObject);
    }
}
