using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : MonoBehaviour {

    // 所有的组
    public List<GroupPlayer> groups;
    // 玩家距离边框的最小半径
    public float boardRadius;
    // 最大偏移角
    public float maxBiasDegree;

    // 竖直角度
    public float verticalDegree = 42.0f;
    // 当前相机到目标的距离
    public float curCam2TargetLength = 56.0f;
    // 相机到目标的最小距离
    public float minCam2TargetLength = 42.0f;
    // 相机到目标的最大距离
    public float maxCam2TargetLength = 62.0f;

    // 逆时针角度
    public float ccwDegree = 45;
    // 水平逆时针旋转角(弧度表示)
    private float ccwRadian = Mathf.PI * 3 / 2;

    // 改变镜头远近的速度
    public float changeCamLenSpeed = 0.02f;
    // 上下左右移动镜头的速度
    public float camMoveSpeed = 0.2f;

    private Vector3 offset = Vector3.zero;
    
    // 测试目标对象
    public Transform target;
    
    private void Start ()
    {
        ccwRadian = ccwDegree / 180 * Mathf.PI;
    }
	
	private void Update ()
    {

    }
    
    // 计算所有角色包围盒的投影
    private void PlayerBoundBox(List<GroupPlayer> gps)
    {
        float yMax = float.NegativeInfinity, yMin = float.PositiveInfinity;
        float xMax = float.NegativeInfinity, xMin = float.PositiveInfinity;
        List<Vector3> viewportPoses = new List<Vector3>();
        foreach (GroupPlayer gp in gps)
        {
            Vector3 gpPos = gp.transform.position;

            Vector3 gpViewportPosForward = Camera.main.WorldToViewportPoint(gpPos + Vector3.forward * boardRadius);
            Vector3 gpViewportPosBackward = Camera.main.WorldToViewportPoint(gpPos + Vector3.back * boardRadius);
            Vector3 gpViewportPosLeft = Camera.main.WorldToViewportPoint(gpPos + Vector3.left * boardRadius);
            Vector3 gpViewportPosRight = Camera.main.WorldToViewportPoint(gpPos + Vector3.right * boardRadius);

            viewportPoses.Add(gpViewportPosForward);
            viewportPoses.Add(gpViewportPosBackward);
            viewportPoses.Add(gpViewportPosLeft);
            viewportPoses.Add(gpViewportPosRight);

            foreach (Vector3 viewportPos in viewportPoses)
            {
                
            }
        }
    }

    void ChangeCam2TargetLength()
    {

    }

    void ReCalculateOffset()
    {
        float verticalRadian = (90 - verticalDegree) / 180 * Mathf.PI;
        offset = new Vector3(Mathf.Sin(verticalRadian) * Mathf.Cos(ccwRadian), Mathf.Cos(verticalRadian), Mathf.Sin(verticalRadian) * Mathf.Sin(ccwRadian)) * curCam2TargetLength;
    }

    void ChangeTransform()
    {
        if (Time.timeScale != 0)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime);
            transform.forward = -offset;
        }
        else
        {
            FastRepos();
        }
    }

    void FastRepos()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, camMoveSpeed);
        transform.forward = -offset;
    }

}
