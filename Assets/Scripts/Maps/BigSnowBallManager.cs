using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowBallManager : MonoBehaviour {

    public static BigSnowBallManager Instance { get; private set; }

    public GameObject bigSnowBall;
    // 每次生成时间间隔
    public float spawnCD = 3f;
    // 出生地
    public Transform[] spawnPoints;
    // 死亡地
    public Transform[] endPoint;

    //private BigSnowball bigSnowball;
    // 当前可以生成新的雪球
    public bool isReBornBigSnowBall = true;
    public float pastSpawnCD;

    private void Awake()
    {
        Instance = this;
    }
    private void Start ()
    {
        //bigSnowball = BigSnowball.Instance;
        //if (bigSnowball == null)
        //    Debug.LogError("bigSnowball == null");
        pastSpawnCD = 0;
        //InvokeRepeating("SpawnBigSnowball", spawnTime, spawnTime);
    }

    private void Update()
    {
        if (isReBornBigSnowBall == true)
        {
            pastSpawnCD += Time.deltaTime;
            if (pastSpawnCD >= spawnCD)
            {
                pastSpawnCD = 0;
                isReBornBigSnowBall = false;
                SpawnBigSnowball();
            }
        }
    }
    public void SpawnBigSnowball()
    {
        int pointIndex = Random.Range(0, spawnPoints.Length);

        GameObject ball = Instantiate(bigSnowBall, spawnPoints[pointIndex].position, spawnPoints[pointIndex].rotation);
        BigSnowball bigSnowball = ball.GetComponent<BigSnowball>();
        bigSnowball.EndPointChange(endPoint[pointIndex]);
        //bigSnowball.EndPointChange(endPoint[pointIndex]);
    }

    public void IsReBornBigSnowBall(bool status)
    {
        isReBornBigSnowBall = status;
    }
}
