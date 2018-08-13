using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour {

    // 冰面
    public GameObject icePlane;
    // 地板
    public MeshRenderer planeMeshRender;
    // 地板材质
    private Material planeMaterial;
    // 天气粒子
    public ParticleSystem snowParticles;
    // 季节转换时间
    public float seasonDuring;
    // 当前季节 0表示春天 1表示冬天
    private int season = 0;
    
    private void Start ()
    {
        icePlane.SetActive(false);
        planeMaterial = planeMeshRender.material;
    }
    
    // 通过时间更新
    public void UpdateAccordingTime()
    {
        if(season == 0)
        {
            season = 1;
            ChangeWeather();
        }   
    }

    // 改变天气
    public void ChangeWeather()
    {
        StartCoroutine(GradientDissovle(seasonDuring));
    }

    // 溶解变换协程
    IEnumerator GradientDissovle(float during)
    {
        snowParticles.Play(true);
        float totalDuring = during;
        while (during > 0)
        {
            during -= Time.deltaTime;
            float dissolveRatio = during / totalDuring * 2 - 1;
            planeMaterial.SetFloat("_DissolveRatio", dissolveRatio);
            yield return new WaitForEndOfFrame();
        }
        icePlane.SetActive(true);
        snowParticles.Stop();
    }

}
