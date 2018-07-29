using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyCloud : MonoBehaviour
{

	public struct Cloud
	{
		public GameObject cloud;
		public float rotateRadian;
		public float AngularSpeed;
		public float radius;
	}

    [SerializeField] private GameObject skyCloud;  //云纹理
    [SerializeField] private Transform center;     //天空的中心
    [SerializeField] private float minRadius;      //最小距离中心点
    [SerializeField] private float maxRadius;      //最大距离中心点
    [SerializeField] private int cloudNum;         //云朵的数目
    [SerializeField] private float minSpeed;       //最小旋转速度
    [SerializeField] private float maxSpeed;       //最大旋转速度

    [SerializeField] private List<Cloud> clouds;

	void Start ()
	{
		SpawnCloud ();
	}

	void Update ()
	{
		for(int i = 0; i < clouds.Count; ++i){
			var m_cloud = clouds [i];
			m_cloud.rotateRadian += Time.deltaTime * m_cloud.AngularSpeed;
			Vector3 pos = center.position;
			pos.x += Mathf.Cos (m_cloud.rotateRadian) * m_cloud.radius;
			pos.z += Mathf.Sin (m_cloud.rotateRadian) * m_cloud.radius;
			pos.y = m_cloud.cloud.transform.position.y;
			m_cloud.cloud.transform.position = pos;
			m_cloud.cloud.transform.LookAt (center.position);
			clouds [i] = m_cloud;
		}
	}

	void SpawnCloud() 
	{
		clouds = new List<Cloud> ();
		for(int i = 0; i < cloudNum; ++i) {
			float verticalRadius = Random.Range (0, 2 * Mathf.PI);
			float horizontalRadius = Random.Range (Mathf.PI / 26, Mathf.PI / 3);
			float currentRadius = Random.Range (minRadius, maxRadius);
			Vector3 disDir = Vector3.zero;
			disDir.y = -currentRadius * Mathf.Sin (horizontalRadius);
			disDir.x = currentRadius * Mathf.Cos (horizontalRadius) * Mathf.Cos (verticalRadius);
			disDir.z = currentRadius * Mathf.Cos (horizontalRadius) * Mathf.Sin (verticalRadius);
			Vector3 finalPos = center.position + disDir;
			GameObject cloud = Instantiate (skyCloud, finalPos, Quaternion.identity) as GameObject;
			cloud.transform.parent = transform;
			Cloud m_cloud = new Cloud ();
			m_cloud.cloud = cloud;
			m_cloud.rotateRadian = currentRadius * Mathf.Cos (horizontalRadius);
			m_cloud.AngularSpeed = Random.Range (minSpeed, maxSpeed);
			m_cloud.radius = currentRadius * Mathf.Cos (horizontalRadius);
			clouds.Add (m_cloud);
		}
	}

}
