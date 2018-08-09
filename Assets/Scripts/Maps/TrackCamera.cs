using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : MonoBehaviour {

    public static TrackCamera Instance { get; private set; }

    // 所有的组
    public List<Transform> groups;

    // 玩家距离边框的最小半径
    public float boardRadius;
    // 相机到目标的最小距离
    public float minCam2TargetLength = 42.0f;
    // 相机到目标的最大距离
    public float maxCam2TargetLength = 62.0f;

    // 镜头移动速度
    public float camMoveSpeed = 6f;
    // 镜头的朝向的反向
    private Vector3 toCameraDirection;

    //// 竖直角度
    //public float verticalDegree = 42.0f;
    //// 逆时针角度
    //public float ccwDegree = 45;
    //// 水平逆时针旋转角(弧度表示)
    //private float ccwRadian;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //ccwRadian = ccwDegree * Mathf.Deg2Rad;
        toCameraDirection = -1 * Camera.main.transform.forward;
    }

    private void Update()
    {
        //Vector3 toCameraDirection = ReCalculateCameraDirection();
        Vector3 finalPos = PlayerBoundBox_1(groups, toCameraDirection);
        ChangeTransform(finalPos, toCameraDirection);
    }

    private Vector3 PlayerBoundBox_1(List<Transform> gps, Vector3 toCameraDirection)
    {
        Vector3 tmpWorldCenterPos = Vector3.zero;
        List<Vector3> worldGps = new List<Vector3>();
        foreach (Transform gp in gps)
        {
            tmpWorldCenterPos += gp.position;
            worldGps.Add(gp.position);
        }
        tmpWorldCenterPos /= gps.Count;

        Vector3 tmpOrigin = Camera.main.transform.position;
        Camera.main.transform.position = tmpWorldCenterPos;
        Matrix4x4 worldToCamTmp = Camera.main.worldToCameraMatrix;
        Camera.main.transform.position = tmpOrigin;

        Vector3 tmpCamCenterPos = worldToCamTmp.MultiplyPoint(Vector3.one);

        Debug.DrawRay(Vector3.zero, worldToCamTmp.inverse.MultiplyVector(Vector3.forward), Color.blue);
        Debug.DrawRay(Vector3.zero, worldToCamTmp.inverse.MultiplyVector(Vector3.right), Color.red);
        Debug.DrawRay(Vector3.zero, worldToCamTmp.inverse.MultiplyVector(Vector3.up), Color.green);

        float yTan = Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float xTan = Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * Camera.main.aspect;

        float curCam2TargetLength = -1;

        foreach (Vector3 tmpWorldPos in worldGps)
        {
            Vector3 tmpCamPos = worldToCamTmp.MultiplyPoint(tmpWorldPos);

            float tmpOffsetDis = (Mathf.Abs(tmpCamPos.x) + boardRadius) / xTan;
            tmpOffsetDis += tmpCamPos.z;
            curCam2TargetLength = curCam2TargetLength > tmpOffsetDis ? curCam2TargetLength : tmpOffsetDis;

            tmpOffsetDis = (Mathf.Abs(tmpCamPos.y) + boardRadius) / yTan;
            tmpOffsetDis += tmpCamPos.z;
            curCam2TargetLength = curCam2TargetLength > tmpOffsetDis ? curCam2TargetLength : tmpOffsetDis;
        }

        curCam2TargetLength = curCam2TargetLength > minCam2TargetLength ? curCam2TargetLength : minCam2TargetLength;
        curCam2TargetLength = curCam2TargetLength < maxCam2TargetLength ? curCam2TargetLength : maxCam2TargetLength;

        return tmpWorldCenterPos + toCameraDirection * curCam2TargetLength;
    }

    //// 计算镜头的方向
    //private Vector3 ReCalculateCameraDirection()
    //{
    //    float verticalRadian = (90 - verticalDegree) / 180 * Mathf.PI;
    //    ccwRadian = ccwDegree * Mathf.Deg2Rad;
    //    Vector3 cameraDirection = new Vector3(Mathf.Sin(verticalRadian) * Mathf.Cos(ccwRadian), Mathf.Cos(verticalRadian), Mathf.Sin(verticalRadian) * Mathf.Sin(ccwRadian));
    //    return cameraDirection;
    //}

    // 改变镜头位置
    void ChangeTransform(Vector3 finalPos, Vector3 toCameraDirection)
    {
        if (Time.timeScale != 0)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, camMoveSpeed * Time.deltaTime);
            transform.forward = -toCameraDirection;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, camMoveSpeed);
            transform.forward = -toCameraDirection;
        }
    }

}
